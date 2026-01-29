using CraftDailyCorner.Models;

namespace CraftDailyCorner.Seed.Datas
{
    public class SeedCreatorPost
    {
        private readonly CraftDailyCornerContext _context;

        public SeedCreatorPost(CraftDailyCornerContext context)
        {
            _context = context;
        }
        public void Run(string[] imageGuids)
        {
            if (!_context.CreatorPosts.Any()) // 避免重複 Seed
            {
                var creatorPosts = new List<CreatorPost>
                {
                    new CreatorPost
                    {
                        PostID = imageGuids[0],
                        Title = "木作日常",
                        Content = "今天完成了一個新的榫接盒。",
                        ImageUrl = imageGuids[0] + ".png",
                        Visibility = 0,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new CreatorPost
                    {
                        PostID = imageGuids[1],
                        Title = "木作日常｜榫接練習",
                        Content = "今天嘗試了不同角度的榫接方式，手感比之前穩定許多。",
                        ImageUrl = imageGuids[1] + ".png",
                        Visibility = 0,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new CreatorPost
                    {
                        PostID = imageGuids[2],
                        Title = "筆墨之間",
                        Content = "今晨研墨鋪紙，以行書練習《靜心》二字。筆鋒轉折時，墨色在宣紙上自然暈開，彷彿呼吸也隨之放慢。" +
                        "書寫不只是技巧的堆疊，更是心境的映照。當雜念漸散，字形反而愈發穩定。願這份黑白之間的寧靜，也能在完成的作品中被感受到。",
                        ImageUrl = imageGuids[2] + ".png",
                        Visibility = 0,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00002"
                    },
                    new CreatorPost
                    {
                        PostID = imageGuids[3],
                        Title = "選材筆記",
                        Content = "最近偏好使用樟木，氣味溫潤，紋理也很適合做小型作品。",
                        ImageUrl = imageGuids[3] + ".png",
                        Visibility = (CreatorPostVisibility)1,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new CreatorPost
                    {
                        PostID = imageGuids[4],
                        Title = "工作室的一天",
                        Content = "整理了一整天的木料，雖然累，但看到整齊的材料牆很療癒。",
                        ImageUrl = imageGuids[4] + ".png",
                        Visibility = 0,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new CreatorPost
                    {
                        PostID = imageGuids[5],
                        Title = "新作品打樣中",
                        Content = "正在嘗試把榫接結構用在首飾盒上，希望能兼顧美觀與實用。",
                        ImageUrl = imageGuids[5] + ".png",
                        Visibility = 0,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new CreatorPost
                    {
                        PostID = imageGuids[6],
                        Title = "工具保養紀錄",
                        Content = "今天替工作室的老鑿刀、刨刀與鋸子進行完整清潔與保養。" +
                        "逐一除鏽、上油、磨刃，讓工具恢復原本應有的銳利與順手手感。" +
                        "每一把工具都有陪伴創作的痕跡與記憶，它們不只是工作器具，更像是長年並肩作戰的夥伴。" +
                        "也提醒自己，唯有用心對待工具，作品才能保有溫度與品質。",
                        ImageUrl = imageGuids[6] + ".png",
                        Visibility = 0,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    }
                };
                _context.CreatorPosts.AddRange(creatorPosts);
                _context.SaveChanges();
            }
        }
    }
}
