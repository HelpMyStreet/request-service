using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class ChangeRecipientAgeQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 18, 21 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 18, 21 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 18, 21 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 18, 21 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 18, 21 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 18, 21 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "details1", 2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 18,
                column: "QuestionType",
                value: (byte)2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 18, 21 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 18, 21 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 18, 21 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 18, 21 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 18, 21 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 18, 20 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 18, 21 },
                columns: new[] { "Location", "RequestFormStageID" },
                values: new object[] { "pos1", 1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 18,
                column: "QuestionType",
                value: (byte)1);
        }
    }
}
