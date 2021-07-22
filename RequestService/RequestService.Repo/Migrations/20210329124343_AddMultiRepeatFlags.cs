using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddMultiRepeatFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Multi",
                schema: "Request",
                table: "Job",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Repeat",
                schema: "Request",
                table: "Job",
                nullable: true);

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "DueDateType",
                columns: new[] { "ID", "Name" },
                values: new object[] { 5, "ASAP" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "DueDateType",
                keyColumn: "ID",
                keyValue: 5);

            migrationBuilder.DropColumn(
                name: "Multi",
                schema: "Request",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "Repeat",
                schema: "Request",
                table: "Job");
        }
    }
}
