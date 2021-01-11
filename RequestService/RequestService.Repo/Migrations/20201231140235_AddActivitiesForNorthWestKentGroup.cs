using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddActivitiesForNorthWestKentGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 6, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 6, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 10, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 10, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 12, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 12, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 14, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 14, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 6, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 6, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 3, 1, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 3, 1, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 3, 6, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 3, 6, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 3, 10, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 3, 10, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 3, 14, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 3, 14, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 4, 1, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 4, 1, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 4, 6, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 4, 6, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 4, 10, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 4, 10, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 4, 14, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 4, 14, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 1, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 1, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 6, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 6, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 10, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 10, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 14, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 14, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 6, 1, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 6, 1, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 6, 6, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 6, 6, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 6, 10, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 6, 10, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 6, 14, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 6, 14, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 6, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 6, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 8, 1, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 8, 1, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 8, 6, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 8, 6, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 8, 10, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 8, 10, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 8, 14, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 8, 14, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 9, 1, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 9, 1, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 9, 6, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 9, 6, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 9, 10, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 9, 10, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 9, 14, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 9, 14, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 1, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 1, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 6, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 6, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 10, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 10, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 14, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 14, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 6, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 6, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 2, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 2, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 3, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 3, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 4, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 4, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 5, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 5, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 14, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 14, 11 });

            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                columns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID", "Location", "Order", "PlaceholderText", "RequestFormStageID", "Required", "Subtext" },
                values: new object[,]
                {
                    { 21, 1, 10, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 21, 10, 10, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 21, 14, 10, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 16, 1, 10, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 16, 10, 10, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 16, 14, 10, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 21, 1, 11, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 21, 10, 11, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 21, 14, 11, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 16, 1, 11, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 16, 10, 11, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 16, 14, 11, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 22, 1, 11, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 22, 10, 11, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 22, 14, 11, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 16, 1, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 16, 1, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 16, 10, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 16, 10, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 16, 14, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 16, 14, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 21, 1, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 21, 1, 11 });

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
                keyValues: new object[] { 21, 14, 10 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 21, 14, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 1, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 11 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 14, 11 });

            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                columns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID", "Location", "Order", "PlaceholderText", "RequestFormStageID", "Required", "Subtext" },
                values: new object[,]
                {
                    { 1, 12, 10, "pos1", 1, "For example, Hovis wholemeal bread, 2 pints semi-skimmed milk, 6 large eggs.", 1, true, "Make sure to include the size, brand, and any other important details" },
                    { 6, 1, 11, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 5, 14, 11, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 5, 6, 11, "pos3", 2, null, 1, true, null },
                    { 5, 10, 11, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 5, 1, 11, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 4, 14, 11, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 4, 6, 11, "pos3", 2, null, 1, true, null },
                    { 6, 10, 11, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 4, 10, 11, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 3, 14, 11, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 3, 6, 11, "pos3", 2, null, 1, true, null },
                    { 3, 10, 11, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 3, 1, 11, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 2, 6, 11, "pos3", 2, null, 1, true, null },
                    { 1, 14, 11, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 1, 6, 11, "pos3", 2, null, 1, true, null },
                    { 4, 1, 11, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 1, 10, 11, "details2", 2, "For example, any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 6, 6, 11, "pos3", 2, null, 1, true, null },
                    { 7, 6, 11, "pos3", 2, null, 1, true, null },
                    { 12, 4, 11, "pos3", 3, null, 1, false, null },
                    { 12, 3, 11, "pos3", 1, null, 1, true, "Remember they’re washable and reusable, so only request what you need between washes." },
                    { 12, 2, 11, "pos2", 2, "Don’t forget to tell us how many of each size you need. If you have very specific style requirements it may take longer to find a volunteer to help with your request. Please don’t include personal information such as name or address in this box, we’ll ask for that later.", 1, false, "Size guide:<br />&nbsp;- Men’s (Small / Medium / Large)<br />&nbsp;- Ladies’ (Small / Medium / Large)<br />&nbsp;- Children’s (One Size - under 12)" },
                    { 11, 6, 11, "pos3", 2, null, 1, true, null },
                    { 10, 14, 11, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 10, 6, 11, "pos3", 2, null, 1, true, null },
                    { 10, 10, 11, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 6, 14, 11, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 10, 1, 11, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 9, 6, 11, "pos3", 2, null, 1, true, null },
                    { 9, 10, 11, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 9, 1, 11, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 8, 14, 11, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 8, 6, 11, "pos3", 2, null, 1, true, null },
                    { 8, 10, 11, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 8, 1, 11, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 9, 14, 11, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 1, 12, 11, "pos1", 1, "For example, Hovis wholemeal bread, 2 pints semi-skimmed milk, 6 large eggs.", 1, true, "Make sure to include the size, brand, and any other important details" },
                    { 12, 14, 10, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 12, 5, 10, "pos3", 4, null, 1, false, "Volunteers are providing their time and skills free of charge." },
                    { 5, 14, 10, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 5, 6, 10, "pos3", 2, null, 1, true, null },
                    { 5, 10, 10, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 5, 1, 10, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 4, 14, 10, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 4, 6, 10, "pos3", 2, null, 1, true, null },
                    { 4, 10, 10, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 6, 1, 10, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 4, 1, 10, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 3, 6, 10, "pos3", 2, null, 1, true, null },
                    { 3, 10, 10, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 3, 1, 10, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 2, 6, 10, "pos3", 2, null, 1, true, null },
                    { 1, 14, 10, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 1, 6, 10, "pos3", 2, null, 1, true, null },
                    { 1, 10, 10, "details2", 2, "For example, any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 3, 14, 10, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 6, 10, 10, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 6, 6, 10, "pos3", 2, null, 1, true, null },
                    { 6, 14, 10, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 12, 4, 10, "pos3", 3, null, 1, false, null },
                    { 12, 3, 10, "pos3", 1, null, 1, true, "Remember they’re washable and reusable, so only request what you need between washes." },
                    { 12, 2, 10, "pos2", 2, "Don’t forget to tell us how many of each size you need. If you have very specific style requirements it may take longer to find a volunteer to help with your request. Please don’t include personal information such as name or address in this box, we’ll ask for that later.", 1, false, "Size guide:<br />&nbsp;- Men’s (Small / Medium / Large)<br />&nbsp;- Ladies’ (Small / Medium / Large)<br />&nbsp;- Children’s (One Size - under 12)" },
                    { 11, 6, 10, "pos3", 2, null, 1, true, null },
                    { 10, 14, 10, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 10, 6, 10, "pos3", 2, null, 1, true, null },
                    { 10, 10, 10, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 10, 1, 10, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 9, 14, 10, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 9, 6, 10, "pos3", 2, null, 1, true, null },
                    { 9, 10, 10, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 9, 1, 10, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 8, 14, 10, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 8, 6, 10, "pos3", 2, null, 1, true, null },
                    { 8, 10, 10, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 8, 1, 10, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 7, 6, 10, "pos3", 2, null, 1, true, null },
                    { 12, 5, 11, "pos3", 4, null, 1, false, "Volunteers are providing their time and skills free of charge." },
                    { 12, 14, 11, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" }
                });
        }
    }
}
