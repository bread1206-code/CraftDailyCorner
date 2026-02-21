using CraftDailyCorner.DTOs;
using CraftDailyCorner.Models;
using CraftDailyCorner.Models.enums;
using CraftDailyCorner.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace CraftDailyCorner.Services
{
    public class ReportService 
    {
        private readonly CraftDailyCornerContext _context;

        public ReportService(
            CraftDailyCornerContext context)
        {
            _context = context;
        }
        //檢舉留言
        public async Task<ReportResponse> ReportAsync(ReportDTO dto,string reporterId)
        {
            var response = new ReportResponse();
            //驗證會員是否存在
            var member = await _context.Members
                .FirstOrDefaultAsync(m => m.MemberID == reporterId);

            if (member == null)
            {
                response.Result = ReportResponseEnum.Forbidden;
                return response;
            }
            //檢查會員是否被暫停檢舉權限
            if (member.ReportBanUntil.HasValue &&
                member.ReportBanUntil > DateTime.Now)
            {
                // 尚在停權期間
                response.Result = ReportResponseEnum.Forbidden;
                return response;
            }
            //驗證目標內容是否存在，並取得內容擁有者ID
            var (exists, ownerId, extraId) =
                await ValidateTargetAsync(dto.ReportType, dto.TargetID);

            if (!exists)
            {
                response.Result = ReportResponseEnum.NotFound;
                return response;
            }
            //禁止檢舉自己的內容
            if (ownerId == reporterId)
            {
                response.Result = ReportResponseEnum.Forbidden;
                return response;
            }
            //檢查是否已檢舉過（Service層防呆），即使DB有Unique Index，這裡仍需檢查
            var already = await _context.Reports.AnyAsync(r =>
                r.ReportType == dto.ReportType &&
                r.TargetID == dto.TargetID &&
                r.MemberID == reporterId);

            if (already)
            {
                response.Result = ReportResponseEnum.AlreadyReported;
                return response;
            }
            //建立檢舉資料
            var report = new Report
            {
                ReportType = dto.ReportType,// 檢舉類型（留言/商品/日誌/作品集/商品評價）
                TargetID = dto.TargetID,    // 被檢舉目標ID
                MemberID = reporterId,  // 檢舉人
                ReasonCode = dto.ReasonCode,
                Description = dto.Description,
                StatusID = 1,
                CreatedAt = DateTime.Now
            };

            _context.Reports.Add(report);
            await _context.SaveChangesAsync();

            // 動態統計檢舉次數
            var count = await _context.Reports.CountAsync(r =>
                r.ReportType == dto.ReportType &&
                r.TargetID == dto.TargetID &&
                r.StatusID == 1);

            //回傳成功結果
            response.Result = ReportResponseEnum.Success;
            // 如果是留言，可回傳 PostID 讓 Controller 導頁
            response.TargetID = extraId;

            return response;
        }

        private async Task<(bool Exists, string? OwnerId, string? ExtraId)>
        ValidateTargetAsync(ReportTargetType type, string targetId)
        {
            // 根據不同的檢舉類型，切換驗證邏輯
            switch (type)
            {
                //檢舉留言
                case ReportTargetType.Comment:
                    // 查詢留言是否存在
                    var comment = await _context.PostComments
                        .FirstOrDefaultAsync(x => x.CommentID == targetId);

                    // 如果留言不存在，回傳 Exists = false
                    // 如果留言存在，回傳：Exists = true，OwnerId = 留言作者ID，ExtraId = 留言所屬的PostID（用於導頁）
                    return comment == null
                        ? (false, null, null)
                        : (true, comment.MemberID, comment.PostID);

                //檢舉日誌
                case ReportTargetType.Post:
                    var post = await _context.CreatorPosts
                        .FirstOrDefaultAsync(x => x.PostID == targetId);

                    // OwnerId = 日誌作者（CreatorID）
                    return post == null
                        ? (false, null, null)
                        : (true, post.CreatorID, null);

                //檢舉商品
                case ReportTargetType.Product:
                    var product = await _context.Products
                        .FirstOrDefaultAsync(x => x.ProductID == targetId);

                    // OwnerId = 商品創作者
                    return product == null
                        ? (false, null, null)
                        : (true, product.CreatorID, null);

                //檢舉作品集
                case ReportTargetType.Portfolio:
                    var portfolio = await _context.Portfolios
                        .FirstOrDefaultAsync(x => x.PortfolioID == targetId);

                    // OwnerId = 作品集創作者
                    return portfolio == null
                        ? (false, null, null)
                        : (true, portfolio.CreatorID, null);

                default:
                    return (false, null, null);
            }
        }
    }
}