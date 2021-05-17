using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddSubTextToAgeQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 18, 20 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 18, 21 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 18, 20 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 18, 21 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 18, 20 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 18, 21 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 18, 20 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 18, 21 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 18, 20 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 18, 20 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 18, 21 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 18, 20 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 18, 21 },
                column: "Subtext",
                value: "We use age to check which services we are able to provide. You can put an approximate age if you prefer.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 18, 20 },
                column: "Subtext",
                value: null);

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 18, 21 },
                column: "Subtext",
                value: null);

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 18, 20 },
                column: "Subtext",
                value: null);

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 18, 21 },
                column: "Subtext",
                value: null);

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 18, 20 },
                column: "Subtext",
                value: null);

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 18, 21 },
                column: "Subtext",
                value: null);

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 18, 20 },
                column: "Subtext",
                value: null);

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 18, 21 },
                column: "Subtext",
                value: null);

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 18, 20 },
                column: "Subtext",
                value: null);

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 18, 20 },
                column: "Subtext",
                value: null);

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 18, 21 },
                column: "Subtext",
                value: null);

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 18, 20 },
                column: "Subtext",
                value: null);

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 18, 21 },
                column: "Subtext",
                value: null);
        }
    }
}
