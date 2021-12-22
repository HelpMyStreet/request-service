using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class TweakPropertyNameForStatusField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                keyColumn: "ID",
                keyValue: 1,
                column: "TriggersStatusChangeEmail",
                value: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                keyColumn: "ID",
                keyValue: 1,
                column: "TriggersStatusChangeEmail",
                value: true);
        }
    }
}
