using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class UpdateAdditionalDataForPrivacyQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 19,
                column: "AdditionalData",
                value: "[{\"Key\":\"Yes\",\"Value\":\"Yes, the volunteer should use our ‘requester’ details and we will provide the necessary personal information\"},{\"Key\":\"No\",\"Value\":\"No, the volunteer can access the necessary personal information as soon as they accept the request\"}]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 19,
                column: "AdditionalData",
                value: "");
        }
    }
}
