using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddBostonGNS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RequestFormVariant",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 30, "BostonGNS_RequestSubmitter" },
                    { 29, "BostonGNS_Public" }
                });

            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                columns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID", "Location", "Order", "PlaceholderText", "RequestFormStageID", "Required", "Subtext" },
                values: new object[,]
                {
                    { 11, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 7, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 7, 14, 30, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 2, 13, 30, "pos1", 1, "Please give the name and address of the pharmacy, e.g. Boots Pharmacy, Victoria Centre, Nottingham.", 1, true, null },
                    { 2, 10, 30, "details2", 2, "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 2, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 2, 14, 30, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 27, 1, 30, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 27, 10, 30, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 27, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 11, 14, 30, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 27, 14, 30, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 5, 10, 30, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 5, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 5, 14, 30, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 29, 1, 30, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 29, 10, 30, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 29, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 29, 14, 30, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 11, 1, 30, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 11, 10, 30, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 7, 10, 30, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 5, 1, 30, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 7, 1, 30, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 1, 19, 30, "details1", 1, null, 2, true, "If yes, volunteer(s) who accept this request will be asked to contact the requester to get the information they need to complete the request." },
                    { 1, 10, 29, "details2", 2, "For example, any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 1, 14, 29, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 7, 1, 29, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 7, 10, 29, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 7, 14, 29, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 2, 13, 29, "pos1", 1, "Please give the name and address of the pharmacy, e.g. Boots Pharmacy, Victoria Centre, Nottingham.", 1, true, null },
                    { 2, 10, 29, "details2", 2, "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 2, 14, 29, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 27, 1, 29, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 27, 10, 29, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 1, 14, 30, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 27, 14, 29, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 5, 10, 29, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 5, 14, 29, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 29, 1, 29, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 29, 10, 29, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 29, 14, 29, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 11, 1, 29, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 11, 10, 29, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 11, 14, 29, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 1, 12, 30, "pos1", 1, "For example, Hovis wholemeal bread, 2 pints semi-skimmed milk, 6 large eggs.", 1, true, "Make sure to include the size, brand, and any other important details" },
                    { 1, 10, 30, "details2", 2, "For example, any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 5, 1, 29, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 1, 12, 29, "pos1", 1, "For example, Hovis wholemeal bread, 2 pints semi-skimmed milk, 6 large eggs.", 1, true, "Make sure to include the size, brand, and any other important details" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RequestFormVariant",
                keyColumn: "ID",
                keyValue: 29);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RequestFormVariant",
                keyColumn: "ID",
                keyValue: 30);

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 10, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 10, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 12, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 12, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 14, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 14, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 19, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 13, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 13, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 14, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 14, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 19, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 1, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 1, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 10, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 10, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 14, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 14, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 5, 19, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 1, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 1, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 10, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 10, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 14, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 14, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 19, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 1, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 1, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 10, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 10, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 14, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 14, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 19, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 1, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 1, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 10, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 10, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 14, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 14, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 27, 19, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 29, 1, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 29, 1, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 29, 10, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 29, 10, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 29, 14, 29 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 29, 14, 30 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 29, 19, 30 });
        }
    }
}
