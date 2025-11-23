using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace What2Gift.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTopUpCodeToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TopUpCode",
                schema: "public",
                table: "Users",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TopUpCode",
                schema: "public",
                table: "Users");
        }
    }
}
