using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddShiftTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Shift",
                schema: "Request",
                columns: table => new
                {
                    RequestId = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    ShiftLength = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shift", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_Shift_RequestID",
                        column: x => x.RequestId,
                        principalSchema: "Request",
                        principalTable: "Request",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shift",
                schema: "Request");


        }
    }
}
