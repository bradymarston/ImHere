using Microsoft.EntityFrameworkCore.Migrations;

namespace ImHere.Data.Migrations
{
    public partial class SeedStudentTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData("StudentTypes", "Description", new string[] { "Student", "NWOSU Faculty/Staff", "Wesley Staff/Volunteer", "Guest" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData("StudentTypes", "Description", new string[] { "Student", "NWOSU Faculty/Staff", "Wesley Staff/Volunteer", "Guest" });
        }
    }
}
