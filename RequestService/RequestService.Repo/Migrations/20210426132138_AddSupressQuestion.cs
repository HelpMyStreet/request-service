using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddSupressQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 19,
                columns: new[] { "AdditionalData", "Name" },
                values: new object[] { "[{\"Key\":\"Yes\",\"Value\":\"Yes, the volunteer will see details of the person or organisation who requested the help and contact them first\"},{\"Key\":\"No\",\"Value\":\"No, the volunteer can access the personal details of the person who needs help as soon as they accept the request\"}]", "Would you like to hide personal details of the person in need of help and require volunteers to contact the organisation/person who requested the help first?" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 19,
                columns: new[] { "AdditionalData", "Name" },
                values: new object[] { "[{\"Key\":\"Yes\",\"Value\":\"Yes, the volunteer should use our ‘requester’ details and we will provide the necessary personal information\"},{\"Key\":\"No\",\"Value\":\"No, the volunteer can access the necessary personal information as soon as they accept the request\"}]", "Would you like the volunteer to contact your organisation directly to obtain the personal details for this request?" });
        }
    }
}
