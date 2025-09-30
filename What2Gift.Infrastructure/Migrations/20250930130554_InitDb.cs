using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace What2Gift.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "Brands",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplate",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<string>(type: "text", nullable: false),
                    Header = table.Column<string>(type: "text", nullable: false),
                    MainContent = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplate", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Category = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SpentAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IncomeSources",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncomeSources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Occasions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    DateRangeStart = table.Column<DateOnly>(type: "date", nullable: false),
                    DateRangeEnd = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Occasions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    MembershipStatus = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Revenues",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IncomeSourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revenues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Revenues_IncomeSources_IncomeSourceId",
                        column: x => x.IncomeSourceId,
                        principalSchema: "public",
                        principalTable: "IncomeSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BrandId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    OccasionId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Brands_BrandId",
                        column: x => x.BrandId,
                        principalSchema: "public",
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "public",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_Occasions_OccasionId",
                        column: x => x.OccasionId,
                        principalSchema: "public",
                        principalTable: "Occasions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MembershipType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    StartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    EndDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Message = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GiftSuggestions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    OccasionId = table.Column<Guid>(type: "uuid", nullable: true),
                    RecipientGender = table.Column<int>(type: "integer", nullable: false),
                    RecipientAge = table.Column<int>(type: "integer", nullable: false),
                    RecipientHobby = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    BudgetMin = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    BudgetMax = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    SuggestedProductId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GiftSuggestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GiftSuggestions_Occasions_OccasionId",
                        column: x => x.OccasionId,
                        principalSchema: "public",
                        principalTable: "Occasions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GiftSuggestions_Products_SuggestedProductId",
                        column: x => x.SuggestedProductId,
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_GiftSuggestions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductSources",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    VendorName = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    AffiliateLink = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSources", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSources_Products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "public",
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    SuggestionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Rating = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_GiftSuggestions_SuggestionId",
                        column: x => x.SuggestionId,
                        principalSchema: "public",
                        principalTable: "GiftSuggestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AffiliateClicks",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    ProductSourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ClickedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AffiliateClicks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AffiliateClicks_ProductSources_ProductSourceId",
                        column: x => x.ProductSourceId,
                        principalSchema: "public",
                        principalTable: "ProductSources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AffiliateClicks_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateClicks_ProductSourceId",
                schema: "public",
                table: "AffiliateClicks",
                column: "ProductSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_AffiliateClicks_UserId",
                schema: "public",
                table: "AffiliateClicks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_SuggestionId",
                schema: "public",
                table: "Feedbacks",
                column: "SuggestionId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_UserId",
                schema: "public",
                table: "Feedbacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftSuggestions_OccasionId",
                schema: "public",
                table: "GiftSuggestions",
                column: "OccasionId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftSuggestions_SuggestedProductId",
                schema: "public",
                table: "GiftSuggestions",
                column: "SuggestedProductId");

            migrationBuilder.CreateIndex(
                name: "IX_GiftSuggestions_UserId",
                schema: "public",
                table: "GiftSuggestions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId",
                schema: "public",
                table: "Memberships",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                schema: "public",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_BrandId",
                schema: "public",
                table: "Products",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                schema: "public",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_OccasionId",
                schema: "public",
                table: "Products",
                column: "OccasionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSources_ProductId",
                schema: "public",
                table: "ProductSources",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Revenues_IncomeSourceId",
                schema: "public",
                table: "Revenues",
                column: "IncomeSourceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AffiliateClicks",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmailTemplate",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Expenses",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Feedbacks",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Memberships",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Notifications",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Revenues",
                schema: "public");

            migrationBuilder.DropTable(
                name: "ProductSources",
                schema: "public");

            migrationBuilder.DropTable(
                name: "GiftSuggestions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "IncomeSources",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Products",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Brands",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Occasions",
                schema: "public");
        }
    }
}
