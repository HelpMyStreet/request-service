using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class TweakPreferredLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 26,
                column: "AdditionalData",
                value: "[{\"Key\":\"England, North East (inc. Newcastle, Sunderland, Gateshead)\",\"Value\":\"England, North East (inc. Newcastle, Sunderland, Gateshead)\"},{\"Key\":\"England, North West (inc. Liverpool, Manchester, Bolton)\",\"Value\":\"England, North West (inc. Liverpool, Manchester, Bolton)\"},{\"Key\":\"England, Yorkshire and The Humber (inc. Sheffield, Leeds, Bradford)\",\"Value\":\"England, Yorkshire and The Humber (inc. Sheffield, Leeds, Bradford)\"},{\"Key\":\"England, East Midlands (inc. Leicester, Nottingham, Derby)\",\"Value\":\"England, East Midlands (inc. Leicester, Nottingham, Derby)\"},{\"Key\":\"England, West Midlands (inc. Birmingham, Coventry, Stoke-on-Trent)\",\"Value\":\"England, West Midlands (inc. Birmingham, Coventry, Stoke-on-Trent)\"},{\"Key\":\"England, East of England (inc. Luton, Norwich, Southend-on-Sea)\",\"Value\":\"England, East of England (inc. Luton, Norwich, Southend-on-Sea)\"},{\"Key\":\"England, London\",\"Value\":\"England, London\"},{\"Key\":\"England, South East (inc. Southampton, Portsmouth, Brighton)\",\"Value\":\"England, South East (inc. Southampton, Portsmouth, Brighton)\"},{\"Key\":\"England, South West (inc. Bristol, Plymouth, Bournemouth)\",\"Value\":\"England, South West (inc. Bristol, Plymouth, Bournemouth)\"},{\"Key\":\"Northern Ireland (inc. Belfast, Londonderry, Newtownabbey)\",\"Value\":\"Northern Ireland (inc. Belfast, Londonderry, Newtownabbey)\"},{\"Key\":\"Scotland (inc. Glasgow, Edinburgh, Aberdeen)\",\"Value\":\"Scotland (inc. Glasgow, Edinburgh, Aberdeen)\"},{\"Key\":\"Wales (inc. Cardiff, Swansea, Newport)\",\"Value\":\"Wales (inc. Cardiff, Swansea, Newport)\"}]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 26,
                column: "AdditionalData",
                value: "[{\"Key\":\"North East (England)\",\"Value\":\"North East (England)\"},{\"Key\":\"North West (England)\",\"Value\":\"North West (England)\"},{\"Key\":\"Yorkshire and The Humber\",\"Value\":\"Yorkshire and The Humber\"},{\"Key\":\"East Midlands (England)\",\"Value\":\"East Midlands (England)\"},{\"Key\":\"West Midlands (England)\",\"Value\":\"West Midlands (England)\"},{\"Key\":\"East of England\",\"Value\":\"East of England\"},{\"Key\":\"London\",\"Value\":\"London\"},{\"Key\":\"South East (England)\",\"Value\":\"South East (England)\"},{\"Key\":\"South West (England)\",\"Value\":\"South West (England)\"},{\"Key\":\"Scotland\",\"Value\":\"Scotland\"},{\"Key\":\"Wales\",\"Value\":\"Wales\"}]");
        }
    }
}
