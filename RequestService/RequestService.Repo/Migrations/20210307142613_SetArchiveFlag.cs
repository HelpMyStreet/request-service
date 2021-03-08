using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class SetArchiveFlag : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                Update [Request].[Request]
                Set Archive = 0
                where Archive is null
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
