using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddDietaryQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Question",
                columns: new[] { "ID", "Name" },
                values: new object[] { 15, "SpecialDietaryRequirements" });

            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "Question",
                columns: new[] { "ID", "AdditionalData", "AnswerContainsSensitiveData", "Name", "QuestionType" },
                values: new object[] { 15, "", false, "Are there any special dietary requirements?", (byte)3 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "Question",
                keyColumn: "ID",
                keyValue: 15);

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 15);
        }
    }
}
