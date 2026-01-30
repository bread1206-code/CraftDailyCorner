using Microsoft.EntityFrameworkCore;
using CraftDailyCorner.ViewModels;

namespace CraftDailyCorner.Models
{
    public class CraftDailyCornerContext : DbContext
    {
        public CraftDailyCornerContext(DbContextOptions<CraftDailyCornerContext> options) : base(options) { }

        #region DbSet

        public DbSet<Member> Members => Set<Member>();
        public DbSet<Privacy> Privacies => Set<Privacy>();
        public DbSet<Role> Roles => Set<Role>();
        public DbSet<MemberRole> MemberRoles => Set<MemberRole>();
        public DbSet<MemberRoleHistory> MemberRoleHistories => Set<MemberRoleHistory>();
        public DbSet<CreatorApplication> CreatorApplications => Set<CreatorApplication>();
        public DbSet<CreatorProfile> CreatorProfiles => Set<CreatorProfile>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<ProductImage> ProductImages => Set<ProductImage>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<ProductTag> ProductTags => Set<ProductTag>();
        public DbSet<Cart> Carts => Set<Cart>();
        public DbSet<CartItem> CartItems => Set<CartItem>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<OrderDetail> OrderDetails => Set<OrderDetail>();
        public DbSet<Payment> Payments => Set<Payment>();
        public DbSet<Shipment> Shipments => Set<Shipment>();
        public DbSet<FavoriteProduct> FavoriteProducts => Set<FavoriteProduct>();
        public DbSet<FollowCreator> FollowCreators => Set<FollowCreator>();
        public DbSet<ProductReview> ProductReviews => Set<ProductReview>();
        public DbSet<MessageThread> MessageThreads => Set<MessageThread>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<AutoReplyTemplate> AutoReplyTemplates => Set<AutoReplyTemplate>();
        public DbSet<CreatorPost> CreatorPosts => Set<CreatorPost>();
        public DbSet<PostComment> PostComments => Set<PostComment>();
        public DbSet<Inventory> Inventories => Set<Inventory>();
        public DbSet<InventoryAlert> InventoryAlerts => Set<InventoryAlert>();
        public DbSet<PlatformAnnouncement> PlatformAnnouncements => Set<PlatformAnnouncement>();
        public DbSet<HomepageBanner> HomepageBanners => Set<HomepageBanner>();
        public DbSet<PlatformSetting> PlatformSettings => Set<PlatformSetting>();
        public DbSet<Portfolio> Portfolios => Set<Portfolio>();
        public DbSet<PortfolioItem> PortfolioItems => Set<PortfolioItem>();
        public DbSet<NotificationPreference> NotificationPreferences => Set<NotificationPreference>();
        public DbSet<NotificationEvent> NotificationEvents => Set<NotificationEvent>();
        public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Primary Keys

            modelBuilder.Entity<Member>().HasKey(x => x.MemberID);
            //modelBuilder.Entity<Privacy>().HasKey(x => x.Email); // 假設 Email 為唯一鍵
            modelBuilder.Entity<Privacy>().HasKey(p => p.MemberID);
            modelBuilder.Entity<Privacy>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<Role>().HasKey(x => x.RoleID);
            modelBuilder.Entity<MemberRoleHistory>().HasKey(x => x.MemberRoleHistoryID);
            modelBuilder.Entity<CreatorApplication>().HasKey(x => x.ApplicationID);
            modelBuilder.Entity<CreatorProfile>().HasKey(x => x.CreatorID);
            modelBuilder.Entity<CreatorProfile>().HasIndex(c => c.MemberID).IsUnique();
            modelBuilder.Entity<Product>().HasKey(x => x.ProductID);
            modelBuilder.Entity<ProductImage>().HasKey(x => x.ImageID);
            modelBuilder.Entity<Category>().HasKey(x => x.CategoryID);
            modelBuilder.Entity<Tag>().HasKey(x => x.TagID);
            modelBuilder.Entity<Cart>().HasKey(x => x.CartID);
            modelBuilder.Entity<Order>().HasKey(x => x.OrderID);
            modelBuilder.Entity<Payment>().HasKey(x => x.PaymentID);
            modelBuilder.Entity<Shipment>().HasKey(x => x.ShipmentID);
            modelBuilder.Entity<ProductReview>().HasKey(x => x.ReviewID);
            modelBuilder.Entity<MessageThread>().HasKey(x => x.ThreadID);
            modelBuilder.Entity<Message>().HasKey(x => x.MessageID);
            modelBuilder.Entity<AutoReplyTemplate>().HasKey(x => x.TemplateID);
            modelBuilder.Entity<CreatorPost>().HasKey(x => x.PostID);
            modelBuilder.Entity<PostComment>().HasKey(x => x.CommentID);
            modelBuilder.Entity<Inventory>().HasKey(x => x.InventoryID);
            modelBuilder.Entity<InventoryAlert>().HasKey(x => x.AlertID);
            modelBuilder.Entity<PlatformAnnouncement>().HasKey(x => x.AnnouncementID);
            modelBuilder.Entity<HomepageBanner>().HasKey(x => x.BannerID);
            modelBuilder.Entity<PlatformSetting>().HasKey(x => x.SettingID);
            modelBuilder.Entity<Portfolio>().HasKey(x => x.PortfolioID);
            modelBuilder.Entity<PortfolioItem>().HasKey(x => x.ItemID);
            modelBuilder.Entity<NotificationPreference>().HasKey(x => x.PreferenceID);
            modelBuilder.Entity<NotificationEvent>().HasKey(x => x.EventID);

