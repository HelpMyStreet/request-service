using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class MoveMultiRepeatFlagsToRequest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Multi",
                schema: "Request",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "Repeat",
                schema: "Request",
                table: "Job");

            migrationBuilder.AddColumn<bool>(
                name: "MultiVolunteer",
                schema: "Request",
                table: "Request",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Repeat",
                schema: "Request",
                table: "Request",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MultiVolunteer",
                schema: "Request",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "Repeat",
                schema: "Request",
                table: "Request");

            migrationBuilder.AddColumn<bool>(
                name: "Multi",
                schema: "Request",
                table: "Job",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Repeat",
                schema: "Request",
                table: "Job",
                type: "bit",
                nullable: true);
        }
    }
}
