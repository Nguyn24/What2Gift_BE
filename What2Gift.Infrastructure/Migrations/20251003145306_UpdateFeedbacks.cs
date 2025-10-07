using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace What2Gift.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateFeedbacks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_GiftSuggestions_SuggestionId",
                schema: "public",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_SuggestionId",
                schema: "public",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "SuggestionId",
                schema: "public",
                table: "Feedbacks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SuggestionId",
                schema: "public",
                table: "Feedbacks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_SuggestionId",
                schema: "public",
                table: "Feedbacks",
                column: "SuggestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_GiftSuggestions_SuggestionId",
                schema: "public",
                table: "Feedbacks",
                column: "SuggestionId",
                principalSchema: "public",
                principalTable: "GiftSuggestions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
