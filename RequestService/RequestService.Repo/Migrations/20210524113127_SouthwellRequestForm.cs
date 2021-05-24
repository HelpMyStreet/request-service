using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class SouthwellRequestForm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RequestFormVariant",
                columns: new[] { "ID", "Name" },
                values: new object[] { 25, "Soutwell_Public" });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 1 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 2 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 3 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 5 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 7 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 8 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 9 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 10 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 11 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 12 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 13 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 14 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 19 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 20 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 21 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 22 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 23 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                columns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID", "Location", "Order", "PlaceholderText", "RequestFormStageID", "Required", "Subtext" },
                values: new object[,]
                {
                    { 11, 6, 25, "pos3", 2, null, 1, true, null },
                    { 11, 10, 25, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 11, 1, 25, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 2, 14, 25, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 2, 6, 25, "pos3", 2, null, 1, true, null },
                    { 2, 10, 25, "details2", 2, "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 2, 13, 25, "pos1", 1, "Please give the name and address of the pharmacy, e.g. Boots Pharmacy, Victoria Centre, Nottingham.", 1, true, null },
                    { 1, 14, 25, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 1, 6, 25, "pos3", 2, null, 1, true, null },
                    { 1, 10, 25, "details2", 2, "For example, any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 11, 14, 25, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 1, 12, 25, "pos1", 1, "For example, Hovis wholemeal bread, 2 pints semi-skimmed milk, 6 large eggs.", 1, true, "Make sure to include the size, brand, and any other important details" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RequestFormVariant",
                keyColumn: "ID",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 6, 25 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 10, 25 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 12, 25 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 14, 25 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 6, 25 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 25 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 13, 25 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 14, 25 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 1, 25 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 6, 25 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 10, 25 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 14, 25 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 1 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 2 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 3 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 5 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 7 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 8 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 9 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 10 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 11 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 12 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 13 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 14 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 19 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 20 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 21 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 22 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 23 },
                column: "PlaceholderText",
                value: "For example, let us know if the prescription needs to be paid for, or if there are any mobility or communication needs or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.");
        }
    }
}
