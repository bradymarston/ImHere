using Microsoft.EntityFrameworkCore.Migrations;

namespace ImHere.Data.Migrations
{
    public partial class AddedRequireConfirmationToEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "RequireConfirmation",
                table: "Events",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequireConfirmation",
                table: "Events");
        }
    }
}
