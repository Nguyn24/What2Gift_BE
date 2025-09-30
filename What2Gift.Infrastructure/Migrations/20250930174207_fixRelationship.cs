using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace What2Gift.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Memberships_UserId",
                schema: "public",
                table: "Memberships");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId",
                schema: "public",
                table: "Memberships",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Memberships_UserId",
                schema: "public",
                table: "Memberships");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_UserId",
                schema: "public",
                table: "Memberships",
                column: "UserId");
        }
    }
}
