using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class CardiffNewForms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                columns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID", "Location", "Order", "PlaceholderText", "RequestFormStageID", "Required", "Subtext" },
                values: new object[,]
                {
                    { 1, 12, 20, "pos1", 1, "For example, Hovis wholemeal bread, 2 pints semi-skimmed milk, 6 large eggs.", 1, true, "Make sure to include the size, brand, and any other important details" },
                    { 1, 12, 21, "pos1", 1, "For example, Hovis wholemeal bread, 2 pints semi-skimmed milk, 6 large eggs.", 1, true, "Make sure to include the size, brand, and any other important details" },
                    { 1, 10, 21, "details2", 2, "For example, any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 1, 18, 21, "pos1", 2, null, 1, false, null },
                    { 1, 14, 21, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 7, 1, 21, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 7, 10, 21, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 7, 18, 21, "pos1", 2, null, 1, false, null },
                    { 7, 14, 21, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 26, 1, 21, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 26, 10, 21, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 11, 14, 20, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 26, 18, 21, "pos1", 2, null, 1, false, null },
                    { 2, 13, 21, "pos1", 1, "Please give the name and address of the pharmacy, e.g. Boots Pharmacy, Victoria Centre, Nottingham.", 1, true, null },
                    { 2, 10, 21, "details2", 2, "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 2, 18, 21, "pos1", 2, null, 1, false, null },
                    { 2, 14, 21, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 27, 1, 21, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 27, 10, 21, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 27, 18, 21, "pos1", 2, null, 1, false, null },
                    { 27, 14, 21, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 11, 1, 21, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 11, 10, 21, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 26, 14, 21, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 11, 18, 20, "pos1", 2, null, 1, false, null },
                    { 11, 10, 20, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 11, 1, 20, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 1, 10, 20, "details2", 2, "For example, any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 1, 18, 20, "pos1", 2, null, 1, false, null },
                    { 1, 14, 20, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 7, 1, 20, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 7, 10, 20, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 7, 18, 20, "pos1", 2, null, 1, false, null },
                    { 7, 14, 20, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 26, 1, 20, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 26, 10, 20, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 26, 18, 20, "pos1", 2, null, 1, false, null },
                    { 26, 14, 20, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 2, 13, 20, "pos1", 1, "Please give the name and address of the pharmacy, e.g. Boots Pharmacy, Victoria Centre, Nottingham.", 1, true, null },
                    { 2, 10, 20, "details2", 2, "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 2, 18, 20, "pos1", 2, null, 1, false, null },
                    { 2, 14, 20, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 27, 1, 20, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 27, 10, 20, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 27, 18, 20, "pos1", 2, null, 1, false, null },
                    { 27, 14, 20, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 22, 1, 20, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 22, 10, 20, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 22, 18, 20, "pos1", 2, null, 1, false, null },
                    { 22, 14, 20, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 11, 18, 21, "pos1", 2, null, 1, false, null },
                    { 11, 14, 21, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 10, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 10, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 12, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 12, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 14, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 14, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 18, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 18, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 13, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 13, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 14, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 14, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 18, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 18, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 1, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 1, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 10, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 10, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 14, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 14, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 18, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 18, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 1, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 1, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 10, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 10, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 14, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 14, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 18, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 18, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 1, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 10, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 14, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 22, 18, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 1, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 1, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 10, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 10, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 14, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 14, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 18, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 26, 18, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 1, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 1, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 10, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 10, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 14, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 14, 21 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 18, 20 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 18, 21 });
        }
    }
}
