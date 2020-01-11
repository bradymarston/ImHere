using Microsoft.EntityFrameworkCore.Migrations;

namespace ImHere.Data.Migrations
{
    public partial class AddStudentTypeToStudentEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentTypeId",
                table: "Students",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentTypeId",
                table: "Students",
                column: "StudentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_StudentTypes_StudentTypeId",
                table: "Students",
                column: "StudentTypeId",
                principalTable: "StudentTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_StudentTypes_StudentTypeId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_StudentTypeId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "StudentTypeId",
                table: "Students");
        }
    }
}
