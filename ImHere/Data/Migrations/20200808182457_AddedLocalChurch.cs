using Microsoft.EntityFrameworkCore.Migrations;

namespace ImHere.Data.Migrations
{
    public partial class AddedLocalChurch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocalChurch",
                table: "Students",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LocalChurch",
                table: "Students");
        }
    }
}
