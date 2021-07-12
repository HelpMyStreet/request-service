using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class LogRequestEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RequestEvent",
                schema: "Lookup",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestEvent", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LogRequestEvent",
                schema: "Request",
                columns: table => new
                {
                    RequestId = table.Column<int>(nullable: false),
                    DateRequested = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    RequestEventId = table.Column<byte>(nullable: false),
                    JobId = table.Column<int>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogRequestEvent", x => new { x.RequestId, x.RequestEventId, x.DateRequested });
                    table.ForeignKey(
                        name: "FK_LogRequestEvent_RequestID",
                        column: x => x.RequestId,
                        principalSchema: "Request",
                        principalTable: "Request",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RequestEvent",
                columns: new[] { "ID", "Name" },
                values: new object[] { 1, "ShowFullPostCode" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestEvent",
                schema: "Lookup");

            migrationBuilder.DropTable(
                name: "LogRequestEvent",
                schema: "Request");
        }
    }
}
