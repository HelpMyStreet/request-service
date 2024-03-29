﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class AddNewJobStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "JobStatus",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 8, "Approved" },
                    { 9, "Rejected" }
                });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RequestFormVariant",
                columns: new[] { "ID", "Name" },
                values: new object[] { 34, "NHSVRDemo_RequestSubmitter" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "JobStatus",
                keyColumn: "ID",
                keyValue: 8);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "JobStatus",
                keyColumn: "ID",
                keyValue: 9);            
        }
    }
}
