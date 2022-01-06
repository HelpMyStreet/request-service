using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddChangeTrackingToRequestSupportActivities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[table_store_ChangeTracking_version]([TableName],[SYS_CHANGE_VERSION]) SELECT 'Request.SupportActivities',0;
                ");

            migrationBuilder.Sql(@"
                ALTER TABLE [Request].[SupportActivities] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = ON)
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [dbo].[table_store_ChangeTracking_version] WHERE TableName='Request.SupportActivities';                
                ");

            migrationBuilder.Sql(@"
                ALTER TABLE [Request].[SupportActivities] DISABLE CHANGE_TRACKING
                ");
        }
    }
}
