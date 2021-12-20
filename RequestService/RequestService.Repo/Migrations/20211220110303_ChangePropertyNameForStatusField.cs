using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class ChangePropertyNameForStatusField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggersStatusChange",
                schema: "Lookup",
                table: "JobStatusChangeReasonCode");

            migrationBuilder.AddColumn<bool>(
                name: "TriggersStatusChangeEmail",
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                keyColumn: "ID",
                keyValue: 1,
                column: "TriggersStatusChangeEmail",
                value: true);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                keyColumn: "ID",
                keyValue: 2,
                column: "TriggersStatusChangeEmail",
                value: true);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                keyColumn: "ID",
                keyValue: 4,
                column: "TriggersStatusChangeEmail",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggersStatusChangeEmail",
                schema: "Lookup",
                table: "JobStatusChangeReasonCode");

            migrationBuilder.AddColumn<bool>(
                name: "TriggersStatusChange",
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                keyColumn: "ID",
                keyValue: 1,
                column: "TriggersStatusChange",
                value: true);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                keyColumn: "ID",
                keyValue: 2,
                column: "TriggersStatusChange",
                value: true);

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                keyColumn: "ID",
                keyValue: 4,
                column: "TriggersStatusChange",
                value: true);
        }
    }
}
