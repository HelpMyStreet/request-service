using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddLincolnshireVolunteersRequests_RequestSubmitter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                columns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID", "Location", "Order", "PlaceholderText", "RequestFormStageID", "Required", "Subtext" },
                values: new object[,]
                {
                    { 6, 10, 32, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 6, 21, 32, "details1", 2, null, 2, true, "Does this request require an administrator to volunteer" },
                    { 6, 14, 32, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 11, 20, 32, "pos1", 2, "Please provide details of the help that you require", 1, false, "Provide details of the activity that you need help with. If you need help with more than activity you will need to submit a new request for each." },
                    { 11, 1, 32, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 11, 10, 32, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 11, 21, 32, "details1", 2, null, 2, true, "Does this request require an administrator to volunteer" },
                    { 11, 14, 32, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 6, 1, 32, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 7, 21, 32, "details1", 2, null, 2, true, "Does this request require an administrator to volunteer" },
                    { 7, 14, 32, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 1, 12, 32, "pos1", 1, "For example, Hovis wholemeal bread, 2 pints semi-skimmed milk, 6 large eggs.", 1, true, "Make sure to include the size, brand, and any other important details" },
                    { 1, 10, 32, "details2", 2, "For example, any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 1, 21, 32, "details1", 2, null, 2, true, "Does this request require an administrator to volunteer" },
                    { 1, 14, 32, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 7, 1, 32, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 7, 10, 32, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 9, 14, 32, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 9, 10, 32, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 10, 10, 32, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 10, 21, 32, "details1", 2, null, 2, true, "Does this request require an administrator to volunteer" },
                    { 10, 14, 32, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 2, 13, 32, "pos1", 1, "Please give the name and address of the pharmacy, e.g. Boots Pharmacy, Victoria Centre, Nottingham.", 1, true, null },
                    { 2, 10, 32, "details2", 2, "For example, let us know if the prescription needs to be paid for or if it won’t be ready straight away. You should also let us know if there are any mobility or communication needs, or special instructions for the volunteer. Please don’t include any personal or sensitive information in this box.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 2, 21, 32, "details1", 2, null, 2, true, "Does this request require an administrator to volunteer" },
                    { 2, 14, 32, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 3, 1, 32, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 9, 21, 32, "details1", 2, null, 2, true, "Does this request require an administrator to volunteer" },
                    { 3, 10, 32, "details2", 2, "For example, any special instructions for the volunteer.", 2, false, "This information will be visible to volunteers deciding whether to accept the request" },
                    { 3, 14, 32, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 12, 2, 32, "pos2", 2, "Don’t forget to tell us how many of each size you need. If you have very specific style requirements it may take longer to find a volunteer to help with your request. Please don’t include personal information such as name or address in this box, we’ll ask for that later.", 1, false, "Size guide:<br />&nbsp;- Men’s (Small / Medium / Large)<br />&nbsp;- Ladies’ (Small / Medium / Large)<br />&nbsp;- Children’s (One Size - under 12)" },
                    { 12, 3, 32, "pos3", 1, null, 1, true, "Remember they’re washable and reusable, so only request what you need between washes." },
                    { 12, 4, 32, "pos3", 3, null, 1, false, null },
                    { 12, 5, 32, "pos3", 4, null, 1, false, "Volunteers are providing their time and skills free of charge." },
                    { 12, 21, 32, "details1", 2, null, 2, true, "Does this request require an administrator to volunteer" },
                    { 12, 14, 32, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 9, 1, 32, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null },
                    { 3, 21, 32, "details1", 2, null, 2, true, "Does this request require an administrator to volunteer" },
                    { 10, 1, 32, "pos1", 1, "Please don’t include any sensitive details that aren’t needed in order for us to help you", 1, false, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 10, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 12, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 14, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 1, 21, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 10, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 13, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 14, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 2, 21, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 3, 1, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 3, 10, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 3, 14, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 3, 21, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 6, 1, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 6, 10, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 6, 14, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 6, 21, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 1, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 10, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 14, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 7, 21, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 9, 1, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 9, 10, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 9, 14, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 9, 21, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 1, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 10, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 14, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 10, 21, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 1, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 10, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 14, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 20, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 11, 21, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 2, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 3, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 4, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 5, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 14, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 21, 32 });
        }
    }
}