            #endregion

            #region Composite Keys

            modelBuilder.Entity<MemberRole>()
                .HasKey(x => new { x.MemberID, x.RoleID });

            modelBuilder.Entity<CartItem>()
                .HasKey(x => new { x.CartID, x.ProductID });

            modelBuilder.Entity<OrderDetail>()
                .HasKey(x => new { x.OrderID, x.ProductID });

            modelBuilder.Entity<FavoriteProduct>()
                .HasKey(x => new { x.MemberID, x.ProductID });

            modelBuilder.Entity<FollowCreator>()
                .HasKey(x => new { x.MemberID, x.CreatorID });

            modelBuilder.Entity<ProductCategory>()
                .HasKey(x => new { x.ProductID, x.CategoryID });

            modelBuilder.Entity<ProductTag>()
                .HasKey(x => new { x.ProductID, x.TagID });

            #endregion

            #region One-to-One

            modelBuilder.Entity<Privacy>()
                .HasOne(p => p.Member)
                .WithOne(m => m.Privacy)
                .HasForeignKey<Privacy>(p => p.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region One-to-Many

            modelBuilder.Entity<MemberRole>()
                .HasOne(mr => mr.Member)
                .WithMany(m => m.MemberRole)
                .HasForeignKey(mr => mr.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberRole>()
                .HasOne(mr => mr.Role)
                .WithMany(r => r.MemberRole)
                .HasForeignKey(mr => mr.RoleID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberRoleHistory>()
                .HasOne(h => h.Member)
                .WithMany(m => m.MemberRoleHistory)
                .HasForeignKey(h => h.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MemberRoleHistory>()
                .HasOne(h => h.Role)
                .WithMany(m => m.MemberRoleHistory)
                .HasForeignKey(h => h.RoleID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MemberRoleHistory>()
                .HasOne(h => h.Member)
                .WithMany(m => m.MemberRoleHistory)
                .HasForeignKey(h => h.OperatorMemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CreatorApplication>()
                .HasOne(ca => ca.Member)
                .WithMany(m => m.CreatorApplication)
                .HasForeignKey(ca => ca.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CreatorApplication>()
                .HasOne(ca => ca.Member)
                .WithMany(m => m.CreatorApplication)
                .HasForeignKey(ca => ca.ReviewedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CreatorProfile>()
                .HasOne(c => c.Member)
                .WithOne(m => m.CreatorProfile)
                .HasForeignKey<CreatorProfile>(c => c.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.CreatorProfile)
                .WithMany(c => c.Product)
                .HasForeignKey(p => p.CreatorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImage)
                .HasForeignKey(pi => pi.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategory)
                .HasForeignKey(pc => pc.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategory)
                .HasForeignKey(pc => pc.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.ProductTag)
                .HasForeignKey(pt => pt.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.ProductTag)
                .HasForeignKey(pt => pt.TagID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Member)
                .WithMany(m => m.Cart)
                .HasForeignKey(c => c.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItem)
                .HasForeignKey(ci => ci.CartID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItem)
                .HasForeignKey(ci => ci.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Member)
                .WithMany(m => m.Order)
                .HasForeignKey(o => o.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetail)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetail)
                .HasForeignKey(od => od.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payment)
                .HasForeignKey(p => p.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.Order)
                .WithOne(o => o.Shipment)
                .HasForeignKey<Shipment>(s => s.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteProduct>()
                .HasOne(fp => fp.Member)
                .WithMany(m => m.FavoriteProduct)
                .HasForeignKey(fp => fp.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteProduct>()
                .HasOne(fp => fp.Product)
                .WithMany(p => p.FavoriteProduct)
                .HasForeignKey(fp => fp.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FollowCreator>()
                .HasOne(fc => fc.Member)
                .WithMany(m => m.FollowCreator)
                .HasForeignKey(fc => fc.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FollowCreator>()
                .HasOne(fc => fc.CreatorProfile)
                .WithMany(c => c.FollowCreator)
                .HasForeignKey(fc => fc.CreatorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Member)
                .WithMany(m => m.ProductReview)
                .HasForeignKey(pr => pr.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.ProductReview)
                .HasForeignKey(pr => pr.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MessageThread>()
                .HasOne(mt => mt.Member)
                .WithMany(m => m.MessageThread)
                .HasForeignKey(mt => mt.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MessageThread>()
                .HasOne(mt => mt.Member)
                .WithMany(c => c.MessageThread)
                .HasForeignKey(mt => mt.CreatorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.MessageThread)
                .WithMany(mt => mt.Message)
                .HasForeignKey(m => m.ThreadID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AutoReplyTemplate>()
                .HasOne(a => a.CreatorProfile)
                .WithMany(c => c.AutoReplyTemplate)
                .HasForeignKey(a => a.CreatorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CreatorPost>()
                .HasOne(c => c.CreatorProfile)
                .WithMany(cp => cp.CreatorPost)
                .HasForeignKey(c => c.CreatorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostComment>()
                .HasOne(pc => pc.CreatorPost)
                .WithMany(c => c.PostComment)
                .HasForeignKey(pc => pc.PostID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostComment>()
                .HasOne(pc => pc.Member)
                .WithMany(m => m.PostComment)
                .HasForeignKey(pc => pc.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(i => i.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryAlert>()
                .HasOne(ia => ia.Inventory)
                .WithMany(i => i.InventoryAlert)
                .HasForeignKey(ia => ia.InventoryID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlatformAnnouncement>()
                .HasOne(pa => pa.Member)
                .WithMany(p => p.PlatformAnnouncement)
                .HasForeignKey(pa => pa.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HomepageBanner>()
                .HasOne(hb => hb.Member)
                .WithMany(h => h.HomepageBanner)
                .HasForeignKey(hb => hb.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlatformSetting>()
                .HasOne(ps => ps.Member)
                .WithMany(p => p.PlatformSetting)
                .HasForeignKey(ps => ps.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.CreatorProfile)
                .WithMany(c => c.Portfolio)
                .HasForeignKey(p => p.CreatorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PortfolioItem>()
                .HasOne(pi => pi.Portfolio)
                .WithMany(p => p.PortfolioItem)
                .HasForeignKey(pi => pi.PortfolioID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NotificationPreference>()
                .HasOne(np => np.Member)
                .WithMany(m => m.NotificationPreference)
                .HasForeignKey(np => np.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NotificationEvent>()
                .HasOne(ne => ne.Member)
                .WithMany(m => m.NotificationEvent)
                .HasForeignKey(ne => ne.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            #endregion

            #region Delete Behavior Control (避免 Multiple Cascade Path)

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Member)
                .WithMany(m => m.Message)
                .HasForeignKey(m => m.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Unique
            // Email 唯一
            modelBuilder.Entity<Privacy>()
                .HasIndex(p => p.Email)
                .IsUnique();

            // Phone 唯一（可選，允許為 null）
            modelBuilder.Entity<Privacy>()
                .HasIndex(p => p.Phone)
                .IsUnique()
                .HasFilter("[Phone] IS NOT NULL"); // 允許 Phone 空值

            #endregion
        }
        public DbSet<CraftDailyCorner.ViewModels.VMRegister> VMRegister { get; set; } = default!;
    }
}