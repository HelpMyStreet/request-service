using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class RemoveFaceMask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 2, 1 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 2, 3 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 2, 5 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 2, 7 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 2, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 3, 1 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 3, 3 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 3, 5 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 3, 7 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 3, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 4, 1 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 4, 3 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 4, 5 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 4, 7 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 4, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 5, 1 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 5, 3 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 5, 5 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 5, 7 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 5, 32 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 7, 3 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 14, 1 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 14, 3 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 14, 5 });

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                keyColumns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID" },
                keyValues: new object[] { 12, 14, 7 });

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "ActivityQuestions",
                columns: new[] { "ActivityID", "QuestionID", "RequestFormVariantID", "Location", "Order", "PlaceholderText", "RequestFormStageID", "Required", "Subtext" },
                values: new object[,]
                {
                    { 12, 2, 1, "pos2", 2, "Don’t forget to tell us how many of each size you need. If you have very specific style requirements it may take longer to find a volunteer to help with your request. Please don’t include personal information such as name or address in this box, we’ll ask for that later.", 1, false, "Size guide:<br />&nbsp;- Men’s (Small / Medium / Large)<br />&nbsp;- Ladies’ (Small / Medium / Large)<br />&nbsp;- Children’s (One Size - under 12)" },
                    { 12, 5, 32, "pos3", 4, null, 1, false, "Volunteers are providing their time and skills free of charge." },
                    { 12, 4, 32, "pos3", 3, null, 1, false, null },
                    { 12, 3, 32, "pos3", 1, null, 1, true, "Remember they’re washable and reusable, so only request what you need between washes." },
                    { 12, 2, 32, "pos2", 2, "Don’t forget to tell us how many of each size you need. If you have very specific style requirements it may take longer to find a volunteer to help with your request. Please don’t include personal information such as name or address in this box, we’ll ask for that later.", 1, false, "Size guide:<br />&nbsp;- Men’s (Small / Medium / Large)<br />&nbsp;- Ladies’ (Small / Medium / Large)<br />&nbsp;- Children’s (One Size - under 12)" },
                    { 12, 14, 7, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 12, 5, 7, "pos3", 4, null, 1, false, "Volunteers are providing their time and skills free of charge." },
                    { 12, 4, 7, "pos3", 3, null, 1, false, null },
                    { 12, 3, 7, "pos3", 1, null, 1, true, "Remember they’re washable and reusable, so only request what you need between washes." },
                    { 12, 2, 7, "pos2", 2, "Don’t forget to tell us how many of each size you need. If you have very specific style requirements it may take longer to find a volunteer to help with your request. Please don’t include personal information such as name or address in this box, we’ll ask for that later.", 1, false, "Size guide:<br />&nbsp;- Men’s (Small / Medium / Large)<br />&nbsp;- Ladies’ (Small / Medium / Large)<br />&nbsp;- Children’s (One Size - under 12)" },
                    { 12, 14, 5, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 12, 5, 5, "pos3", 4, null, 1, false, "Volunteers are providing their time and skills free of charge." },
                    { 12, 21, 32, "details1", 2, null, 2, true, "Does this request require an administrator to approve before the volunteer can accept?" },
                    { 12, 4, 5, "pos3", 3, null, 1, false, null },
                    { 12, 2, 5, "pos2", 2, "Don’t forget to tell us how many of each size you need. If you have very specific style requirements it may take longer to find a volunteer to help with your request. Please don’t include personal information such as name or address in this box, we’ll ask for that later.", 1, false, "Size guide:<br />&nbsp;- Men’s (Small / Medium / Large)<br />&nbsp;- Ladies’ (Small / Medium / Large)<br />&nbsp;- Children’s (One Size - under 12)" },
                    { 12, 14, 3, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 12, 7, 3, "pos3", 3, null, 1, true, null },
                    { 12, 5, 3, "pos3", 4, null, 1, false, "Volunteers are providing their time and skills free of charge." },
                    { 12, 4, 3, "pos3", 3, null, 1, false, null },
                    { 12, 3, 3, "pos3", 1, null, 1, true, "Remember they’re washable and reusable, so only request what you need between washes." },
                    { 12, 2, 3, "pos2", 2, "Don’t forget to tell us how many of each size you need. If you have very specific style requirements it may take longer to find a volunteer to help with your request. Please don’t include personal information such as name or address in this box, we’ll ask for that later.", 1, false, "Size guide:<br />&nbsp;- Men’s (Small / Medium / Large)<br />&nbsp;- Ladies’ (Small / Medium / Large)<br />&nbsp;- Children’s (One Size - under 12)" },
                    { 12, 14, 1, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" },
                    { 12, 5, 1, "pos3", 4, null, 1, false, "Volunteers are providing their time and skills free of charge." },
                    { 12, 4, 1, "pos3", 3, null, 1, false, null },
                    { 12, 3, 1, "pos3", 1, null, 1, true, "Remember they’re washable and reusable, so only request what you need between washes." },
                    { 12, 3, 5, "pos3", 1, null, 1, true, "Remember they’re washable and reusable, so only request what you need between washes." },
                    { 12, 14, 32, "details2", 3, "For example, a door entry code, or contact details for a friend / relative / caregiver.", 2, false, "We will only share this information with a volunteer after they have accepted your request" }
                });
        }
    }
}
