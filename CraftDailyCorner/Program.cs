using CraftDailyCorner.Models;
using CraftDailyCorner.Seed;
using CraftDailyCorner.Seed.Datas;
using CraftDailyCorner.Services;
using CraftDailyCorner.Services.Creator;
using CraftDailyCorner.Services.Interface;
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
builder.Services.AddScoped <SeedPostCommentReportStatus>();
builder.Services.AddAuthentication("CraftDailyCornerLogin").AddCookie("CraftDailyCornerLogin", option =>
{
    option.LoginPath = "/Account/Login";//設定登入頁面路徑(入口)，若需登入而未登入時強制導到此路徑
    option.LogoutPath = "/Account/Logout";//設定登出頁面路徑
    option.AccessDeniedPath = "/Home/Index";//設定存取被拒絕頁面路徑(若已登入但角色權限不符,則強制導到此路徑)
});
builder.Services.AddScoped<MemberService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<PriceService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<FavoriteService>();
builder.Services.AddScoped<MemberCenterService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<CreatorApplicationService>();

builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();




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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();