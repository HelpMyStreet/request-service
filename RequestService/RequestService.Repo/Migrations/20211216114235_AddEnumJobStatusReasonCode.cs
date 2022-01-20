using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddEnumJobStatusReasonCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JobStatusChangeReasonCode",
                schema: "Lookup",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobStatusChangeReasonCode", x => x.ID);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "JobStatusChangeReasonCode",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 1, "AutoProgressingOverdueRepeats" },
                    { 2, "AutoProgressingJobsPastDueDates" },
                    { 3, "AutoProgressingShifts" },
                    { 4, "ManualChangeByVolunteer" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobStatusChangeReasonCode",
                schema: "Lookup");
        }
    }
}
