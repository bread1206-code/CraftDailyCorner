using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorApplication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using CraftDailyCorner.Models.enums;
using System.Data;

namespace CraftDailyCorner.Services.Creator
{
    public class CreatorApplicationService : ICreatorApplicationService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;

        public CreatorApplicationService(CraftDailyCornerContext context, IImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        //取得申請頁應顯示的畫面
        public async Task<object> GetApplyPageAsync(string memberId)
        {
            var latest = await _context.CreatorApplications
                .Include(ca => ca.CreatorApplicationStatus)
                .Where(ca => ca.MemberID == memberId)
                .OrderByDescending(ca => ca.AppliedAt)
                .FirstOrDefaultAsync();

            // 尚未申請過
            if (latest == null)
            {
                return new VMCreatorApplicationApply
                {
                    StartDate = DateTime.Today
                };
            }

            var statusCode = latest.CreatorApplicationStatus.StatusCode;

            switch (statusCode)
            {
                case "Pending":
                    return new VMCreatorApplicationPending
                    {
                        DisplayName = latest.BrandName,
                        AppliedAt = latest.AppliedAt
                    };

                case "Approved":
                    return new VMCreatorApplicationApproved
                    {
                        DisplayName = latest.BrandName,
                        ReviewedAt = latest.ReviewedAt ?? latest.AppliedAt
                    };

                default:
                    return new VMCreatorApplicationApply
                    {
                        StartDate = DateTime.Today
                    };
            }
        }

        //是否有審核中的申請
        public async Task<bool> HasPendingAsync(string memberId)
        {
            return await _context.CreatorApplications
                .Include(ca => ca.CreatorApplicationStatus)
                .AnyAsync(ca =>
                    ca.MemberID == memberId &&
                    ca.CreatorApplicationStatus.StatusCode == "Pending");
        }

        //建立申請
        public async Task CreateAsync(CreatorApplicationCreateDTO dto)
        {
            if (await HasPendingAsync(dto.MemberId))
                throw new InvalidOperationException("已有審核中的申請");

            var pendingStatus = await _context.CreatorApplicationStatuses
                .Where(s => s.StatusCode == "Pending" && s.IsActive)
                .FirstOrDefaultAsync();

            if (pendingStatus == null)
                throw new Exception("系統狀態設定錯誤：找不到 Pending 狀態");

            var entity = new CreatorApplication
            {
                MemberID = dto.MemberId,
                BrandName = dto.BrandName,
                BrandIntro = dto.BrandIntro,
                PortfolioSampleUrl = dto.PortfolioSampleUrl,
                StartDate = dto.StartDate,
                AppliedAt = DateTime.Now,
                StatusID = pendingStatus.StatusID
            };

            _context.CreatorApplications.Add(entity);
            await _context.SaveChangesAsync();
        }

        //取得會員最新一筆申請
        public async Task<CreatorApplication?> GetLatestByMemberAsync(string memberId)
        {
            return await _context.CreatorApplications
                .Include(ca => ca.CreatorApplicationStatus)
                .Where(ca => ca.MemberID == memberId)
                .OrderByDescending(ca => ca.AppliedAt)
                .FirstOrDefaultAsync();
        }

        public async Task<VMApprovedConfirm?> GetApprovedConfirmAsync(string memberId, int? applicationId = null)
        {
            // 已是創作者就不該走這頁
            var isCreator = await _context.CreatorProfiles.AnyAsync(c => c.MemberID == memberId);
            if (isCreator) return null;

            var query = _context.CreatorApplications.AsNoTracking()
                .Where(x => x.MemberID == memberId);

            if (applicationId.HasValue)
                query = query.Where(x => x.ApplicationID == applicationId.Value);

            var app = await query
                .OrderByDescending(x => x.AppliedAt)
                .FirstOrDefaultAsync();

            if (app == null) return null;

            // 必須是「已通過」(StatusID=2) 才能填資料建立 CreatorProfile
            if (app.StatusID != 2) return null;

            return new VMApprovedConfirm
            {
                ApplicationID = app.ApplicationID,
                BrandName = app.BrandName ?? string.Empty,
                BrandIntro = app.BrandIntro ?? string.Empty,
                StartDate = app.StartDate,
                BankCode = "",
                BankAccount = ""
            };
        }

        public async Task SubmitApprovedConfirmAsync(string memberId, VMApprovedConfirm vm)
        {
            var app = await _context.CreatorApplications
                .FirstOrDefaultAsync(x => x.ApplicationID == vm.ApplicationID && x.MemberID == memberId);

            if (app == null)
                throw new Exception("找不到申請資料");

            // 必須是「已通過」(StatusID=2) 才能進入此流程
            if (app.StatusID != 2)
                throw new Exception("此申請狀態不可確認");

            // 已有 CreatorProfile 就不可重複建立
            var hasCreatorProfile = await _context.CreatorProfiles
                .AnyAsync(x => x.MemberID == memberId);

            if (hasCreatorProfile)
                throw new Exception("你已經是創作者，無需重複建立資料");

            using var tx = await _context.Database.BeginTransactionAsync();

            try
            {
                // 產生 CreatorID
                var creatorId = GetNewCreatorID();

                var imageKey = _imageUploadService.UploadImage(
                    vm.BrandImageFile,
                    null,
                    "03CreatorBrand",
                    ImageSizePresets.Creator
                );

                // 建立 CreatorProfile
                _context.CreatorProfiles.Add(new CreatorProfile
                {
                    CreatorID = creatorId,
                    MemberID = memberId,
                    BrandName = (app.BrandName ?? string.Empty).Trim(),
                    BrandIntro = (app.BrandIntro ?? string.Empty).Trim(),
                    StartDate = app.StartDate,
                    BankCode = vm.BankCode.Trim(),
                    BankAccount = vm.BankAccount.Trim(),
                    StatusID = 1,                 // 依你 Seed：1=啟用（若你的 CreatorProfileStatus 定義不同請改）
                    ImageUrl = imageKey ?? "default",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });

                // 建立預設訊息模板
                // 1. 固定建立 1 筆 FirstMessage 自動回覆模板
                // 2. 一併建立其他預設 QuickReply 模板
                AddDefaultMessageTemplates(creatorId);

                // 若你「管理者審核通過」時沒有掛 Role(02)，在這裡補一次最保險
                var hasRole = await _context.MemberRoles
                    .AnyAsync(r => r.MemberID == memberId && r.RoleID == "02");

                if (!hasRole)
                {
                    _context.MemberRoles.Add(new MemberRole
                    {
                        MemberID = memberId,
                        RoleID = "02",
                        AssignedAt = DateTime.Now
                    });

                    var operatorId = string.IsNullOrWhiteSpace(app.ReviewedBy) ? null : app.ReviewedBy;

                    _context.MemberRoleHistories.Add(new MemberRoleHistory
                    {
                        Action = (MemberRoleHistoryAction)1,          // 1=Add（依你 Seed 寫法）
                        OperatedAt = DateTime.Now,
                        MemberID = memberId,
                        RoleID = "02",
                        OperatedBy = (MemberRoleHistoryOperated)1,    // 1=Admin/System 視你 Seed
                        OperatorMemberID = operatorId
                    });
                }

                // 更新申請狀態 => Confirm(StatusID=4)
                app.StatusID = 4;
                await _context.SaveChangesAsync();

                await tx.CommitAsync();
            }
            catch
            {
                await tx.RollbackAsync();
                throw;
            }
        }

        public async Task<VMRejectedConfirm?> GetRejectedConfirmAsync(string memberId, int? applicationId = null)
        {
            var query = _context.CreatorApplications.AsNoTracking()
                .Where(x => x.MemberID == memberId);

            if (applicationId.HasValue)
                query = query.Where(x => x.ApplicationID == applicationId.Value);

            var app = await query
                .OrderByDescending(x => x.AppliedAt)
                .FirstOrDefaultAsync();

            if (app == null) return null;

            // 必須是「已拒絕」(StatusID=3) 才能進入此頁確認
            if (app.StatusID != 3) return null;

            return new VMRejectedConfirm
            {
                ApplicationID = app.ApplicationID,
                BrandName = app.BrandName ?? string.Empty,
                ReviewNote = app.ReviewNote ?? string.Empty
            };
        }

        public async Task SubmitRejectedConfirmAsync(string memberId, int applicationId)
        {
            var app = await _context.CreatorApplications
                .FirstOrDefaultAsync(x => x.ApplicationID == applicationId && x.MemberID == memberId);

            if (app == null)
                throw new Exception("找不到申請資料");

            if (app.StatusID != 3)
                throw new Exception("此申請狀態不可確認");

            app.StatusID = 4; // Confirm
            await _context.SaveChangesAsync();
        }
        private string GetNewCreatorID()
        {
            var outputParam = new SqlParameter
            {
                ParameterName = "@NewCreatorID",
                SqlDbType = SqlDbType.Char,
                Size = 6,
                Direction = ParameterDirection.Output
            };

            _context.Database.ExecuteSqlRaw(
                "EXEC getCreatedCreatorID @NewCreatorID OUTPUT",
                outputParam
            );

            var newId = (outputParam.Value?.ToString() ?? "").Trim();

            if (string.IsNullOrWhiteSpace(newId))
                throw new Exception("產生創作者編號失敗");

            return newId;
        }
        /// <summary>
        /// 建立新創作者的預設訊息模板
        /// 規則：
        /// 1. 每位創作者固定建立 1 筆 FirstMessage
        /// 2. 其餘建立預設 QuickReply
        /// </summary>
        private void AddDefaultMessageTemplates(string creatorId)
        {
            var now = DateTime.Now;

            var templates = new List<AutoReplyTemplate>
            {
                // =============================
                // 系統固定模板：第一次訊息自動回覆
                // 每位創作者只有一筆
                // =============================
                new AutoReplyTemplate
                {
                    Title = "首次訊息自動回覆",
                    Content = "您好，感謝您的來訊！我會盡快回覆您，謝謝您的耐心等候。",
                    IsActive = true,
                    TriggerType = AutoReplyTemplateTriggerType.FirstMessage,
                    CreatedAt = now,
                    CreatorID = creatorId
                },

                // =============================
                // 其他預設快速回覆模板（標記位置）
                // =============================
                new AutoReplyTemplate
                {
                    Title = "感謝詢問",
                    Content = "您好，感謝您的詢問，我會盡快為您確認並回覆。",
                    IsActive = true,
                    TriggerType = AutoReplyTemplateTriggerType.QuickReply,
                    CreatedAt = now,
                    CreatorID = creatorId
                },
                new AutoReplyTemplate
                {
                    Title = "確認庫存中",
                    Content = "您好，這邊幫您確認庫存中，請稍候一下，謝謝。",
                    IsActive = true,
                    TriggerType = AutoReplyTemplateTriggerType.QuickReply,
                    CreatedAt = now,
                    CreatorID = creatorId
                },
                new AutoReplyTemplate
                {
                    Title = "客製需求",
                    Content = "您好，若有客製需求，歡迎提供想法與細節，我會再與您討論。",
                    IsActive = true,
                    TriggerType = AutoReplyTemplateTriggerType.QuickReply,
                    CreatedAt = now,
                    CreatorID = creatorId
                }
            };

            _context.AutoReplyTemplates.AddRange(templates);
        }
    }
}