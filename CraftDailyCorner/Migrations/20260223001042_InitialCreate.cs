using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CraftDailyCorner.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ParentCategoryID = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryID);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryID",
                        column: x => x.ParentCategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreatorApplicationStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorApplicationStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "CreatorPostStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorPostStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "CreatorProfileStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorProfileStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "HomepageBannerStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomepageBannerStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "MemberStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "OrderStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                columns: table => new
                {
                    MethodID = table.Column<byte>(type: "tinyint", nullable: false),
                    MethodCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    MethodName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.MethodID);
                });

            migrationBuilder.CreateTable(
                name: "PaymentStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "PlatformAnnouncementStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformAnnouncementStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "PlatformSettingCategories",
                columns: table => new
                {
                    CategoryID = table.Column<byte>(type: "tinyint", nullable: false),
                    CategoryCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformSettingCategories", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "ProductImageStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImageStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "ProductStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "ReportStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleID = table.Column<string>(type: "nchar(2)", maxLength: 2, nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleID);
                });

            migrationBuilder.CreateTable(
                name: "ShipmentStatuses",
                columns: table => new
                {
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipmentStatuses", x => x.StatusID);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    MaliciousReportCount = table.Column<int>(type: "int", nullable: false),
                    ReportBanUntil = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberID);
                    table.ForeignKey(
                        name: "FK_Members_MemberStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "MemberStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    CartID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.CartID);
                    table.ForeignKey(
                        name: "FK_Carts_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatorApplications",
                columns: table => new
                {
                    ApplicationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DisplayName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Intro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PortfolioSampleUrl = table.Column<string>(type: "nchar(36)", maxLength: 36, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReviewNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false),
                    ReviewedBy = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorApplications", x => x.ApplicationID);
                    table.ForeignKey(
                        name: "FK_CreatorApplications_CreatorApplicationStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "CreatorApplicationStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreatorApplications_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreatorApplications_Members_ReviewedBy",
                        column: x => x.ReviewedBy,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CreatorProfiles",
                columns: table => new
                {
                    CreatorID = table.Column<string>(type: "nchar(6)", maxLength: 6, nullable: false),
                    ImageUrl = table.Column<string>(type: "nchar(36)", maxLength: 36, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Intro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BankCode = table.Column<string>(type: "nchar(3)", maxLength: 3, nullable: true),
                    BankAccount = table.Column<string>(type: "nvarchar(14)", maxLength: 14, nullable: true),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorProfiles", x => x.CreatorID);
                    table.ForeignKey(
                        name: "FK_CreatorProfiles_CreatorProfileStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "CreatorProfileStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreatorProfiles_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "HomepageBanners",
                columns: table => new
                {
                    BannerID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nchar(36)", maxLength: 36, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Subtitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomepageBanners", x => x.BannerID);
                    table.ForeignKey(
                        name: "FK_HomepageBanners_HomepageBannerStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "HomepageBannerStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HomepageBanners_Members_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberRoleHistories",
                columns: table => new
                {
                    MemberRoleHistoryID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Action = table.Column<byte>(type: "tinyint", nullable: false),
                    OperatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false),
                    RoleID = table.Column<string>(type: "nchar(2)", maxLength: 2, nullable: false),
                    OperatedBy = table.Column<byte>(type: "tinyint", nullable: false),
                    OperatorMemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRoleHistories", x => x.MemberRoleHistoryID);
                    table.ForeignKey(
                        name: "FK_MemberRoleHistories_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRoleHistories_Members_OperatorMemberID",
                        column: x => x.OperatorMemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MemberRoleHistories_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MemberRoles",
                columns: table => new
                {
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false),
                    RoleID = table.Column<string>(type: "nchar(2)", maxLength: 2, nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberRoles", x => new { x.MemberID, x.RoleID });
                    table.ForeignKey(
                        name: "FK_MemberRoles_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberRoles_Roles_RoleID",
                        column: x => x.RoleID,
                        principalTable: "Roles",
                        principalColumn: "RoleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationEvents",
                columns: table => new
                {
                    EventID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationType = table.Column<byte>(type: "tinyint", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationEvents", x => x.EventID);
                    table.ForeignKey(
                        name: "FK_NotificationEvents_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationPreferences",
                columns: table => new
                {
                    PreferenceID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotificationType = table.Column<byte>(type: "tinyint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationPreferences", x => x.PreferenceID);
                    table.ForeignKey(
                        name: "FK_NotificationPreferences_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderID = table.Column<string>(type: "nchar(12)", maxLength: 12, nullable: false),
                    ReceiverName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ReceiverPhone = table.Column<string>(type: "nchar(10)", maxLength: 10, nullable: false),
                    ShippingAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "money", nullable: false),
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderID);
                    table.ForeignKey(
                        name: "FK_Orders_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_OrderStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "OrderStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PasswordResetTokens",
                columns: table => new
                {
                    PasswordResetId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MemberID = table.Column<string>(type: "nchar(8)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Used = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PasswordResetTokens", x => x.PasswordResetId);
                    table.ForeignKey(
                        name: "FK_PasswordResetTokens_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PlatformAnnouncements",
                columns: table => new
                {
                    AnnouncementID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nchar(8)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformAnnouncements", x => x.AnnouncementID);
                    table.ForeignKey(
                        name: "FK_PlatformAnnouncements_Members_CreatedBy",
                        column: x => x.CreatedBy,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlatformAnnouncements_PlatformAnnouncementStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "PlatformAnnouncementStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PlatformSettings",
                columns: table => new
                {
                    SettingID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SettingKey = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SettingValue = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DataType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CategoryID = table.Column<byte>(type: "tinyint", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlatformSettings", x => x.SettingID);
                    table.ForeignKey(
                        name: "FK_PlatformSettings_Members_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PlatformSettings_PlatformSettingCategories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "PlatformSettingCategories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Privacies",
                columns: table => new
                {
                    MemberID = table.Column<string>(type: "nchar(8)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nchar(10)", nullable: true),
                    Birthday = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Privacies", x => x.MemberID);
                    table.ForeignKey(
                        name: "FK_Privacies_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AutoReplyTemplates",
                columns: table => new
                {
                    TemplateID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    TriggerType = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorID = table.Column<string>(type: "nchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AutoReplyTemplates", x => x.TemplateID);
                    table.ForeignKey(
                        name: "FK_AutoReplyTemplates_CreatorProfiles_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "CreatorProfiles",
                        principalColumn: "CreatorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreatorPosts",
                columns: table => new
                {
                    PostID = table.Column<string>(type: "nchar(36)", maxLength: 36, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nchar(36)", nullable: false),
                    Visibility = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorID = table.Column<string>(type: "nchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreatorPosts", x => x.PostID);
                    table.ForeignKey(
                        name: "FK_CreatorPosts_CreatorPostStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "CreatorPostStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreatorPosts_CreatorProfiles_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "CreatorProfiles",
                        principalColumn: "CreatorID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FollowCreators",
                columns: table => new
                {
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false),
                    CreatorID = table.Column<string>(type: "nchar(6)", maxLength: 6, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FollowCreators", x => new { x.MemberID, x.CreatorID });
                    table.ForeignKey(
                        name: "FK_FollowCreators_CreatorProfiles_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "CreatorProfiles",
                        principalColumn: "CreatorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FollowCreators_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageThreads",
                columns: table => new
                {
                    ThreadID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false),
                    CreatorID = table.Column<string>(type: "nchar(6)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageThreads", x => x.ThreadID);
                    table.ForeignKey(
                        name: "FK_MessageThreads_CreatorProfiles_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "CreatorProfiles",
                        principalColumn: "CreatorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageThreads_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Portfolios",
                columns: table => new
                {
                    PortfolioID = table.Column<string>(type: "nchar(36)", maxLength: 36, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Visibility = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorID = table.Column<string>(type: "nchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portfolios", x => x.PortfolioID);
                    table.ForeignKey(
                        name: "FK_Portfolios_CreatorProfiles_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "CreatorProfiles",
                        principalColumn: "CreatorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Portfolios_PortfolioStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "PortfolioStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<string>(type: "nchar(10)", maxLength: 10, nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "money", nullable: false),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorID = table.Column<string>(type: "nchar(6)", maxLength: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_CreatorProfiles_CreatorID",
                        column: x => x.CreatorID,
                        principalTable: "CreatorProfiles",
                        principalColumn: "CreatorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Products_ProductStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "ProductStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    PaymentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MethodID = table.Column<byte>(type: "tinyint", nullable: false),
                    Amount = table.Column<decimal>(type: "money", nullable: false),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    GatewayTradeNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AttemptNo = table.Column<byte>(type: "tinyint", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderID = table.Column<string>(type: "nchar(12)", maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.PaymentID);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentMethods_MethodID",
                        column: x => x.MethodID,
                        principalTable: "PaymentMethods",
                        principalColumn: "MethodID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "PaymentStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Shipments",
                columns: table => new
                {
                    ShipmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackingNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    OrderID = table.Column<string>(type: "nchar(12)", maxLength: 12, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shipments", x => x.ShipmentID);
                    table.ForeignKey(
                        name: "FK_Shipments_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shipments_ShipmentStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "ShipmentStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PostComments",
                columns: table => new
                {
                    CommentID = table.Column<string>(type: "nchar(36)", maxLength: 36, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PostID = table.Column<string>(type: "nchar(36)", maxLength: 36, nullable: false),
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostComments", x => x.CommentID);
                    table.ForeignKey(
                        name: "FK_PostComments_CreatorPosts_PostID",
                        column: x => x.PostID,
                        principalTable: "CreatorPosts",
                        principalColumn: "PostID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostComments_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    MessageID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ThreadID = table.Column<int>(type: "int", nullable: false),
                    SenderID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.MessageID);
                    table.ForeignKey(
                        name: "FK_Messages_Members_SenderID",
                        column: x => x.SenderID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_MessageThreads_ThreadID",
                        column: x => x.ThreadID,
                        principalTable: "MessageThreads",
                        principalColumn: "ThreadID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PortfolioItems",
                columns: table => new
                {
                    ItemID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nchar(36)", maxLength: 36, nullable: false),
                    SortOrder = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PortfolioID = table.Column<string>(type: "nchar(36)", maxLength: 36, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PortfolioItems", x => x.ItemID);
                    table.ForeignKey(
                        name: "FK_PortfolioItems_Portfolios_PortfolioID",
                        column: x => x.PortfolioID,
                        principalTable: "Portfolios",
                        principalColumn: "PortfolioID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<string>(type: "nchar(10)", maxLength: 10, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => new { x.CartID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartID",
                        column: x => x.CartID,
                        principalTable: "Carts",
                        principalColumn: "CartID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FavoriteProducts",
                columns: table => new
                {
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false),
                    ProductID = table.Column<string>(type: "nchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteProducts", x => new { x.MemberID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FavoriteProducts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inventories",
                columns: table => new
                {
                    InventoryID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StockQty = table.Column<int>(type: "int", nullable: false),
                    AlertQty = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<string>(type: "nchar(10)", maxLength: 10, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventories", x => x.InventoryID);
                    table.ForeignKey(
                        name: "FK_Inventories_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrderID = table.Column<string>(type: "nchar(12)", maxLength: 12, nullable: false),
                    ProductID = table.Column<string>(type: "nchar(10)", maxLength: 10, nullable: false),
                    ProductNameSnapshot = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    PriceSnapshot = table.Column<decimal>(type: "money", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => new { x.OrderID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrderID",
                        column: x => x.OrderID,
                        principalTable: "Orders",
                        principalColumn: "OrderID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    ProductID = table.Column<string>(type: "nchar(10)", maxLength: 10, nullable: false),
                    CategoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => new { x.ProductID, x.CategoryID });
                    table.ForeignKey(
                        name: "FK_ProductCategories_Categories_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Categories",
                        principalColumn: "CategoryID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCategories_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    ImageID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImageUrl = table.Column<string>(type: "nchar(36)", maxLength: 36, nullable: false),
                    SortOrder = table.Column<byte>(type: "tinyint", nullable: false),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    ProductID = table.Column<string>(type: "nchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.ImageID);
                    table.ForeignKey(
                        name: "FK_ProductImages_ProductImageStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "ProductImageStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductReviews",
                columns: table => new
                {
                    ReviewID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Rating = table.Column<byte>(type: "tinyint", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MemberID = table.Column<string>(type: "nchar(8)", maxLength: 8, nullable: false),
                    ProductID = table.Column<string>(type: "nchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductReviews", x => x.ReviewID);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductReviews_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductTags",
                columns: table => new
                {
                    ProductID = table.Column<string>(type: "nchar(10)", maxLength: 10, nullable: false),
                    TagID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductTags", x => new { x.ProductID, x.TagID });
                    table.ForeignKey(
                        name: "FK_ProductTags_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductTags_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ReportID = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportType = table.Column<byte>(type: "tinyint", nullable: false),
                    TargetID = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: false),
                    ReasonCode = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    MemberID = table.Column<string>(type: "nchar(8)", nullable: false),
                    StatusID = table.Column<byte>(type: "tinyint", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReviewedBy = table.Column<string>(type: "nchar(8)", nullable: true),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PostCommentCommentID = table.Column<string>(type: "nchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ReportID);
                    table.ForeignKey(
                        name: "FK_Reports_Members_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_Members_ReviewedBy",
                        column: x => x.ReviewedBy,
                        principalTable: "Members",
                        principalColumn: "MemberID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_PostComments_PostCommentCommentID",
                        column: x => x.PostCommentCommentID,
                        principalTable: "PostComments",
                        principalColumn: "CommentID");
                    table.ForeignKey(
                        name: "FK_Reports_ReportStatuses_StatusID",
                        column: x => x.StatusID,
                        principalTable: "ReportStatuses",
                        principalColumn: "StatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InventoryAlerts",
                columns: table => new
                {
                    AlertID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TriggeredAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<byte>(type: "tinyint", nullable: false),
                    InventoryID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventoryAlerts", x => x.AlertID);
                    table.ForeignKey(
                        name: "FK_InventoryAlerts_Inventories_InventoryID",
                        column: x => x.InventoryID,
                        principalTable: "Inventories",
                        principalColumn: "InventoryID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AutoReplyTemplates_CreatorID",
                table: "AutoReplyTemplates",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductID",
                table: "CartItems",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_MemberID",
                table: "Carts",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryID",
                table: "Categories",
                column: "ParentCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorApplications_MemberID",
                table: "CreatorApplications",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorApplications_ReviewedBy",
                table: "CreatorApplications",
                column: "ReviewedBy");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorApplications_StatusID",
                table: "CreatorApplications",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorApplicationStatuses_StatusCode",
                table: "CreatorApplicationStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreatorPosts_CreatorID",
                table: "CreatorPosts",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorPosts_StatusID",
                table: "CreatorPosts",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorPostStatuses_StatusCode",
                table: "CreatorPostStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreatorProfiles_MemberID",
                table: "CreatorProfiles",
                column: "MemberID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CreatorProfiles_StatusID",
                table: "CreatorProfiles",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_CreatorProfileStatuses_StatusCode",
                table: "CreatorProfileStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_MemberID_ProductID",
                table: "FavoriteProducts",
                columns: new[] { "MemberID", "ProductID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteProducts_ProductID",
                table: "FavoriteProducts",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_FollowCreators_CreatorID",
                table: "FollowCreators",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_FollowCreators_MemberID_CreatorID",
                table: "FollowCreators",
                columns: new[] { "MemberID", "CreatorID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_HomepageBanners_CreatedBy",
                table: "HomepageBanners",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_HomepageBanners_StatusID",
                table: "HomepageBanners",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_HomepageBannerStatuses_StatusCode",
                table: "HomepageBannerStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventories_ProductID",
                table: "Inventories",
                column: "ProductID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryAlerts_InventoryID",
                table: "InventoryAlerts",
                column: "InventoryID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRoleHistories_MemberID",
                table: "MemberRoleHistories",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRoleHistories_OperatorMemberID",
                table: "MemberRoleHistories",
                column: "OperatorMemberID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRoleHistories_RoleID",
                table: "MemberRoleHistories",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberRoles_RoleID",
                table: "MemberRoles",
                column: "RoleID");

            migrationBuilder.CreateIndex(
                name: "IX_Members_StatusID",
                table: "Members",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_MemberStatuses_StatusCode",
                table: "MemberStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderID",
                table: "Messages",
                column: "SenderID");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ThreadID",
                table: "Messages",
                column: "ThreadID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageThreads_CreatorID",
                table: "MessageThreads",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_MessageThreads_MemberID",
                table: "MessageThreads",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationEvents_MemberID",
                table: "NotificationEvents",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationPreferences_MemberID",
                table: "NotificationPreferences",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductID",
                table: "OrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_MemberID",
                table: "Orders",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StatusID",
                table: "Orders",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStatuses_StatusCode",
                table: "OrderStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PasswordResetTokens_MemberID",
                table: "PasswordResetTokens",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_MethodCode",
                table: "PaymentMethods",
                column: "MethodCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MethodID",
                table: "Payments",
                column: "MethodID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderID",
                table: "Payments",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_StatusID",
                table: "Payments",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentStatuses_StatusCode",
                table: "PaymentStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformAnnouncements_CreatedBy",
                table: "PlatformAnnouncements",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformAnnouncements_StatusID",
                table: "PlatformAnnouncements",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformAnnouncementStatuses_StatusCode",
                table: "PlatformAnnouncementStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformSettingCategories_CategoryCode",
                table: "PlatformSettingCategories",
                column: "CategoryCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlatformSettings_CategoryID",
                table: "PlatformSettings",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_PlatformSettings_UpdatedBy",
                table: "PlatformSettings",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioItems_PortfolioID",
                table: "PortfolioItems",
                column: "PortfolioID");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_CreatorID",
                table: "Portfolios",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Portfolios_StatusID",
                table: "Portfolios",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_PortfolioStatuses_StatusCode",
                table: "PortfolioStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_MemberID",
                table: "PostComments",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_PostComments_PostID",
                table: "PostComments",
                column: "PostID");

            migrationBuilder.CreateIndex(
                name: "IX_Privacies_Email",
                table: "Privacies",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Privacies_Phone",
                table: "Privacies",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCategories_CategoryID",
                table: "ProductCategories",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductID",
                table: "ProductImages",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_StatusID",
                table: "ProductImages",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductImageStatuses_StatusCode",
                table: "ProductImageStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_MemberID",
                table: "ProductReviews",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductReviews_ProductID",
                table: "ProductReviews",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CreatorID",
                table: "Products",
                column: "CreatorID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_StatusID",
                table: "Products",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStatuses_StatusCode",
                table: "ProductStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductTags_TagID",
                table: "ProductTags",
                column: "TagID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_MemberID",
                table: "Reports",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PostCommentCommentID",
                table: "Reports",
                column: "PostCommentCommentID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportType_TargetID_MemberID",
                table: "Reports",
                columns: new[] { "ReportType", "TargetID", "MemberID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReviewedBy",
                table: "Reports",
                column: "ReviewedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_StatusID",
                table: "Reports",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ReportStatuses_StatusCode",
                table: "ReportStatuses",
                column: "StatusCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_OrderID",
                table: "Shipments",
                column: "OrderID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Shipments_StatusID",
                table: "Shipments",
                column: "StatusID");

            migrationBuilder.CreateIndex(
                name: "IX_ShipmentStatuses_StatusCode",
                table: "ShipmentStatuses",
                column: "StatusCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AutoReplyTemplates");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "CreatorApplications");

            migrationBuilder.DropTable(
                name: "FavoriteProducts");

            migrationBuilder.DropTable(
                name: "FollowCreators");

            migrationBuilder.DropTable(
                name: "HomepageBanners");

            migrationBuilder.DropTable(
                name: "InventoryAlerts");

            migrationBuilder.DropTable(
                name: "MemberRoleHistories");

            migrationBuilder.DropTable(
                name: "MemberRoles");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "NotificationEvents");

            migrationBuilder.DropTable(
                name: "NotificationPreferences");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "PasswordResetTokens");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PlatformAnnouncements");

            migrationBuilder.DropTable(
                name: "PlatformSettings");

            migrationBuilder.DropTable(
                name: "PortfolioItems");

            migrationBuilder.DropTable(
                name: "Privacies");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropTable(
                name: "ProductReviews");

            migrationBuilder.DropTable(
                name: "ProductTags");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Shipments");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "CreatorApplicationStatuses");

            migrationBuilder.DropTable(
                name: "HomepageBannerStatuses");

            migrationBuilder.DropTable(
                name: "Inventories");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "MessageThreads");

            migrationBuilder.DropTable(
                name: "PaymentMethods");

            migrationBuilder.DropTable(
                name: "PaymentStatuses");

            migrationBuilder.DropTable(
                name: "PlatformAnnouncementStatuses");

            migrationBuilder.DropTable(
                name: "PlatformSettingCategories");

            migrationBuilder.DropTable(
                name: "Portfolios");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "ProductImageStatuses");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "PostComments");

            migrationBuilder.DropTable(
                name: "ReportStatuses");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "ShipmentStatuses");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "PortfolioStatuses");

            migrationBuilder.DropTable(
                name: "CreatorPosts");

            migrationBuilder.DropTable(
                name: "OrderStatuses");

            migrationBuilder.DropTable(
                name: "ProductStatuses");

            migrationBuilder.DropTable(
                name: "CreatorPostStatuses");

            migrationBuilder.DropTable(
                name: "CreatorProfiles");

            migrationBuilder.DropTable(
                name: "CreatorProfileStatuses");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "MemberStatuses");
        }
    }
}
