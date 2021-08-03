using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddRequestorTypesLookup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestorType",
                schema: "Lookup",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestorType", x => x.ID);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RequestorType",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "OnBehalf" });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RequestorType",
                columns: new[] { "ID", "Name" },
                values: new object[] { 2, "Myself" });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RequestorType",
                columns: new[] { "ID", "Name" },
                values: new object[] { 3, "Organisation" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestorType",
                schema: "Lookup");
        }
    }
}
