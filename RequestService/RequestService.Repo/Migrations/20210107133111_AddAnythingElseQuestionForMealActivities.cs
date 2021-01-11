using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddAnythingElseQuestionForMealActivities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                columns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID", "Location", "Order", "PlaceholderText", "RequestFormStageID", "Required", "Subtext" },
                values: new object[,]
                {
                    { 21, 10, 10, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 21, 10, 11, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 23, 10, 13, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 21, 10, 13, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 23, 10, 14, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 21, 10, 14, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 23, 10, 15, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 21, 10, 15, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 23, 10, 16, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 21, 10, 16, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 21, 10, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 21, 10, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 21, 10, 13 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 21, 10, 14 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 21, 10, 15 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 21, 10, 16 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 23, 10, 13 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 23, 10, 14 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 23, 10, 15 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 23, 10, 16 });
        }
    }
}
