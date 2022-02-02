using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddCascadeDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddForeignKey(
                name: "FK_NewRequest_NewRequestID",
                schema: "Request",
                table: "Job",
                column: "RequestId",
                principalSchema: "Request",
                principalTable: "Request",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_JobAvailableToGroup_JobID",
                schema: "Request",
                table: "JobAvailableToGroup",
                column: "JobID",
                principalSchema: "Request",
                principalTable: "Job",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_JobID",
                schema: "Request",
                table: "RequestJobStatus",
                column: "JobID",
                principalSchema: "Request",
                principalTable: "Job",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
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
