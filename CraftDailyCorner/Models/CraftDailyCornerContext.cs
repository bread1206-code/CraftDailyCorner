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
        public DbSet<PortfolioStatus> PortfolioStatuses => Set<PortfolioStatus>();
        public DbSet<NotificationPreference> NotificationPreferences => Set<NotificationPreference>();
        public DbSet<NotificationEvent> NotificationEvents => Set<NotificationEvent>();
        public DbSet<PasswordResetToken> PasswordResetTokens => Set<PasswordResetToken>();
        public DbSet<MemberStatus> MemberStatuses => Set<MemberStatus>();
        public DbSet<CreatorApplicationStatus> CreatorApplicationStatuses => Set<CreatorApplicationStatus>();
        public DbSet<CreatorProfileStatus> CreatorProfileStatuses => Set<CreatorProfileStatus>();
        public DbSet<ProductStatus> ProductStatuses => Set<ProductStatus>();
        public DbSet<ProductImageStatus> ProductImageStatuses => Set<ProductImageStatus>();
        public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
        public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
        public DbSet<PaymentStatus> PaymentStatuses => Set<PaymentStatus>();
        public DbSet<ShipmentStatus> ShipmentStatuses => Set<ShipmentStatus>();
        public DbSet<CreatorPostStatus> CreatorPostStatuses => Set<CreatorPostStatus>();
        public DbSet<PlatformAnnouncementStatus> PlatformAnnouncementStatuses => Set<PlatformAnnouncementStatus>();
        public DbSet<HomepageBannerStatus> HomepageBannerStatuses => Set<HomepageBannerStatus>();
        public DbSet<PlatformSettingCategory> PlatformSettingCategories => Set<PlatformSettingCategory>();
        public DbSet<Report> Reports => Set<Report>();
        public DbSet<ReportStatus> ReportStatuses => Set<ReportStatus>();

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            #region Primary Keys

            modelBuilder.Entity<Member>().HasKey(x => x.MemberID);
            modelBuilder.Entity<Privacy>().HasKey(p => p.MemberID);
            modelBuilder.Entity<Role>().HasKey(x => x.RoleID);
            modelBuilder.Entity<MemberRoleHistory>().HasKey(x => x.MemberRoleHistoryID);
            modelBuilder.Entity<CreatorApplication>().HasKey(x => x.ApplicationID);
            modelBuilder.Entity<CreatorProfile>().HasKey(x => x.CreatorID);
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
            modelBuilder.Entity<PortfolioStatus>().HasKey(x => x.StatusID);
            modelBuilder.Entity<NotificationPreference>().HasKey(x => x.PreferenceID);
            modelBuilder.Entity<NotificationEvent>().HasKey(x => x.EventID);
            modelBuilder.Entity<PasswordResetToken>().HasKey(x => x.PasswordResetId);
            modelBuilder.Entity<MemberStatus>().HasKey(x => x.StatusID);
            modelBuilder.Entity<CreatorApplicationStatus>().HasKey(x => x.StatusID);
            modelBuilder.Entity<CreatorProfileStatus>().HasKey(x => x.StatusID);
            modelBuilder.Entity<ProductStatus>().HasKey(x => x.StatusID);
            modelBuilder.Entity<ProductImageStatus>().HasKey(x => x.StatusID);
            modelBuilder.Entity<OrderStatus>().HasKey(x => x.StatusID);
            modelBuilder.Entity<PaymentMethod>().HasKey(x => x.MethodID);
            modelBuilder.Entity<PaymentStatus>().HasKey(x => x.StatusID);
            modelBuilder.Entity<ShipmentStatus>().HasKey(x => x.StatusID);
            modelBuilder.Entity<CreatorPostStatus>().HasKey(x => x.StatusID);
            modelBuilder.Entity<PlatformAnnouncementStatus>().HasKey(x => x.StatusID);
            modelBuilder.Entity<HomepageBannerStatus>().HasKey(x => x.StatusID);
            modelBuilder.Entity<PlatformSettingCategory>().HasKey(x => x.CategoryID);
            modelBuilder.Entity<Report>().HasKey(x => x.ReportID);
            modelBuilder.Entity<ReportStatus>().HasKey(x => x.StatusID);

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
                .WithMany(m => m.MemberRoles)
                .HasForeignKey(mr => mr.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberRole>()
                .HasOne(mr => mr.Role)
                .WithMany(r => r.MemberRoles)
                .HasForeignKey(mr => mr.RoleID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberRoleHistory>()
                .HasOne(h => h.Member)
                .WithMany(m => m.RoleChangeHistories)
                .HasForeignKey(h => h.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MemberRoleHistory>()
                .HasOne(h => h.OperatorMember)
                .WithMany(m => m.OperatedRoleHistories)
                .HasForeignKey(h => h.OperatorMemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MemberRoleHistory>()
                .HasOne(h => h.Role)
                .WithMany(r => r.MemberRoleHistories)
                .HasForeignKey(h => h.RoleID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<CreatorApplication>()
                .HasOne(ca => ca.Member)
                .WithMany(m => m.CreatorApplications)
                .HasForeignKey(ca => ca.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CreatorApplication>()
                .HasOne(ca => ca.Reviewer)
                .WithMany(m => m.ReviewedCreatorApplications)
                .HasForeignKey(ca => ca.ReviewedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CreatorProfile>()
                .HasOne(c => c.Member)
                .WithOne(m => m.CreatorProfile)
                .HasForeignKey<CreatorProfile>(c => c.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.CreatorProfile)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CreatorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductImage>()
                .HasOne(pi => pi.Product)
                .WithMany(p => p.ProductImages)
                .HasForeignKey(pi => pi.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Product)
                .WithMany(p => p.ProductCategories)
                .HasForeignKey(pc => pc.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductCategory>()
                .HasOne(pc => pc.Category)
                .WithMany(c => c.ProductCategories)
                .HasForeignKey(pc => pc.CategoryID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Product)
                .WithMany(p => p.ProductTags)
                .HasForeignKey(pt => pt.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductTag>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.ProductTags)
                .HasForeignKey(pt => pt.TagID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Cart>()
                .HasOne(c => c.Member)
                .WithMany(m => m.Carts)
                .HasForeignKey(c => c.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.CartItems)
                .HasForeignKey(ci => ci.CartID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.CartItems)
                .HasForeignKey(ci => ci.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Member)
                .WithMany(m => m.Orders)
                .HasForeignKey(o => o.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(od => od.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(od => od.Product)
                .WithMany(p => p.OrderDetails)
                .HasForeignKey(od => od.ProductID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Shipment>()
                .HasOne(s => s.Order)
                .WithOne(o => o.Shipment)
                .HasForeignKey<Shipment>(s => s.OrderID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FavoriteProduct>()
                .HasOne(fp => fp.Member)
                .WithMany(m => m.FavoriteProducts)
                .HasForeignKey(fp => fp.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FavoriteProduct>()
                .HasOne(fp => fp.Product)
                .WithMany(p => p.FavoriteProducts)
                .HasForeignKey(fp => fp.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FollowCreator>()
                .HasOne(fc => fc.Member)
                .WithMany(m => m.FollowCreators)
                .HasForeignKey(fc => fc.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<FollowCreator>()
                .HasOne(fc => fc.CreatorProfile)
                .WithMany(c => c.FollowCreators)
                .HasForeignKey(fc => fc.CreatorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Member)
                .WithMany(m => m.ProductReviews)
                .HasForeignKey(pr => pr.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductReview>()
                .HasOne(pr => pr.Product)
                .WithMany(p => p.ProductReviews)
                .HasForeignKey(pr => pr.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MessageThread>()
                .HasOne(mt => mt.Member)
                .WithMany(m => m.MessageThreads)
                .HasForeignKey(mt => mt.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MessageThread>()
                .HasOne(mt => mt.CreatorProfile)
                .WithMany(c => c.MessageThreads)
                .HasForeignKey(mt => mt.CreatorID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.MessageThread)
                .WithMany(mt => mt.Messages)
                .HasForeignKey(m => m.ThreadID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AutoReplyTemplate>()
                .HasOne(a => a.CreatorProfile)
                .WithMany(c => c.AutoReplyTemplates)
                .HasForeignKey(a => a.CreatorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CreatorPost>()
                .HasOne(c => c.CreatorProfile)
                .WithMany(cp => cp.CreatorPosts)
                .HasForeignKey(c => c.CreatorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostComment>()
                .HasOne(pc => pc.CreatorPost)
                .WithMany(c => c.PostComments)
                .HasForeignKey(pc => pc.PostID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostComment>()
                .HasOne(pc => pc.Member)
                .WithMany(m => m.PostComments)
                .HasForeignKey(pc => pc.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inventory>()
                .HasOne(i => i.Product)
                .WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(i => i.ProductID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InventoryAlert>()
                .HasOne(ia => ia.Inventory)
                .WithMany(i => i.InventoryAlerts)
                .HasForeignKey(ia => ia.InventoryID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlatformAnnouncement>()
                .HasOne(pa => pa.Member)
                .WithMany(p => p.PlatformAnnouncements)
                .HasForeignKey(pa => pa.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HomepageBanner>()
                .HasOne(hb => hb.Member)
                .WithMany(h => h.HomepageBanners)
                .HasForeignKey(hb => hb.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlatformSetting>()
                .HasOne(ps => ps.Member)
                .WithMany(p => p.PlatformSettings)
                .HasForeignKey(ps => ps.UpdatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Portfolio>()
                .HasOne(p => p.CreatorProfile)
                .WithMany(c => c.Portfolios)
                .HasForeignKey(p => p.CreatorID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PortfolioItem>()
                .HasOne(pi => pi.Portfolio)
                .WithMany(p => p.PortfolioItems)
                .HasForeignKey(pi => pi.PortfolioID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PortfolioStatus>()
                .HasMany(cps => cps.Portfolio)
                .WithOne(cp => cp.PortfolioStatus)
                .HasForeignKey(cp => cp.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<NotificationPreference>()
                .HasOne(np => np.Member)
                .WithMany(m => m.NotificationPreferences)
                .HasForeignKey(np => np.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NotificationEvent>()
                .HasOne(ne => ne.Member)
                .WithMany(m => m.NotificationEvents)
                .HasForeignKey(ne => ne.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PasswordResetToken>()
                .HasOne(t => t.Member)
                .WithMany(m => m.PasswordResetTokens)
                .HasForeignKey(t => t.MemberID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<MemberStatus>()
                .HasMany(ms => ms.Members)
                .WithOne(m => m.MemberStatus)
                .HasForeignKey(m => m.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CreatorApplicationStatus>()
                .HasMany(cas => cas.CreatorApplications)
                .WithOne(ca => ca.CreatorApplicationStatus)
                .HasForeignKey(ca => ca.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CreatorProfileStatus>()
                .HasMany(cps => cps.CreatorProfiles)
                .WithOne(cp => cp.CreatorProfileStatus)
                .HasForeignKey(cp => cp.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductStatus>()
                .HasMany(ps => ps.Products)
                .WithOne(p => p.ProductStatus)
                .HasForeignKey(p => p.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductImageStatus>()
                .HasMany(pis => pis.ProductImages)
                .WithOne(pi => pi.ProductImageStatus)
                .HasForeignKey(pi => pi.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderStatus>()
                .HasMany(os => os.Orders)
                .WithOne(o => o.OrderStatus)
                .HasForeignKey(o => o.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentMethod>()
                .HasMany(pm => pm.Payments)
                .WithOne(p => p.PaymentMethod)
                .HasForeignKey(p => p.MethodID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PaymentStatus>()
                .HasMany(ps => ps.Payments)
                .WithOne(p => p.PaymentStatus)
                .HasForeignKey(p => p.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ShipmentStatus>()
                .HasMany(ss => ss.Shipments)
                .WithOne(s => s.ShipmentStatus)
                .HasForeignKey(s => s.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<CreatorPostStatus>()
                .HasMany(cps => cps.CreatorPosts)
                .WithOne(cp => cp.CreatorPostStatus)
                .HasForeignKey(cp => cp.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlatformAnnouncementStatus>()
                .HasMany(pas => pas.PlatformAnnouncements)
                .WithOne(pa => pa.PlatformAnnouncementStatus)
                .HasForeignKey(pa => pa.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<HomepageBannerStatus>()
                .HasMany(hbs => hbs.HomepageBanners)
                .WithOne(hb => hb.HomepageBannerStatus)
                .HasForeignKey(hb => hb.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlatformSettingCategory>()
                .HasMany(psc => psc.PlatformSettings)
                .WithOne(ps => ps.PlatformSettingCategory)
                .HasForeignKey(ps => ps.CategoryID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Reporter)
                .WithMany(m => m.ReportsCreated)
                .HasForeignKey(r => r.MemberID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Report>()
                .HasOne(r => r.Reviewer)
                .WithMany(m => m.ReportsReviewed)
                .HasForeignKey(r => r.ReviewedBy)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ReportStatus>()
                .HasMany(pcrs => pcrs.Reports)
                .WithOne(pcr => pcr.ReportStatus)
                .HasForeignKey(pcr => pcr.StatusID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Member)
                .WithMany(m => m.Messages)
                .HasForeignKey(m => m.SenderID)
                .OnDelete(DeleteBehavior.Restrict);

            #endregion

            #region Unique
            modelBuilder.Entity<Privacy>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<Privacy>()
                .HasIndex(p => p.Phone)
                .IsUnique()
                .HasFilter("[Phone] IS NOT NULL"); // 允許 Phone 空值

            modelBuilder.Entity<MemberStatus>()
               .HasIndex(ms => ms.StatusCode)
               .IsUnique();

            modelBuilder.Entity<CreatorApplicationStatus>()
               .HasIndex(cas => cas.StatusCode)
               .IsUnique();

            modelBuilder.Entity<CreatorProfileStatus>()
               .HasIndex(cps => cps.StatusCode)
               .IsUnique();

            modelBuilder.Entity<CreatorProfile>()
                .HasIndex(c => c.MemberID)
                .IsUnique();

            modelBuilder.Entity<ProductStatus>()
                .HasIndex(ps => ps.StatusCode)
                .IsUnique();

            modelBuilder.Entity<ProductImageStatus>()
                .HasIndex(pis => pis.StatusCode)
                .IsUnique();

            modelBuilder.Entity<OrderStatus>()
                .HasIndex(os => os.StatusCode)
                .IsUnique();

            modelBuilder.Entity<PaymentMethod>()
                .HasIndex(pm => pm.MethodCode)
                .IsUnique();

            modelBuilder.Entity<PaymentStatus>()
                .HasIndex(ps => ps.StatusCode)
                .IsUnique();

            modelBuilder.Entity<ShipmentStatus>()
                .HasIndex(ss => ss.StatusCode)
                .IsUnique();

            modelBuilder.Entity<CreatorPostStatus>()
                .HasIndex(cps => cps.StatusCode)
                .IsUnique();

            modelBuilder.Entity<PortfolioStatus>()
                .HasIndex(cps => cps.StatusCode)
                .IsUnique();

            modelBuilder.Entity<PlatformAnnouncementStatus>()
                .HasIndex(pas => pas.StatusCode)
                .IsUnique();

            modelBuilder.Entity<HomepageBannerStatus>()
                .HasIndex(hbs => hbs.StatusCode)
                .IsUnique();

            modelBuilder.Entity<PlatformSettingCategory>()
                .HasIndex(psc => psc.CategoryCode)
                .IsUnique();

            modelBuilder.Entity<ReportStatus>()
                .HasIndex(pcrs => pcrs.StatusCode)
                .IsUnique();

            modelBuilder.Entity<FavoriteProduct>()
                .HasIndex(e => new { e.MemberID, e.ProductID })
                .IsUnique();

            modelBuilder.Entity<FollowCreator>()
                .HasIndex(e => new { e.MemberID, e.CreatorID })
                .IsUnique();

            modelBuilder.Entity<Report>()
                .HasIndex(r => new { r.ReportType, r.TargetID, r.MemberID })
                .IsUnique();

            #endregion

            modelBuilder.Entity<PortfolioItem>()
                .HasQueryFilter(i => !i.IsDeleted);
        }
    }
}