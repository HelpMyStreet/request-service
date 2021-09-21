using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class RemoveSuppressQuestionForBoston : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 19, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 19, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 19, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 19, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 19, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 19, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 29, 19, 30 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                columns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID", "Location", "Order", "PlaceholderText", "RequestFormStageID", "Required", "Subtext" },
                values: new object[,]
                {
                    { 1, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 7, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 2, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 27, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 5, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 29, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 11, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." }
                });
        }
    }
}
