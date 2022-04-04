using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddUkranianRefees_ShowPersonalDetailsQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                columns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID", "Location", "Order", "PlaceholderText", "RequestFormStageID", "Required", "Subtext" },
                values: new object[,]
                {
                    { 34, 19, 33, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 1, 19, 33, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 7, 19, 33, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 10, 19, 33, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 11, 19, 33, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 19, 33 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 19, 33 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 19, 33 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 19, 33 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 34, 19, 33 });
        }
    }
}
