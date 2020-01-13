using Microsoft.EntityFrameworkCore.Migrations;

namespace ImHere.Data.Migrations
{
    public partial class AddedEventSuspended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Suspended",
                table: "Events",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Suspended",
                table: "Events");
        }
    }
}
