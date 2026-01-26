using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Diagnostics.Metrics;
using System.Drawing;
using CraftDailyCorner.Models;
using static System.Net.Mime.MediaTypeNames;
namespace CraftDailyCorner.Seed
{
    public class SeedData
    {
        //1.3.3 撰寫SeedData類別的內容
        //      (1)撰寫靜態方法 Initialize(IServiceProvider serviceProvider)
        //      (2)撰寫Book及ReBook資料表內的初始資料程式
        //      (3)撰寫上傳圖片的程式
        //      (4)加上 using() 及 判斷資料庫是否有資料的程式

        //(1)撰寫靜態方法 Initialize(IServiceProvider serviceProvider)
        public static void Initialize(IServiceProvider serviceProvider)//靜態方法：不需要建構物件，可以直接使用此方法
        {
            //(4)加上 using()
            //(2)撰寫Book及ReBook資料表內的初始資料程式
            using (CraftDailyCornerContext context = new CraftDailyCornerContext(serviceProvider.GetRequiredService<DbContextOptions<CraftDailyCornerContext>>()))
            {
                //(4)判斷資料庫是否有資料
                //判斷Book資料表是否已有資料，若有資料則不執行後續新增初始資料的動作
                if (context.Member.Any())
                {
                    return; //資料庫已經有初始資料，結束
                }

                //使用陣列存放GUID
                //01Member
                int memberCount = 5;
                string[] mGuid = new string[memberCount];

                for (int i = 0; i < memberCount; i++)
                {
                    mGuid[i] = Guid.NewGuid().ToString();
                }
                //02CreatorApp
                int caCount = 3;
                string[] caGuid = new string[caCount];

                for (int i = 0; i < caCount; i++)
                {
                    caGuid[i] = Guid.NewGuid().ToString();
                }
                //03Creator
                int cCount = 1;
                string[] cGuid = new string[cCount];

                for (int i = 0; i < cCount; i++)
                {
                    cGuid[i] = Guid.NewGuid().ToString();
                }
                //04Products
                int pCount = 6;
                string[] pGuid = new string[pCount];

                for (int i = 0; i < pCount; i++)
                {
                    pGuid[i] = Guid.NewGuid().ToString();
                }
                //05Post
                int postCount = 7;
                string[] postGuid = new string[postCount];
                for (int i = 0; i < postCount; i++)
                {
                    postGuid[i] = Guid.NewGuid().ToString();
                }
                //06Portfolio
                int pfCount = 2;
                string[] pfGuid = new string[pfCount];
                for (int i = 0; i < pfCount; i++)
                {
                    pfGuid[i] = Guid.NewGuid().ToString();
                }
                //07HomeBanner
                int hbCount = 1;
                string[] hbGuid = new string[hbCount];
                for (int i = 0; i < hbCount; i++)
                {
                    hbGuid[i] = Guid.NewGuid().ToString();
                }


                //新增資料表的初始資料
                context.Member.AddRange(
                    new Member
                    {
                        MemberID = "M0000001",
                        ImageUrl = mGuid[0] + ".png",
                        DisplayName = "一號會員",
                        Status = 0,
                        CreatedAt = DateTime.Now
                    },
                    new Member
                    {
                        MemberID = "M0000002",
                        ImageUrl = mGuid[1] + ".png",
                        DisplayName = "二號會員",
                        Status = 0,
                        CreatedAt = DateTime.Now
                    },
                    new Member
                    {
                        MemberID = "M0000003",
                        ImageUrl = mGuid[2] + ".png",
                        DisplayName = "三號會員",
                        Status = 0,
                        CreatedAt = DateTime.Now
                    },
                    new Member
                    {
                        MemberID = "M0000004",
                        ImageUrl = mGuid[3] + ".png",
                        DisplayName = "四號會員",
                        Status = 0,
                        CreatedAt = DateTime.Now
                    },
                    new Member
                    {
                        MemberID = "M0000005",
                        ImageUrl = mGuid[4] + ".png",
                        DisplayName = "五號會員",
                        Status = 0,
                        CreatedAt = DateTime.Now
                    }

                );

                context.Privacy.AddRange(
                    new Privacy
                    {
                        Email = "member01@member.com",
                        PasswordHash = "123",
                        Phone = "0912345678",
                        Birthday = new DateTime(2025, 12, 20),
                        Gender = 0,
                        MemberID = "M0000001"
                    }, new Privacy
                    {
                        Email = "member02@member.com",
                        PasswordHash = "123",
                        Phone = "0912345678",
                        Birthday = new DateTime(2025, 12, 21),
                        Gender = 0,
                        MemberID = "M0000002"
                    }, new Privacy
                    {
                        Email = "member03@member.com",
                        PasswordHash = "123",
                        Phone = "0912345678",
                        Birthday = new DateTime(2025, 12, 22),
                        Gender = 0,
                        MemberID = "M0000003"
                    }, new Privacy
                    {
                        Email = "member04@member.com",
                        PasswordHash = "123",
                        Phone = "0912345678",
                        Birthday = new DateTime(2025, 12, 23),
                        Gender = 0,
                        MemberID = "M0000004"
                    }, new Privacy
                    {
                        Email = "member05@member.com",
                        PasswordHash = "123",
                        Phone = "0912345678",
                        Birthday = new DateTime(2025, 12, 24),
                        Gender = 0,
                        MemberID = "M0000005"
                    }
                );

                context.Role.AddRange(
                    new Role
                    {
                        RoleID = "01",
                        RoleName = "一般會員",
                        Description = "可以使用大部分功能。"
                    }, new Role
                    {
                        RoleID = "02",
                        RoleName = "創作者",
                        Description = "可以使用販賣功能"
                    }, new Role
                    {
                        RoleID = "03",
                        RoleName = "管理者",
                        Description = "可以管理平台資料。"
                    }
                );

                context.MemberRole.AddRange(
                    new MemberRole
                    {
                        MemberID = "M0000001",
                        RoleID = "01",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000001",
                        RoleID = "03",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000002",
                        RoleID = "01",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000003",
                        RoleID = "01",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000004",
                        RoleID = "01",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000004",
                        RoleID = "02",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000005",
                        RoleID = "01",
                        AssignedAt = DateTime.Now
                    }, new MemberRole
                    {
                        MemberID = "M0000005",
                        RoleID = "02",
                        AssignedAt = DateTime.Now
                    }
                );

                context.MemberRoleHistory.AddRange(
                    new MemberRoleHistory
                    {
                        Action = 0,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000001",
                        RoleID = "01",
                        OperatedBy = 0,
                        OperatorMemberID = null
                    }, new MemberRoleHistory
                    {
                        Action = 0,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000002",
                        RoleID = "01",
                        OperatedBy = 0,
                        OperatorMemberID = null
                    }, new MemberRoleHistory
                    {
                        Action = 0,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000003",
                        RoleID = "01",
                        OperatedBy = 0,
                        OperatorMemberID = null
                    }, new MemberRoleHistory
                    {
                        Action = 0,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000004",
                        RoleID = "01",
                        OperatedBy = 0,
                        OperatorMemberID = null
                    }, new MemberRoleHistory
                    {
                        Action = 0,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000005",
                        RoleID = "01",
                        OperatedBy = 0,
                        OperatorMemberID = null
                    }, new MemberRoleHistory
                    {
                        Action = (MemberRoleHistoryAction)1,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000004",
                        RoleID = "02",
                        OperatedBy = (MemberRoleHistoryOperated)1,
                        OperatorMemberID = null
                    }, new MemberRoleHistory
                    {
                        Action = (MemberRoleHistoryAction)1,
                        OperatedAt = DateTime.Now,
                        MemberID = "M0000005",
                        RoleID = "02",
                        OperatedBy = (MemberRoleHistoryOperated)1,
                        OperatorMemberID = null
                    }
                );

                context.CreatorApplication.AddRange(
                    new CreatorApplication
                    {
                        DisplayName = "木匠大師",
                        Intro = "我是阿拓。",
                        PortfolioSampleUrl = caGuid[0] + ".png",
                        StartDate = new DateTime(2020, 01, 01),
                        Status = (CreatorApplicationStatus)2,
                        AppliedAt = new DateTime(2025, 12, 01),
                        ReviewedAt = new DateTime(2025, 12, 03),
                        ReviewNote = "簡介太少",
                        MemberID = "M0000004",
                        ReviewedBy = "M0000001"
                    }, new CreatorApplication
                    {
                        DisplayName = "木匠大師",
                        Intro = "我是阿拓。這雙手除了與木材對話，別無長處。" +
                        "我專注於打磨帶有溫潤手感的木牌項鍊，也雕琢能盛裝回憶的榫接置物盒。" +
                        "每一道木紋都是時間的贈禮，我用鑿刀留住森林的氣息，" +
                        "只為將這份靜謐的陪伴，送到你的掌心。",
                        PortfolioSampleUrl = caGuid[1] + ".png",
                        StartDate = new DateTime(2020, 01, 01),
                        Status = (CreatorApplicationStatus)1,
                        AppliedAt = new DateTime(2025, 12, 03),
                        ReviewedAt = new DateTime(2025, 12, 04),
                        ReviewNote = null,
                        MemberID = "M0000004",
                        ReviewedBy = "M0000001"
                    }, new CreatorApplication
                    {
                        DisplayName = "墨尋",
                        Intro = "我是墨尋，一生只在黑白之間修行。" +
                        "除了書寫紅紙黑字的春聯與氣勢磅礴的詩詞掛軸，我也將筆墨染上手工摺扇，捕捉流動的清風。" +
                        "我筆下的每一點一畫，不求驚世駭俗，只願在墨香散去前，為你這浮躁的世間留下一抹安定的神韻。",
                        PortfolioSampleUrl = caGuid[2] + ".png",
                        StartDate = new DateTime(2020, 08, 01),
                        Status = (CreatorApplicationStatus)1,
                        AppliedAt = new DateTime(2025, 12, 01),
                        ReviewedAt = new DateTime(2025, 12, 03),
                        ReviewNote = null,
                        MemberID = "M0000004",
                        ReviewedBy = "M0000001"
                    }
                );

                context.CreatorProfile.AddRange(
                    new CreatorProfile
                    {
                        CreatorID = "C00001",
                        ImageUrl = cGuid[0] + ".png",
                        DisplayName = "木匠大師",
                        Intro = "我是阿拓。這雙手除了與木材對話，別無長處。" +
                        "我專注於打磨帶有溫潤手感的木牌項鍊，也雕琢能盛裝回憶的榫接置物盒。" +
                        "每一道木紋都是時間的贈禮，我用鑿刀留住森林的氣息，" +
                        "只為將這份靜謐的陪伴，送到你的掌心。",
                        StartDate = new DateTime(2020, 01, 01),
                        BankCode = " ",
                        BankAccount = " ",
                        Status = (CreatorProfileStatus)1,
                        CreatedAt = DateTime.Now,
                        MemberID = "M0000004"
                    }, new CreatorProfile
                    {
                        CreatorID = "C00002",
                        ImageUrl = cGuid[0] + ".png",
                        DisplayName = "墨尋",
                        Intro = "我是墨尋，一生只在黑白之間修行。" +
                        "除了書寫紅紙黑字的春聯與氣勢磅礴的詩詞掛軸，我也將筆墨染上手工摺扇，捕捉流動的清風。" +
                        "我筆下的每一點一畫，不求驚世駭俗，只願在墨香散去前，為你這浮躁的世間留下一抹安定的神韻。",
                        StartDate = new DateTime(2020, 08, 01),
                        BankCode = " ",
                        BankAccount = " ",
                        Status = (CreatorProfileStatus)1,
                        CreatedAt = DateTime.Now,
                        MemberID = "M0000005"
                    }
                );

                context.Product.AddRange(
                        new Product
                        {
                            ProductID = "P000000001",
                            ProductName = "木牌項鍊",
                            Description = "手工雕刻的溫潤木牌項鍊",
                            Price = 1200,
                            Status = (ProductStatus)1,
                            CreatedAt = DateTime.Now,
                            CreatorID = "C00001"
                        },
                        new Product
                        {
                            ProductID = "P000000002",
                            ProductName = "書法摺扇",
                            Description = "以行書書寫的手工摺扇",
                            Price = 1800,
                            Status = (ProductStatus)1,
                            CreatedAt = DateTime.Now,
                            CreatorID = "C00002"
                        }, new Product
                        {
                            ProductID = "P000000003",
                            ProductName = "原木書架",
                            Description = "手工打造的原木書架，保留自然木紋紋理，兼具美觀與實用。",
                            Price = 3500,
                            Status = (ProductStatus)1,
                            CreatedAt = DateTime.Now,
                            CreatorID = "C00001"
                        },
                        new Product
                        {
                            ProductID = "P000000004",
                            ProductName = "榫接茶几",
                            Description = "採用傳統榫接工法製作的手工茶几，穩固耐用，木紋自然清晰。",
                            Price = 4800,
                            Status = (ProductStatus)1,
                            CreatedAt = DateTime.Now,
                            CreatorID = "C00001"
                        },
                        new Product
                        {
                            ProductID = "P000000005",
                            ProductName = "行書字帖",
                            Description = "由書法家手寫的行書練習字帖，包含經典篇章與練習指南。",
                            Price = 8000,
                            Status = (ProductStatus)1,
                            CreatedAt = DateTime.Now,
                            CreatorID = "C00002"
                        }
                );

                context.ProductImage.AddRange(
                        new ProductImage
                        {
                            ImageUrl = pGuid[0] + ".png",
                            SortOrder = 0,
                            Status = (ProductImageStatus)1,
                            ProductID = "P000000001"
                        },
                        new ProductImage
                        {
                            ImageUrl = pGuid[1] + ".png",
                            SortOrder = 1,
                            Status = (ProductImageStatus)1,
                            ProductID = "P000000001"
                        },
                        new ProductImage
                        {
                            ImageUrl = pGuid[2] + ".png",
                            SortOrder = 0,
                            Status = (ProductImageStatus)1,
                            ProductID = "P000000002"
                        },
                        new ProductImage
                        {
                            ImageUrl = pGuid[3] + ".png",
                            SortOrder = 0,
                            Status = (ProductImageStatus)1,
                            ProductID = "P000000003"
                        },
                        new ProductImage
                        {
                            ImageUrl = pGuid[4] + ".png",
                            SortOrder = 0,
                            Status = (ProductImageStatus)1,
                            ProductID = "P000000004"
                        },
                        new ProductImage
                        {
                            ImageUrl = pGuid[5] + ".png",
                            SortOrder = 0,
                            Status = (ProductImageStatus)1,
                            ProductID = "P000000005"
                        }
                );
                context.SaveChanges();
                context.Category.AddRange(
                        new Category { CategoryName = "木作" },
                        new Category { CategoryName = "書法" },
                        new Category { CategoryName = "生活擺飾" }
                );
                context.SaveChanges();
                context.Tag.AddRange(
                        new Tag { TagName = "手工" },
                        new Tag { TagName = "限量" },
                        new Tag { TagName = "原創" }
                );
                context.SaveChanges();
                context.ProductCategory.AddRange(
                        new ProductCategory { ProductID = "P000000001", CategoryID = 1 },
                        new ProductCategory { ProductID = "P000000002", CategoryID = 2 }
                );
                context.SaveChanges();
                context.ProductTag.AddRange(
                        new ProductTag { ProductID = "P000000001", TagID = 1 },
                        new ProductTag { ProductID = "P000000001", TagID = 3 },
                        new ProductTag { ProductID = "P000000002", TagID = 1 }
                );
                context.SaveChanges();
                context.Cart.Add(
                    new Cart
                    {
                        MemberID = "M0000002",
                        UpdatedAt = DateTime.Now
                    }
                );
                context.SaveChanges();

                context.CartItem.Add(
                    new CartItem
                    {
                        CartID = 1,
                        ProductID = "P000000001",
                        Quantity = 1
                    }
                );
                context.SaveChanges();
                context.Order.Add(
                    new Order
                    {
                        OrderID = "202601010001",
                        ReceiverName = "王小明",
                        ReceiverPhone = "0912345678",
                        ShippingAddress = "台北市中正區",
                        CreatedAt = DateTime.Now,
                        Status = 1,
                        TotalAmount = 1200,
                        MemberID = "M0000002"
                    }
                );
                context.SaveChanges();
                context.OrderDetail.Add(
                    new OrderDetail
                    {
                        OrderID = "202601010001",
                        ProductID = "P000000001",
                        ProductNameSnapshot = "木牌項鍊",
                        PriceSnapshot = 1200,
                        Quantity = 1
                    }
                );
                context.Payment.Add(
                    new Payment
                    {
                        PaymentMethod = (PaymentPaymentMethod)1,
                        Amount = 1200,
                        Status = (PaymentStatus)1,
                        GatewayTradeNo = "TEST123456",
                        AttemptNo = 1,
                        PaidAt = DateTime.Now,
                        CreatedAt = DateTime.Now,
                        OrderID = "202601010001"
                    }
                );
                context.SaveChanges();
                context.Shipment.Add(
                    new Shipment
                    {
                        TrackingNo = "EC123456789TW",
                        Status = (ShipmentStatus)1,
                        OrderID = "202601010001"
                    }
                );
                context.SaveChanges();
                context.FavoriteProduct.Add(
                    new FavoriteProduct
                    {
                        MemberID = "M0000002",
                        ProductID = "P000000001",
                        CreatedAt = DateTime.Now
                    }
                );
                context.SaveChanges();
                context.FollowCreator.Add(
                    new FollowCreator
                    {
                        MemberID = "M0000002",
                        CreatorID = "C00001",
                        CreatedAt = DateTime.Now
                    }
                );
                context.SaveChanges();
                context.ProductReview.Add(
                    new ProductReview
                    {
                        Rating = 5,
                        Comment = "質感非常好，會再回購",
                        CreatedAt = DateTime.Now,
                        MemberID = "M0000002",
                        ProductID = "P000000001"
                    }
                );
                context.SaveChanges();
                context.PlatformAnnouncement.AddRange(
                    new PlatformAnnouncement
                    {
                        Title = "平台正式上線",
                        Content = "歡迎加入手作市集平台！",
                        Status = (PlatformAnnouncementStatus)1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    },
                    new PlatformAnnouncement
                    {
                        Title = "春節出貨公告",
                        Content = "春節期間出貨將延後 3–5 日。",
                        Status = (PlatformAnnouncementStatus)1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    }
                );
                context.SaveChanges();
                context.MessageThread.Add(
                    new MessageThread
                    {
                        CreatedAt = DateTime.Now,
                        MemberID = "M0000002",
                        CreatorID = "M0000004"
                    }
                );
                context.SaveChanges();
                context.Message.AddRange(
                    new Message
                    {
                        Content = "請問這個商品可以客製刻字嗎？",
                        CreatedAt = DateTime.Now,
                        ThreadID = 1,
                        SenderID = "M0000002"
                    },
                    new Message
                    {
                        Content = "可以的，請在備註說明想刻的內容。",
                        CreatedAt = DateTime.Now,
                        ThreadID = 1,
                        SenderID = "M0000004"
                    }
                );
                context.SaveChanges();
                context.AutoReplyTemplate.Add(
                    new AutoReplyTemplate
                    {
                        Title = "客製詢問回覆",
                        Content = "您好，客製需求請提供詳細說明，謝謝。",
                        IsActive = true,
                        TriggerType = (AutoReplyTemplateTriggerType)1,
                        CreatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    }
                );
                context.SaveChanges();
                context.CreatorPost.AddRange(
                    new CreatorPost
                    {
                        PostID = postGuid[0],
                        Title = "木作日常",
                        Content = "今天完成了一個新的榫接盒。",
                        ImageUrl = postGuid[0] + ".png",
                        Visibility = 0,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new CreatorPost
                    {
                        PostID = postGuid[1],
                        Title = "木作日常｜榫接練習",
                        Content = "今天嘗試了不同角度的榫接方式，手感比之前穩定許多。",
                        ImageUrl = postGuid[1] + ".png",
                        Visibility = 0,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new CreatorPost
                    {
                        PostID = postGuid[2],
                        Title = "筆墨之間",
                        Content = "今晨研墨鋪紙，以行書練習《靜心》二字。筆鋒轉折時，墨色在宣紙上自然暈開，彷彿呼吸也隨之放慢。" +
                        "書寫不只是技巧的堆疊，更是心境的映照。當雜念漸散，字形反而愈發穩定。願這份黑白之間的寧靜，也能在完成的作品中被感受到。",
                        ImageUrl = postGuid[2] + ".png",
                        Visibility = 0,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00002"
                    },
                    new CreatorPost
                    {
                        PostID = postGuid[3],
                        Title = "選材筆記",
                        Content = "最近偏好使用樟木，氣味溫潤，紋理也很適合做小型作品。",
                        ImageUrl = postGuid[3] + ".png",
                        Visibility = (CreatorPostVisibility)1,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new CreatorPost
                    {
                        PostID = postGuid[4],
                        Title = "工作室的一天",
                        Content = "整理了一整天的木料，雖然累，但看到整齊的材料牆很療癒。",
                        ImageUrl = postGuid[4] + ".png",
                        Visibility = 0,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new CreatorPost
                    {
                        PostID = postGuid[5],
                        Title = "新作品打樣中",
                        Content = "正在嘗試把榫接結構用在首飾盒上，希望能兼顧美觀與實用。",
                        ImageUrl = postGuid[5] + ".png",
                        Visibility = 0,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    },
                    new CreatorPost
                    {
                        PostID = postGuid[6],
                        Title = "工具保養紀錄",
                        Content = "今天替工作室的老鑿刀、刨刀與鋸子進行完整清潔與保養。" +
                        "逐一除鏽、上油、磨刃，讓工具恢復原本應有的銳利與順手手感。" +
                        "每一把工具都有陪伴創作的痕跡與記憶，它們不只是工作器具，更像是長年並肩作戰的夥伴。" +
                        "也提醒自己，唯有用心對待工具，作品才能保有溫度與品質。",
                        ImageUrl = postGuid[6] + ".png",
                        Visibility = 0,
                        Status = (CreatorPostStatus)1,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    }
                );
                context.SaveChanges();
                context.PostComment.Add(
                    new PostComment
                    {
                        CommentID = Guid.NewGuid().ToString(),
                        Content = "真的很漂亮！",
                        Status = (PostCommentStatus)1,
                        CreatedAt = DateTime.Now,
                        PostID = postGuid[0],
                        MemberID = "M0000002"
                    }
                );
                context.SaveChanges();
                context.Inventory.Add(
                    new Inventory
                    {
                        StockQty = 10,
                        AlertQty = 3,
                        ProductID = "P000000001"
                    }
                );
                context.SaveChanges();
                context.InventoryAlert.Add(
                    new InventoryAlert
                    {
                        TriggeredAt = DateTime.Now,
                        Status = (InventoryAlertStatus)1,
                        InventoryID = context.Inventory.First().InventoryID
                    }
                );
                context.SaveChanges();
                context.HomepageBanner.Add(
                    new HomepageBanner
                    {
                        ImageUrl = hbGuid[0] + ".png",
                        Title = "手作職人市集",
                        Subtitle = "慢活 × 原創 × 溫度",
                        Status = (HomepageBannerStatus)1,
                        CreatedAt = DateTime.Now,
                        CreatedBy = "M0000001"
                    }
                );
                context.SaveChanges();
                context.PlatformSetting.Add(
                    new PlatformSetting
                    {
                        SettingKey = "OrderAutoCancelDays",
                        SettingValue = "7",
                        DataType = "int",
                        Category = (PlatformSettingCategory)1,
                        Description = "未付款訂單自動取消天數",
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = "M0000001"
                    }
                );
                context.SaveChanges();
                context.Portfolio.Add(
                    new Portfolio
                    {
                        PortfolioID = pfGuid[0],
                        Title = "木作精選",
                        Description = "近年代表作品",
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        CreatorID = "C00001"
                    }
                );
                context.SaveChanges();
                context.PortfolioItem.Add(
                    new PortfolioItem
                    {
                        ItemID = Guid.NewGuid().ToString(),
                        ImageUrl = Guid.NewGuid().ToString() + ".png",
                        Title = "榫接木盒",
                        Description = "全手工榫接製作",
                        SortOrder = 0,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        PortfolioID = pfGuid[0]
                    }
                );
                context.SaveChanges();
                context.NotificationPreference.Add(
                    new NotificationPreference
                    {
                        NotificationType = (NotificationType)1,
                        IsActive = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                        MemberID = "M0000002"
                    }
                );
                context.SaveChanges();
                context.NotificationEvent.Add(
                    new NotificationEvent
                    {
                        NotificationType = (NotificationType)1,
                        Content = "您的訂單已成立",
                        CreatedAt = DateTime.Now,
                        MemberID = "M0000002"
                    }
                );
                context.SaveChanges();
                //01MemberImage
                string SeedPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "Seed", "SeedPhotos", "01Member");
                string BasePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", "01Member");
                // 設定不同尺寸資料夾
                var sizeFolders = new Dictionary<string, int>
                {
                    { "Thumbnail", 100 },
                    { "Medium", 300 },
                    { "Large", 800 }
                };
                // 建立資料夾
                foreach (var folder in sizeFolders.Keys)
                {
                    string path = Path.Combine(BasePhotoPath, folder);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                // 取得
                string[] files = Directory.GetFiles(SeedPhotoPath);

                for (int i = 0; i < memberCount; i++)
                {
                    string fileGuid = mGuid[i];

                    using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(files[i]))
                    {
                        foreach (var size in sizeFolders)
                        {
                            var options = new ResizeOptions
                            {
                                Size = new SixLabors.ImageSharp.Size(size.Value, size.Value),
                                Mode = ResizeMode.Crop,
                                Position = AnchorPositionMode.Center
                            };

                            using (var cloned = image.Clone(ctx => ctx.Resize(options)))
                            {
                                string destFile = Path.Combine(BasePhotoPath, size.Key, $"{fileGuid}.png");
                                cloned.Save(destFile, new PngEncoder());
                            }
                        }
                    }
                }
                //02CreatorAppImage
                SeedPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "Seed", "SeedPhotos", "02CreatorApp");
                BasePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", "02CreatorApp");
                // 設定不同尺寸資料夾
                sizeFolders = new Dictionary<string, int>
                {
                    { "Thumbnail", 100 },
                    { "Medium", 300 },
                    { "Large", 800 }
                };
                // 建立資料夾
                foreach (var folder in sizeFolders.Keys)
                {
                    string path = Path.Combine(BasePhotoPath, folder);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                // 取得
                files = Directory.GetFiles(SeedPhotoPath);

                for (int i = 0; i < caCount; i++)
                {
                    string fileGuid = caGuid[i];

                    using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(files[i]))
                    {
                        foreach (var size in sizeFolders)
                        {
                            var options = new ResizeOptions
                            {
                                Size = new SixLabors.ImageSharp.Size(size.Value, size.Value),
                                Mode = ResizeMode.Crop,
                                Position = AnchorPositionMode.Center
                            };

                            using (var cloned = image.Clone(ctx => ctx.Resize(options)))
                            {
                                string destFile = Path.Combine(BasePhotoPath, size.Key, $"{fileGuid}.png");
                                cloned.Save(destFile, new PngEncoder());
                            }
                        }
                    }
                }
                //03CreatorImage
                SeedPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "Seed", "SeedPhotos", "03Creator");
                BasePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", "03Creator");
                // 設定不同尺寸資料夾
                sizeFolders = new Dictionary<string, int>
                {
                    { "Thumbnail", 100 },
                    { "Medium", 300 },
                    { "Large", 800 }
                };
                // 建立資料夾
                foreach (var folder in sizeFolders.Keys)
                {
                    string path = Path.Combine(BasePhotoPath, folder);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                // 取得
                files = Directory.GetFiles(SeedPhotoPath);

                for (int i = 0; i < cCount; i++)
                {
                    string fileGuid = cGuid[i];

                    using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(files[i]))
                    {
                        foreach (var size in sizeFolders)
                        {
                            var options = new ResizeOptions
                            {
                                Size = new SixLabors.ImageSharp.Size(size.Value, size.Value),
                                Mode = ResizeMode.Crop,
                                Position = AnchorPositionMode.Center
                            };

                            using (var cloned = image.Clone(ctx => ctx.Resize(options)))
                            {
                                string destFile = Path.Combine(BasePhotoPath, size.Key, $"{fileGuid}.png");
                                cloned.Save(destFile, new PngEncoder());
                            }
                        }
                    }
                }
                //04ProductsImage
                SeedPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "Seed", "SeedPhotos", "04Products");
                BasePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", "04Products");
                // 設定不同尺寸資料夾
                sizeFolders = new Dictionary<string, int>
                {
                    { "Thumbnail", 100 },
                    { "Medium", 300 },
                    { "Large", 800 }
                };
                // 建立資料夾
                foreach (var folder in sizeFolders.Keys)
                {
                    string path = Path.Combine(BasePhotoPath, folder);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                // 取得
                files = Directory.GetFiles(SeedPhotoPath);

                for (int i = 0; i < pCount; i++)
                {
                    string fileGuid = pGuid[i];

                    using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(files[i]))
                    {
                        foreach (var size in sizeFolders)
                        {
                            var options = new ResizeOptions
                            {
                                Size = new SixLabors.ImageSharp.Size(size.Value, size.Value),
                                Mode = ResizeMode.Crop,
                                Position = AnchorPositionMode.Center
                            };

                            using (var cloned = image.Clone(ctx => ctx.Resize(options)))
                            {
                                string destFile = Path.Combine(BasePhotoPath, size.Key, $"{fileGuid}.png");
                                cloned.Save(destFile, new PngEncoder());
                            }
                        }
                    }
                }
                //05PostImage
                SeedPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "Seed", "SeedPhotos", "05Post");
                BasePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", "05Post");
                // 設定不同尺寸資料夾
                sizeFolders = new Dictionary<string, int>
                {
                    { "Thumbnail", 100 },
                    { "Medium", 300 },
                    { "Large", 800 }
                };
                // 建立資料夾
                foreach (var folder in sizeFolders.Keys)
                {
                    string path = Path.Combine(BasePhotoPath, folder);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                // 取得
                files = Directory.GetFiles(SeedPhotoPath);

                for (int i = 0; i < postCount; i++)
                {
                    string fileGuid = postGuid[i];

                    using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(files[i]))
                    {
                        foreach (var size in sizeFolders)
                        {
                            var options = new ResizeOptions
                            {
                                Size = new SixLabors.ImageSharp.Size(size.Value, size.Value),
                                Mode = ResizeMode.Crop,
                                Position = AnchorPositionMode.Center
                            };

                            using (var cloned = image.Clone(ctx => ctx.Resize(options)))
                            {
                                string destFile = Path.Combine(BasePhotoPath, size.Key, $"{fileGuid}.png");
                                cloned.Save(destFile, new PngEncoder());
                            }
                        }
                    }
                }
                //06PortfolioImage
                SeedPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "Seed", "SeedPhotos", "06Portfolio");
                BasePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", "06Portfolio");
                // 設定不同尺寸資料夾
                sizeFolders = new Dictionary<string, int>
                {
                    { "Thumbnail", 100 },
                    { "Medium", 300 },
                    { "Large", 800 }
                };
                // 建立資料夾
                foreach (var folder in sizeFolders.Keys)
                {
                    string path = Path.Combine(BasePhotoPath, folder);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                // 取得
                files = Directory.GetFiles(SeedPhotoPath);

                for (int i = 0; i < pfCount; i++)
                {
                    string fileGuid = pfGuid[i];

                    using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(files[i]))
                    {
                        foreach (var size in sizeFolders)
                        {
                            var options = new ResizeOptions
                            {
                                Size = new SixLabors.ImageSharp.Size(size.Value, size.Value),
                                Mode = ResizeMode.Crop,
                                Position = AnchorPositionMode.Center
                            };

                            using (var cloned = image.Clone(ctx => ctx.Resize(options)))
                            {
                                string destFile = Path.Combine(BasePhotoPath, size.Key, $"{fileGuid}.png");
                                cloned.Save(destFile, new PngEncoder());
                            }
                        }
                    }
                }
                //07HomeBannerImage
                SeedPhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "Seed", "SeedPhotos", "07HomeBanner");
                BasePhotoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Photos", "07HomeBanner");
                // 設定不同尺寸資料夾
                sizeFolders = new Dictionary<string, int>
                {
                    { "Thumbnail", 100 },
                    { "Medium", 300 },
                    { "Large", 800 }
                };
                // 建立資料夾
                foreach (var folder in sizeFolders.Keys)
                {
                    string path = Path.Combine(BasePhotoPath, folder);
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);
                }
                // 取得
                files = Directory.GetFiles(SeedPhotoPath);

                for (int i = 0; i < hbCount; i++)
                {
                    string fileGuid = hbGuid[i];

                    using (SixLabors.ImageSharp.Image image = SixLabors.ImageSharp.Image.Load(files[i]))
                    {
                        foreach (var size in sizeFolders)
                        {
                            var options = new ResizeOptions
                            {
                                Size = new SixLabors.ImageSharp.Size(size.Value, size.Value),
                                Mode = ResizeMode.Crop,
                                Position = AnchorPositionMode.Center
                            };

                            using (var cloned = image.Clone(ctx => ctx.Resize(options)))
                            {
                                string destFile = Path.Combine(BasePhotoPath, size.Key, $"{fileGuid}.png");
                                cloned.Save(destFile, new PngEncoder());
                            }
                        }
                    }
                }
            }//using結束
        }

    }
}