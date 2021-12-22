using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddPropertyToJobStatusChangeReasonCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "TriggersStatusChange",
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
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
                columns: new[] { "Name", "TriggersStatusChange" },
                values: new object[] { "UserChange", true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TriggersStatusChange",
                schema: "Lookup",
                table: "JobStatusChangeReasonCode");

            migrationBuilder.UpdateData(
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                keyColumn: "ID",
                keyValue: 4,
                column: "Name",
                value: "ManualChangeByVolunteer");
        }
    }
}
