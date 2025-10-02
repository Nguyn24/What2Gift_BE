using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace What2Gift.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class seedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRangeEnd",
                schema: "public",
                table: "Occasions");

            migrationBuilder.DropColumn(
                name: "DateRangeStart",
                schema: "public",
                table: "Occasions");

            migrationBuilder.AddColumn<int>(
                name: "EndDay",
                schema: "public",
                table: "Occasions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EndMonth",
                schema: "public",
                table: "Occasions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartDay",
                schema: "public",
                table: "Occasions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StartMonth",
                schema: "public",
                table: "Occasions",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                schema: "public",
                table: "Brands",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Local handmade crafts and souvenirs", "Handmade Corner" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Fresh flowers and dried bouquets", "Bloom & Co" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Cakes, cookies, and sweet gift sets", "Sweet Delights" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Personalized gifts and accessories", "Giftopia" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("aaaa1111-1111-1111-1111-111111111111"), "Fresh flowers, dried flowers, bouquet arrangements", "Flowers" },
                    { new Guid("bbbb2222-2222-2222-2222-222222222222"), "Handmade gifts, souvenirs, and DIY sets", "Handmade Crafts" },
                    { new Guid("cccc3333-3333-3333-3333-333333333333"), "Cakes, chocolates, candy gift boxes", "Food & Sweets" },
                    { new Guid("dddd4444-4444-4444-4444-444444444444"), "Jewelry, watches, and small fashion gifts", "Accessories" }
                });

            migrationBuilder.InsertData(
                schema: "public",
                table: "Occasions",
                columns: new[] { "Id", "Description", "EndDay", "EndMonth", "Name", "StartDay", "StartMonth" },
                values: new object[,]
                {
                    { new Guid("66666666-4444-4444-4444-444444444444"), "Special gifts to celebrate moms", 12, 5, "Mother's Day", 10, 5 },
                    { new Guid("77777777-3333-3333-3333-333333333333"), "Romantic gifts for Valentine's Day", 14, 2, "Valentine", 14, 2 },
                    { new Guid("88888888-2222-2222-2222-222222222222"), "Warm gifts for Christmas holidays", 31, 12, "Christmas", 20, 12 },
                    { new Guid("99999999-1111-1111-1111-111111111111"), "Perfect gifts for birthdays", 31, 12, "Birthday", 1, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "public",
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Occasions",
                keyColumn: "Id",
                keyValue: new Guid("66666666-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Occasions",
                keyColumn: "Id",
                keyValue: new Guid("77777777-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Occasions",
                keyColumn: "Id",
                keyValue: new Guid("88888888-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                schema: "public",
                table: "Occasions",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111111"));

            migrationBuilder.DropColumn(
                name: "EndDay",
                schema: "public",
                table: "Occasions");

            migrationBuilder.DropColumn(
                name: "EndMonth",
                schema: "public",
                table: "Occasions");

            migrationBuilder.DropColumn(
                name: "StartDay",
                schema: "public",
                table: "Occasions");

            migrationBuilder.DropColumn(
                name: "StartMonth",
                schema: "public",
                table: "Occasions");

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateRangeEnd",
                schema: "public",
                table: "Occasions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));

            migrationBuilder.AddColumn<DateOnly>(
                name: "DateRangeStart",
                schema: "public",
                table: "Occasions",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
