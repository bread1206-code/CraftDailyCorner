using CraftDailyCorner.ImageManagementCore.Services;
using CraftDailyCorner.ImageManagementCore.Services.Interfaces;
using CraftDailyCorner.Models;
using CraftDailyCorner.Seed;
using CraftDailyCorner.Seed.Datas;
using CraftDailyCorner.Services;
using CraftDailyCorner.Services.BackgroundServices;
using CraftDailyCorner.Services.Creator;
using CraftDailyCorner.Services.Interface;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CraftDailyCornerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CraftDailyCornerConnection")));
builder.Services.AddScoped <IImageUploadService, ImageUploadService>();
builder.Services.AddScoped <ISiteSettingService, SiteSettingService>();
builder.Services.AddScoped<ICreatorApplicationService, CreatorApplicationService>();
builder.Services.AddScoped<ICreatorPortfolioService, CreatorPortfolioService>();
builder.Services.AddScoped<ICreatorPostService, CreatorPostService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICreatorDashboardService, CreatorDashboardService>();
builder.Services.AddScoped<ICreatorPublicService, CreatorPublicService>();
builder.Services.AddScoped<IFollowService, FollowService>();
builder.Services.AddScoped<ICreatorPortfolioItemService, CreatorPortfolioItemService>();
builder.Services.AddScoped<IImageFileService, ImageFileService>();
builder.Services.AddScoped<ICreatorPostCommentService, CreatorPostCommentService>();
builder.Services.AddScoped<ISoftDeleteCleanupTask, SoftDeleteCleanupTask>();
builder.Services.AddScoped<ISoftDeleteCleanupTask, CreatorPostCleanupTask>();
builder.Services.AddScoped<IImageManagementService, ProductImageService>();
builder.Services.AddScoped<IImageManagementService, PortfolioImageService>();
builder.Services.AddScoped<ICreatorApplicationService, CreatorApplicationService>();
builder.Services.AddScoped<ICreatorOrderService, CreatorOrderService>();
builder.Services.AddScoped<ICreatorPickListService, CreatorPickListService>();
builder.Services.AddScoped<ICreatorShipmentService, CreatorShipmentService>();
builder.Services.AddScoped<ICreatorAnalyticsService, CreatorAnalyticsService>();
builder.Services.AddScoped<IAdminDashboardService, AdminDashboardService>();
builder.Services.AddScoped<IAdminSidebarService, AdminSidebarService>();
builder.Services.AddScoped<IReactionService, ReactionService>();
builder.Services.AddScoped<IMemberSecurityService, MemberSecurityService>();
builder.Services.AddScoped<IPasswordHasher<Privacy>, PasswordHasher<Privacy>>();
builder.Services.AddScoped<ICreatorProfileService, CreatorProfileService>();
builder.Services.AddScoped<IAdminCreatorReviewService, AdminCreatorReviewService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAdminViolationService, AdminViolationService>();
builder.Services.AddScoped<IAdminCategoryService, AdminCategoryService>();

builder.Services.AddHostedService<SoftDeleteCleanupBackgroundService>();
builder.Services.AddHostedService<OrderAutoCompleteHostedBackgroundService>();
builder.Services.AddScoped <SeedRunner>();
builder.Services.AddScoped <SeedMember>();
builder.Services.AddScoped <SeedPrivacy>();
builder.Services.AddScoped <SeedRole>();
builder.Services.AddScoped <SeedMemberRole>();
builder.Services.AddScoped <SeedMemberRoleHistory>();
builder.Services.AddScoped <SeedCreatorApplication>();
builder.Services.AddScoped <SeedCreatorProfile>();
builder.Services.AddScoped <SeedProduct>();
builder.Services.AddScoped <SeedProductImage>();
builder.Services.AddScoped <SeedCategory>();
builder.Services.AddScoped <SeedTag>();
builder.Services.AddScoped <SeedProductRelation>();
builder.Services.AddScoped <SeedCart>();
builder.Services.AddScoped <SeedCartItem>();
builder.Services.AddScoped <SeedOrder>();
builder.Services.AddScoped <SeedOrderDetail>();
builder.Services.AddScoped <SeedPayment>();
builder.Services.AddScoped <SeedShipment>();
builder.Services.AddScoped <SeedFavoriteProduct>();
builder.Services.AddScoped <SeedFollowCreator>();
builder.Services.AddScoped <SeedProductReview>();
builder.Services.AddScoped <SeedMessageThread>();
builder.Services.AddScoped <SeedMessage>();
builder.Services.AddScoped <SeedAutoReplyTemplate>();
builder.Services.AddScoped <SeedCreatorPost>();
builder.Services.AddScoped <SeedPostComment>();
builder.Services.AddScoped <SeedInventory>();
builder.Services.AddScoped <SeedInventoryAlert>();
builder.Services.AddScoped <SeedPlatformAnnouncement>();
builder.Services.AddScoped <SeedHomepageBanner>();
builder.Services.AddScoped <SeedPlatformSetting>();
builder.Services.AddScoped <SeedPortfolio>();
builder.Services.AddScoped <SeedPortfolioItem>();
builder.Services.AddScoped <SeedPortfolioStatus>();
builder.Services.AddScoped <SeedNotificationPreference>();
builder.Services.AddScoped <SeedNotificationEvent>();
builder.Services.AddScoped <SeedMemberStatus>();
builder.Services.AddScoped <SeedCreatorApplicationStatus>();
builder.Services.AddScoped <SeedCreatorProfileStatus>();
builder.Services.AddScoped <SeedProductStatus>();
builder.Services.AddScoped <SeedProductImageStatus>();
builder.Services.AddScoped <SeedOrderStatus>();
builder.Services.AddScoped <SeedPaymentMethod>();
builder.Services.AddScoped <SeedPaymentStatus>();
builder.Services.AddScoped <SeedShipmentStatus>();
builder.Services.AddScoped <SeedCreatorPostStatus>();
builder.Services.AddScoped <SeedPlatformAnnouncementStatus>();
builder.Services.AddScoped <SeedHomepageBannerStatus>();
builder.Services.AddScoped <SeedPlatformSettingCategory>();
builder.Services.AddScoped <SeedReportStatus>();
builder.Services.AddAuthentication("CraftDailyCornerLogin").AddCookie("CraftDailyCornerLogin", option =>
{
    option.LoginPath = "/Account/Login";//設定登入頁面路徑(入口)，若需登入而未登入時強制導到此路徑
    option.LogoutPath = "/Account/Logout";//設定登出頁面路徑
    option.AccessDeniedPath = "/Home/Index";//設定存取被拒絕頁面路徑(若已登入但角色權限不符,則強制導到此路徑)
});
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<PriceService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IMemberCenterService, MemberCenterService>();
builder.Services.AddScoped<ProductService>();

builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<CreatorProductService>();
builder.Services.AddScoped<CreatorProductImageService>();


builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "RequestVerificationToken";
});
//提高上傳容量
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 200 * 1024 * 1024; // 200MB (整包表單)
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 200 * 1024 * 1024; // 200MB
});


var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    //SeedData.Initialize(scope.ServiceProvider);
    var services = scope.ServiceProvider;
    var runner = services.GetRequiredService<SeedRunner>();
    runner.Run();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");


//預設路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();