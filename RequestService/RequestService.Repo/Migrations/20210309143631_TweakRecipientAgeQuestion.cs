using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class TweakRecipientAgeQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 18,
                columns: new[] { "AdditionalDataSource", "AnswerContainsSensitiveData" },
                values: new object[] { null, true });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 18,
                columns: new[] { "AdditionalDataSource", "AnswerContainsSensitiveData" },
                values: new object[] { (byte)1, false });
        }
    }
}
