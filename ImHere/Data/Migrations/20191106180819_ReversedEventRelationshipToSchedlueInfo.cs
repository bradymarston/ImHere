using Microsoft.EntityFrameworkCore.Migrations;

namespace ImHere.Data.Migrations
{
    public partial class ReversedEventRelationshipToSchedlueInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventScheduleInfoBase_ScheduleId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_ScheduleId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "Events");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "EventScheduleInfoBase",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_EventScheduleInfoBase_EventId",
                table: "EventScheduleInfoBase",
                column: "EventId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EventScheduleInfoBase_Events_EventId",
                table: "EventScheduleInfoBase",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventScheduleInfoBase_Events_EventId",
                table: "EventScheduleInfoBase");

            migrationBuilder.DropIndex(
                name: "IX_EventScheduleInfoBase_EventId",
                table: "EventScheduleInfoBase");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "EventScheduleInfoBase");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "Events",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ScheduleId",
                table: "Events",
                column: "ScheduleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventScheduleInfoBase_ScheduleId",
                table: "Events",
                column: "ScheduleId",
                principalTable: "EventScheduleInfoBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
