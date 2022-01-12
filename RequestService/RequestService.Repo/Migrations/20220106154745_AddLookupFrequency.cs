using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddLookupFrequency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO [dbo].[table_store_ChangeTracking_version]([TableName],[SYS_CHANGE_VERSION]) SELECT 'Lookup.Frequency',0;
                ");

            migrationBuilder.Sql(@"
                ALTER TABLE [Lookup].[Frequency] ENABLE CHANGE_TRACKING WITH (TRACK_COLUMNS_UPDATED = ON)
                ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DELETE FROM [dbo].[table_store_ChangeTracking_version] WHERE TableName='Lookup.Frequency';                
                ");

            migrationBuilder.Sql(@"
                ALTER TABLE [Lookup].[Frequency] DISABLE CHANGE_TRACKING
                ");
        }
    }
}
