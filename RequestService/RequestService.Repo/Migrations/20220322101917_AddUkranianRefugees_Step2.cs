using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddUkranianRefugees_Step2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Question",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 22, "GroupSizeAdults" },
                    { 23, "GroupSizeChildren" },
                    { 24, "GroupSizePets" },
                    { 25, "PreferredLanguage" },
                    { 26, "PreferredLocation" }
                });

            migrationBuilder.InsertData(
                schema: "QuestionSet",
                table: "Question",
                columns: new[] { "ID", "AdditionalData", "AdditionalDataSource", "AnswerContainsSensitiveData", "Name", "QuestionType" },
                values: new object[,]
                {
                    { 22, "", null, false, "How many adults need accomodation", (byte)1 },
                    { 23, "", null, false, "How many children need accomodation", (byte)1 },
                    { 24, "", null, false, "How many pets need accomodation", (byte)2 },
                    { 26, "[{\"Key\":\"North East (England)\",\"Value\":\"North East (England)\"},{\"Key\":\"North West (England)\",\"Value\":\"North West (England)\"},{\"Key\":\"Yorkshire and The Humber\",\"Value\":\"Yorkshire and The Humber\"},{\"Key\":\"East Midlands (England)\",\"Value\":\"East Midlands (England)\"},{\"Key\":\"West Midlands (England)\",\"Value\":\"West Midlands (England)\"},{\"Key\":\"East of England\",\"Value\":\"East of England\"},{\"Key\":\"London\",\"Value\":\"London\"},{\"Key\":\"South East (England)\",\"Value\":\"South East (England)\"},{\"Key\":\"South West (England)\",\"Value\":\"South West (England)\"},{\"Key\":\"Scotland\",\"Value\":\"Scotland\"},{\"Key\":\"Wales\",\"Value\":\"Wales\"}]", null, false, "Do you have a preferred location within the UK", (byte)4 },
                    { 25, "[{\"Key\":\"English\",\"Value\":\"English\"},{\"Key\":\"Ukrainian\",\"Value\":\"Ukrainian\"},{\"Key\":\"Other\",\"Value\":\"Other\"}]", null, false, "What languages do you speak?", (byte)4 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "Question",
                keyColumn: "ID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "Question",
                keyColumn: "ID",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "Question",
                keyColumn: "ID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "Question",
                keyColumn: "ID",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "Question",
                keyColumn: "ID",
                keyValue: 26);

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 22);

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 23);

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 24);

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 25);

            migrationBuilder.DeleteData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 26);
        }
    }
}
