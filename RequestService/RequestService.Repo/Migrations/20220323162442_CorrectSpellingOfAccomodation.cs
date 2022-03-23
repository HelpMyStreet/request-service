using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class CorrectSpellingOfAccomodation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 22,
                column: "Name",
                value: "How many adults need accommodation?");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 23,
                column: "Name",
                value: "How many children need accommodation?");

            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 24,
                column: "Name",
                value: "How many pets need accommodation?");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                column: "Name",
                value: "How many pets need accomodation?");
        }
    }
}
