using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImHere.Data.Migrations
{
    public partial class RenamedScheduleItemToScheduleInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_ScheduleItemBase_ScheduleId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "ScheduleItemBase");

            migrationBuilder.CreateTable(
                name: "EventScheduleInfoBase",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<DateTime>(nullable: false),
                    Duration = table.Column<TimeSpan>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: true),
                    Day = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventScheduleInfoBase", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventScheduleInfoBase_ScheduleId",
                table: "Events",
                column: "ScheduleId",
                principalTable: "EventScheduleInfoBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventScheduleInfoBase_ScheduleId",
                table: "Events");

            migrationBuilder.DropTable(
                name: "EventScheduleInfoBase");

            migrationBuilder.CreateTable(
                name: "ScheduleItemBase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<TimeSpan>(type: "time", nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Day = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleItemBase", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Events_ScheduleItemBase_ScheduleId",
                table: "Events",
                column: "ScheduleId",
                principalTable: "ScheduleItemBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
