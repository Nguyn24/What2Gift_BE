using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace What2Gift.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDescriptionColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "Occasions");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "Description",
                schema: "public",
                table: "Brands");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "Occasions",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "Categories",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "public",
                table: "Brands",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "Description",
                value: "Local handmade crafts and souvenirs");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"),
                column: "Description",
                value: "Fresh flowers and dried bouquets");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"),
                column: "Description",
                value: "Cakes, cookies, and sweet gift sets");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Brands",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"),
                column: "Description",
                value: "Personalized gifts and accessories");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("aaaa1111-1111-1111-1111-111111111111"),
                column: "Description",
                value: "Fresh flowers, dried flowers, bouquet arrangements");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("bbbb2222-2222-2222-2222-222222222222"),
                column: "Description",
                value: "Handmade gifts, souvenirs, and DIY sets");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("cccc3333-3333-3333-3333-333333333333"),
                column: "Description",
                value: "Cakes, chocolates, candy gift boxes");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("dddd4444-4444-4444-4444-444444444444"),
                column: "Description",
                value: "Jewelry, watches, and small fashion gifts");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Occasions",
                keyColumn: "Id",
                keyValue: new Guid("66666666-4444-4444-4444-444444444444"),
                column: "Description",
                value: "Special gifts to celebrate moms");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Occasions",
                keyColumn: "Id",
                keyValue: new Guid("77777777-3333-3333-3333-333333333333"),
                column: "Description",
                value: "Romantic gifts for Valentine's Day");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Occasions",
                keyColumn: "Id",
                keyValue: new Guid("88888888-2222-2222-2222-222222222222"),
                column: "Description",
                value: "Warm gifts for Christmas holidays");

            migrationBuilder.UpdateData(
                schema: "public",
                table: "Occasions",
                keyColumn: "Id",
                keyValue: new Guid("99999999-1111-1111-1111-111111111111"),
                column: "Description",
                value: "Perfect gifts for birthdays");
        }
    }
}
