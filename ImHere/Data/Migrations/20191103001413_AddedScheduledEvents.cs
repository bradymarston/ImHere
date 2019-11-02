using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ImHere.Data.Migrations
{
    public partial class AddedScheduledEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScheduledEvents",
                columns: table => new
                {
                    ScheduledEventId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    EndTime = table.Column<DateTime>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    IsWeekly = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduledEvents", x => x.ScheduledEventId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScheduledEvents");
        }
    }
}
