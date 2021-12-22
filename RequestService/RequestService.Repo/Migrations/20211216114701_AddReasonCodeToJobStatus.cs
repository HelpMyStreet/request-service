using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddReasonCodeToJobStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "JobStatusChangeReasonCodeID",
                schema: "Request",
                table: "RequestJobStatus",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobStatusChangeReasonCodeID",
                schema: "Request",
                table: "RequestJobStatus");
        }
    }
}
