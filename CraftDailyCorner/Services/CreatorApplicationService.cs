using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Services.Interface;
using CraftDailyCorner.ViewModels.CreatorApplication;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CraftDailyCorner.Services.Creator
{
    public class CreatorApplicationService : ICreatorApplicationService
    {
        private readonly CraftDailyCornerContext _context;
        private readonly IImageUploadService _imageUploadService;

        public CreatorApplicationService(
            CraftDailyCornerContext context,
            IImageUploadService imageUploadService)
        {
            _context = context;
            _imageUploadService = imageUploadService;
        }

        // 取得申請頁應顯示的畫面
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

        // 是否有審核中的申請
        public async Task<bool> HasPendingAsync(string memberId)
        {
            return await _context.CreatorApplications
                .Include(ca => ca.CreatorApplicationStatus)
                .AnyAsync(ca =>
                    ca.MemberID == memberId &&
                    ca.CreatorApplicationStatus.StatusCode == "Pending");
        }

        // 建立前先做驗證，讓 Controller 可先驗證再上傳圖片
        public async Task ValidateBeforeCreateAsync(string memberId, string brandName)
        {
            if (await HasPendingAsync(memberId))
                throw new InvalidOperationException("已有審核中的申請");

            if (await IsBrandNameDuplicateAsync(brandName, memberId))
                throw new InvalidOperationException("品牌名稱已被使用，請更換其他名稱");
        }

        // 建立申請
        public async Task CreateAsync(CreatorApplicationCreateDTO dto)
        {
            // 第二層保險，避免未來其他地方直接呼叫 CreateAsync 時漏掉檢查
            await ValidateBeforeCreateAsync(dto.MemberId, dto.BrandName);

            var pendingStatus = await _context.CreatorApplicationStatuses
                .Where(s => s.StatusCode == "Pending" && s.IsActive)
                .FirstOrDefaultAsync();

            if (pendingStatus == null)
                throw new Exception("系統狀態設定錯誤：找不到 Pending 狀態");

            var entity = new CreatorApplication
            {
                MemberID = dto.MemberId,
                BrandName = (dto.BrandName ?? string.Empty).Trim(),
                BrandIntro = dto.BrandIntro,
                PortfolioSampleUrl = dto.PortfolioSampleUrl,
                StartDate = dto.StartDate,
                AppliedAt = DateTime.Now,
                StatusID = pendingStatus.StatusID
            };

            _context.CreatorApplications.Add(entity);
            await _context.SaveChangesAsync();
        }

        // 取得會員最新一筆申請
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

            // 必須是「已通過」(StatusID = 2) 才能填資料建立 CreatorProfile
            if (app.StatusID != 2) return null;

            return new VMApprovedConfirm
            {
                ApplicationID = app.ApplicationID,
                BrandName = app.BrandName ?? string.Empty,
                BrandIntro = app.BrandIntro ?? string.Empty,
                StartDate = app.StartDate,
                BankCode = string.Empty,
                BankAccount = string.Empty
            };
        }

        public async Task SubmitApprovedConfirmAsync(string memberId, VMApprovedConfirm vm)
        {
            var app = await _context.CreatorApplications
                .FirstOrDefaultAsync(x => x.ApplicationID == vm.ApplicationID && x.MemberID == memberId);

            if (app == null)
                throw new Exception("找不到申請資料");

            // 必須是「已通過」(StatusID = 2) 才能進入此流程
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
                    ImageSizePresets.Creator,
                    entityId: null,
                    entitySubFolder: creatorId
                );

                // 建立 CreatorProfile
                _context.CreatorProfiles.Add(new CreatorProfile
                {
                    CreatorID = creatorId,
                    MemberID = memberId,
                    BrandName = (app.BrandName ?? string.Empty).Trim(),
                    BrandIntro = (app.BrandIntro ?? string.Empty).Trim(),
                    StartDate = app.StartDate,
                    BankCode = (vm.BankCode ?? string.Empty).Trim(),
                    BankAccount = (vm.BankAccount ?? string.Empty).Trim(),
                    StatusID = 1, // 依你 Seed：1 = 啟用
                    ImageUrl = imageKey ?? "default",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                });

                // 建立預設訊息模板
                AddDefaultMessageTemplates(creatorId);

                // 若管理者審核通過時沒有掛 Role(02)，這裡補一次
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

                    var operatorId = string.IsNullOrWhiteSpace(app.ReviewedBy)
                        ? null
                        : app.ReviewedBy;

                    _context.MemberRoleHistories.Add(new MemberRoleHistory
                    {
                        Action = (MemberRoleHistoryAction)1,
                        OperatedAt = DateTime.Now,
                        MemberID = memberId,
                        RoleID = "02",
                        OperatedBy = (MemberRoleHistoryOperated)1,
                        OperatorMemberID = operatorId
                    });
                }

                // 更新申請狀態 => Confirm(StatusID = 4)
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

            // 必須是「已拒絕」(StatusID = 3) 才能進入此頁確認
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

            var newId = (outputParam.Value?.ToString() ?? string.Empty).Trim();

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
                new AutoReplyTemplate
                {
                    Title = "首次訊息自動回覆",
                    Content = "您好，感謝您的來訊！我會盡快回覆您，謝謝您的耐心等候。",
                    IsActive = true,
                    TriggerType = AutoReplyTemplateTriggerType.FirstMessage,
                    CreatedAt = now,
                    CreatorID = creatorId
                },
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

        private async Task<bool> IsBrandNameDuplicateAsync(string brandName, string? excludeMemberId = null)
        {
            if (string.IsNullOrWhiteSpace(brandName))
                return false;

            var normalized = brandName.Trim();

            // 1. 檢查正式創作者資料
            var existsInProfiles = await _context.CreatorProfiles
                .AnyAsync(x => x.BrandName == normalized);

            if (existsInProfiles)
                return true;

            // 2. 檢查創作者申請資料
            var appQuery = _context.CreatorApplications
                .Where(x => x.BrandName == normalized && (x.StatusID == 1 || x.StatusID == 2));

            if (!string.IsNullOrWhiteSpace(excludeMemberId))
            {
                appQuery = appQuery.Where(x => x.MemberID != excludeMemberId);
            }

            return await appQuery.AnyAsync();
        }
    }
}