using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class TweakHelpTextForVolunteerSupportAndEmergencySupport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 2 },
                column: "PlaceholderText",
                value: "‘For example, any special instructions for the volunteer such as, what time they need to arrive or if there is any specific they need to bring with them.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 7 },
                column: "PlaceholderText",
                value: "‘For example, any special instructions for the volunteer such as, what time they need to arrive or if there is any specific they need to bring with them.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 8 },
                column: "PlaceholderText",
                value: "‘For example, any special instructions for the volunteer such as, what time they need to arrive or if there is any specific they need to bring with them.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 11 },
                column: "PlaceholderText",
                value: "‘For example, any special instructions for the volunteer such as, what time they need to arrive or if there is any specific they need to bring with them.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 14 },
                column: "PlaceholderText",
                value: "‘For example, any special instructions for the volunteer such as, what time they need to arrive or if there is any specific they need to bring with them.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 16 },
                column: "PlaceholderText",
                value: "‘For example, any special instructions for the volunteer such as, what time they need to arrive or if there is any specific they need to bring with them.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 17 },
                column: "PlaceholderText",
                value: "‘For example, any special instructions for the volunteer such as, what time they need to arrive or if there is any specific they need to bring with them.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 19 },
                column: "PlaceholderText",
                value: "‘For example, any special instructions for the volunteer such as, what time they need to arrive or if there is any specific they need to bring with them.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 25, 10, 17 },
                column: "PlaceholderText",
                value: "‘For example, any special instructions for the volunteer such as, what time they need to arrive or if there is any specific they need to bring with them.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 25, 10, 19 },
                column: "PlaceholderText",
                value: "‘For example, any special instructions for the volunteer such as, what time they need to arrive or if there is any specific they need to bring with them.");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 2 },
                column: "PlaceholderText",
                value: "For example, any special instructions for the volunteer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 7 },
                column: "PlaceholderText",
                value: "For example, any special instructions for the volunteer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 8 },
                column: "PlaceholderText",
                value: "For example, any special instructions for the volunteer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 11 },
                column: "PlaceholderText",
                value: "For example, any special instructions for the volunteer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 14 },
                column: "PlaceholderText",
                value: "For example, any special instructions for the volunteer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 16 },
                column: "PlaceholderText",
                value: "For example, any special instructions for the volunteer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 17 },
                column: "PlaceholderText",
                value: "For example, any special instructions for the volunteer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 19 },
                column: "PlaceholderText",
                value: "For example, any special instructions for the volunteer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 25, 10, 17 },
                column: "PlaceholderText",
                value: "For example, any special instructions for the volunteer.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 25, 10, 19 },
                column: "PlaceholderText",
                value: "For example, any special instructions for the volunteer.");
        }
    }
}
