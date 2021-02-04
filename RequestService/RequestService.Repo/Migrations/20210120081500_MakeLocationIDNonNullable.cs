using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class MakeLocationIDNonNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                schema: "Request",
                table: "Shift",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LocationId",
                schema: "Request",
                table: "Shift",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
