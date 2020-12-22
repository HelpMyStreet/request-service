using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddRequestType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "RequestType",
                schema: "Request",
                table: "Request",
                nullable: false,
                defaultValue: (byte)1);

            migrationBuilder.Sql(@"
                UPDATE [Request].[Request] SET RequestType = 1;               
            ");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequestType",
                schema: "Request",
                table: "Request");
        }
    }
}
