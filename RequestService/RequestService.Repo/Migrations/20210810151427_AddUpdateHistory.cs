using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddUpdateHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UpdateHistory",
                schema: "Request",
                columns: table => new
                {
                    RequestID = table.Column<int>(nullable: false),
                    JobID = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    FieldChanged = table.Column<string>(unicode: false, maxLength: 50, nullable: false),
                    CreatedByUserID = table.Column<int>(nullable: false),
                    OldValue = table.Column<string>(unicode: false, maxLength: 100, nullable: false),
                    NewValue = table.Column<string>(unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpdateHistory", x => new { x.RequestID, x.JobID, x.DateCreated, x.FieldChanged });
                    table.ForeignKey(
                        name: "FK_UpdateHistory_RequestID",
                        column: x => x.RequestID,
                        principalSchema: "Request",
                        principalTable: "Request",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UpdateHistory",
                schema: "Request");
        }
    }
}
