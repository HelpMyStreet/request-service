using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddSelectActivityForOtherQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                columns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID", "Location", "Order", "PlaceholderText", "RequestFormStageID", "Required", "Subtext" },
                values: new object[,]
                {
                    { 11, 20, 30, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 29, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 28, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 26, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 25, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 23, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 22, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 21, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 20, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 19, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 15, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 14, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 13, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 12, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 11, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 10, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 9, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 8, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 7, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 5, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 3, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 2, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 16, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 20, 1, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 1 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 2 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 3 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 5 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 7 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 8 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 9 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 12 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 13 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 14 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 15 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 16 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 19 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 22 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 23 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 25 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 26 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 28 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 30 });
        }
    }
}
