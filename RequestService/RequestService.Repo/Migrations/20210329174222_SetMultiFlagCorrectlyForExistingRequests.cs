using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class SetMultiFlagCorrectlyForExistingRequests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE  [Request].[Request]
                SET     MultiVolunteer = 0, Repeat = 0
                FROM    [Request].[Request]
            ");

            migrationBuilder.Sql(@"
                UPDATE  [Request].[Request]
                SET     MultiVolunteer = 1
                FROM    [Request].[Request] r
                WHERE   (select count(1) from [Request].[Job] j where r.Id = j.RequestId)>1
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
