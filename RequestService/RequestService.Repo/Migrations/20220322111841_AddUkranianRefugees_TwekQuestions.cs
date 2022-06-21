using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddUkranianRefugees_TwekQuestions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 22,
                column: "Name",
                value: "How many adults need accomodation?");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 23,
                column: "Name",
                value: "How many children need accomodation?");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 24,
                columns: new[] { "Name", "QuestionType" },
                values: new object[] { "How many pets need accomodation?", (byte)1 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 25,
                columns: new[] { "AdditionalData", "QuestionType" },
                values: new object[] { "", (byte)2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 22,
                column: "Name",
                value: "How many adults need accomodation");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 23,
                column: "Name",
                value: "How many children need accomodation");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 24,
                columns: new[] { "Name", "QuestionType" },
                values: new object[] { "How many pets need accomodation", (byte)2 });

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 25,
                columns: new[] { "AdditionalData", "QuestionType" },
                values: new object[] { "[{\"Key\":\"English\",\"Value\":\"English\"},{\"Key\":\"Ukrainian\",\"Value\":\"Ukrainian\"},{\"Key\":\"Other\",\"Value\":\"Other\"}]", (byte)4 });
        }
    }
}
