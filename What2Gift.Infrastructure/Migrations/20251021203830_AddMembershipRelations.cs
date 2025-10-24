using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace What2Gift.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMembershipRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembershipType",
                schema: "public",
                table: "Memberships");

            migrationBuilder.AddColumn<Guid>(
                name: "MembershipPlanId",
                schema: "public",
                table: "Memberships",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "MembershipPlans",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTransactions",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MembershipPlanId = table.Column<Guid>(type: "uuid", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    PaymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TransactionCode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Status = table.Column<int>(type: "integer", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_MembershipPlans_MembershipPlanId",
                        column: x => x.MembershipPlanId,
                        principalSchema: "public",
                        principalTable: "MembershipPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentTransactions_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "public",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "MembershipPlans",
                columns: new[] { "Id", "Description", "Name", "Price" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Gói Basic: quyền truy cập cơ bản trong 1 tháng", "Basic", 20000m },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Gói Pro: đầy đủ quyền lợi trong 1 tháng", "Pro", 40000m }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_MembershipPlanId",
                schema: "public",
                table: "Memberships",
                column: "MembershipPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_MembershipPlanId",
                schema: "public",
                table: "PaymentTransactions",
                column: "MembershipPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTransactions_UserId",
                schema: "public",
                table: "PaymentTransactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_MembershipPlans_MembershipPlanId",
                schema: "public",
                table: "Memberships",
                column: "MembershipPlanId",
                principalSchema: "public",
                principalTable: "MembershipPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_MembershipPlans_MembershipPlanId",
                schema: "public",
                table: "Memberships");

            migrationBuilder.DropTable(
                name: "PaymentTransactions",
                schema: "public");

            migrationBuilder.DropTable(
                name: "MembershipPlans",
                schema: "public");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_MembershipPlanId",
                schema: "public",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "MembershipPlanId",
                schema: "public",
                table: "Memberships");

            migrationBuilder.AddColumn<string>(
                name: "MembershipType",
                schema: "public",
                table: "Memberships",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
