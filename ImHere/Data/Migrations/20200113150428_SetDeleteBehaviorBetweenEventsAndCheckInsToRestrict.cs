using Microsoft.EntityFrameworkCore.Migrations;

namespace ImHere.Data.Migrations
{
    public partial class SetDeleteBehaviorBetweenEventsAndCheckInsToRestrict : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckIns_Events_EventId",
                table: "CheckIns");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckIns_Events_EventId",
                table: "CheckIns",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CheckIns_Events_EventId",
                table: "CheckIns");

            migrationBuilder.AddForeignKey(
                name: "FK_CheckIns_Events_EventId",
                table: "CheckIns",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
