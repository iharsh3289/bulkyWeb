using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bulky_Web.Migrations
{
    /// <inheritdoc />
    public partial class ggg : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyyId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompanyyId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }
    }
}
