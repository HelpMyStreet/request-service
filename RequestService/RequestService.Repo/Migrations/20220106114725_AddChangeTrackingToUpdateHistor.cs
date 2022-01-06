using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddChangeTrackingToUpdateHistor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[table_store_ChangeTracking_version]([TableName],[SYS_CHANGE_VERSION]) SELECT 'Request.UpdateHistory',0;
                ");

            migrationBuilder.Sql(@"
                ALTER TABLE [Request].[UpdateHistory] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = ON)
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [dbo].[table_store_ChangeTracking_version] WHERE TableName='Request.UpdateHistory';                
                ");

            migrationBuilder.Sql(@"
                ALTER TABLE [Request].[UpdateHistory] DISABLE CHANGE_TRACKING
                ");
        }
    }
}
