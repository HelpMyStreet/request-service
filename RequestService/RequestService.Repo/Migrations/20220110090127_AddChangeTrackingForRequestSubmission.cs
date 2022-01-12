using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddChangeTrackingForRequestSubmission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[table_store_ChangeTracking_version]([TableName],[SYS_CHANGE_VERSION]) SELECT 'Request.RequestSubmission',0;
                ");

            migrationBuilder.Sql(@"
                ALTER TABLE [Request].[RequestSubmission] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = ON)
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [dbo].[table_store_ChangeTracking_version] WHERE TableName='Request.RequestSubmission';                
                ");

            migrationBuilder.Sql(@"
                ALTER TABLE [Request].[RequestSubmission] DISABLE CHANGE_TRACKING
                ");
        }
    }
}
