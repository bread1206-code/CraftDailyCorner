using CraftDailyCorner.ViewModels.Member;

namespace CraftDailyCorner.Services.Interface
{
    public interface IProductReviewService
    {
        (bool Success, string Message) UpsertReview(string memberId, VMProductReviewForm vm);
    }
}