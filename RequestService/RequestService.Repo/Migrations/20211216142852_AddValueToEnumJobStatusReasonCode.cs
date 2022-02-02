using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddValueToEnumJobStatusReasonCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                columns: new[] { "ID", "Name" },
                values: new object[] { 5, "AutoProgressNewToOpen" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                keyColumn: "ID",
                keyValue: 5);
        }
    }
}
