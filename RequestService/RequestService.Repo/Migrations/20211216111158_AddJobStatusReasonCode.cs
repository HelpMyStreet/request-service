using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddJobStatusReasonCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "JobStatusChangeReasonCodeId",
                schema: "Request",
                table: "RequestJobStatus",
                nullable: true);

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
            migrationBuilder.DropForeignKey(
                name: "FK_NewRequest_NewRequestID",
                schema: "Request",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_JobAvailableToGroup_JobID",
                schema: "Request",
                table: "JobAvailableToGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Job_JobID",
                schema: "Request",
                table: "RequestJobStatus");

            migrationBuilder.DropTable(
                name: "JobStatusChangeReasonCode",
                schema: "Lookup");

            migrationBuilder.DropColumn(
                name: "JobStatusChangeReasonCodeId",
                schema: "Request",
                table: "RequestJobStatus");

            migrationBuilder.AddForeignKey(
                name: "FK_NewRequest_NewRequestID",
                schema: "Request",
                table: "Job",
                column: "RequestId",
                principalSchema: "Request",
                principalTable: "Request",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_JobAvailableToGroup_JobID",
                schema: "Request",
                table: "JobAvailableToGroup",
                column: "JobID",
                principalSchema: "Request",
                principalTable: "Job",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_JobID",
                schema: "Request",
                table: "RequestJobStatus",
                column: "JobID",
                principalSchema: "Request",
                principalTable: "Job",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
