using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddEnumFrequency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Frequency",
                schema: "Lookup",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frequency", x => x.ID);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "Frequency",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 10, "Once" },
                    { 20, "Daily" },
                    { 30, "Weekly" },
                    { 40, "Fortnightly" },
                    { 50, "EveryFourWeeks" }
                });            
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Frequency",
                schema: "Lookup");                        
        }
    }
}
