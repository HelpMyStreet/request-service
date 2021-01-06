using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddGuidToRequestTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "Guid",
                schema: "Request",
                table: "Request",
                nullable: false,
                defaultValueSql: "(newid())");
            
            migrationBuilder.CreateIndex(
                name: "UC_Guid",
                schema: "Request",
                table: "Request",
                column: "Guid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UC_Guid",
                schema: "Request",
                table: "Request");

            migrationBuilder.DropColumn(
                name: "Guid",
                schema: "Request",
                table: "Request");
        }
    }
}
