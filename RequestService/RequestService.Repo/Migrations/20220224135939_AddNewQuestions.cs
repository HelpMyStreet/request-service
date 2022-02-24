using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddNewQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "JobStatus",
                columns: new[] { "ID", "Name" },
                values: new object[] { 7, "AppliedFor" });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Question",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 20, "SelectActivity" },
                    { 21, "RequiresApplicationToAccept" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RequestFormVariant",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {                    
                    { 32, "LincolnshireVolunteersRequests_RequestSubmitter" }
                });

            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "Question",
                columns: new[] { "ID", "AdditionalData", "AdditionalDataSource", "AnswerContainsSensitiveData", "Name", "QuestionType" },
                values: new object[,]
                {
                    { 20, "", null, false, "Please select an activity", (byte)2 },
                    { 21, "", null, false, "Requires an administrator to approve volunteer's application to fulfill request", (byte)4 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "JobStatus",
                keyColumn: "ID",
                keyValue: 7);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "Question",
                keyColumn: "ID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "Question",
                keyColumn: "ID",
                keyValue: 21);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RequestFormVariant",
                keyColumn: "ID",
                keyValue: 32);

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 20);

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 21);
        }
    }
}
