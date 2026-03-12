using CraftDailyCorner.Models.enums;

namespace CraftDailyCorner.Helpers
{
    //心情反應CSS設定
    public static class ReactionIconHelper
    {
        public static string GetIcon(ReactionType type)
        {
            return type switch
            {
                ReactionType.Like => "bi-hand-thumbs-up-fill text-primary",
                ReactionType.Love => "bi-heart-fill text-danger",
                ReactionType.Haha => "bi-emoji-laughing text-success",
                ReactionType.Wow => "bi-emoji-surprise text-warning",
                ReactionType.Sad => "bi-emoji-frown text-info",
                ReactionType.Angry => "bi-emoji-angry text-danger",
                _ => "bi-hand-thumbs-up-fill"
            };
        }
    }
}
