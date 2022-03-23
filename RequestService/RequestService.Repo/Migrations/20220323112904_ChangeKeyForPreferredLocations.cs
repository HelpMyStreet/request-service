using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class ChangeKeyForPreferredLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 26,
                column: "AdditionalData",
                value: "[{\"Key\":\"DH1 1AB\",\"Value\":\"England, North East (inc. Newcastle, Sunderland, Gateshead)\"},{\"Key\":\"M1 1AD\",\"Value\":\"England, North West (inc. Liverpool, Manchester, Bolton)\"},{\"Key\":\"YO1 0ET\",\"Value\":\"England, Yorkshire and The Humber (inc. Sheffield, Leeds, Bradford)\"},{\"Key\":\"NG1 6DQ\",\"Value\":\"England, East Midlands (inc. Leicester, Nottingham, Derby)\"},{\"Key\":\"B1 1QU\",\"Value\":\"England, West Midlands (inc. Birmingham, Coventry, Stoke-on-Trent)\"},{\"Key\":\"CB8 0AA\",\"Value\":\"England, East of England (inc. Luton, Norwich, Southend-on-Sea)\"},{\"Key\":\"SW1A 1AA\",\"Value\":\"England, London\"},{\"Key\":\"RH10 0AG\",\"Value\":\"England, South East (inc. Southampton, Portsmouth, Brighton)\"},{\"Key\":\"BA1 0AA\",\"Value\":\"England, South West (inc. Bristol, Plymouth, Bournemouth)\"},{\"Key\":\"BT1 1AA\",\"Value\":\"Northern Ireland (inc. Belfast, Londonderry, Newtownabbey)\"},{\"Key\":\"PH1 1AA\",\"Value\":\"Scotland (inc. Glasgow, Edinburgh, Aberdeen)\"},{\"Key\":\"SY23 1AB\",\"Value\":\"Wales (inc. Cardiff, Swansea, Newport)\"}]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "QuestionSet",
                table: "Question",
                keyColumn: "ID",
                keyValue: 26,
                column: "AdditionalData",
                value: "[{\"Key\":\"England, North East (inc. Newcastle, Sunderland, Gateshead)\",\"Value\":\"England, North East (inc. Newcastle, Sunderland, Gateshead)\"},{\"Key\":\"England, North West (inc. Liverpool, Manchester, Bolton)\",\"Value\":\"England, North West (inc. Liverpool, Manchester, Bolton)\"},{\"Key\":\"England, Yorkshire and The Humber (inc. Sheffield, Leeds, Bradford)\",\"Value\":\"England, Yorkshire and The Humber (inc. Sheffield, Leeds, Bradford)\"},{\"Key\":\"England, East Midlands (inc. Leicester, Nottingham, Derby)\",\"Value\":\"England, East Midlands (inc. Leicester, Nottingham, Derby)\"},{\"Key\":\"England, West Midlands (inc. Birmingham, Coventry, Stoke-on-Trent)\",\"Value\":\"England, West Midlands (inc. Birmingham, Coventry, Stoke-on-Trent)\"},{\"Key\":\"England, East of England (inc. Luton, Norwich, Southend-on-Sea)\",\"Value\":\"England, East of England (inc. Luton, Norwich, Southend-on-Sea)\"},{\"Key\":\"England, London\",\"Value\":\"England, London\"},{\"Key\":\"England, South East (inc. Southampton, Portsmouth, Brighton)\",\"Value\":\"England, South East (inc. Southampton, Portsmouth, Brighton)\"},{\"Key\":\"England, South West (inc. Bristol, Plymouth, Bournemouth)\",\"Value\":\"England, South West (inc. Bristol, Plymouth, Bournemouth)\"},{\"Key\":\"Northern Ireland (inc. Belfast, Londonderry, Newtownabbey)\",\"Value\":\"Northern Ireland (inc. Belfast, Londonderry, Newtownabbey)\"},{\"Key\":\"Scotland (inc. Glasgow, Edinburgh, Aberdeen)\",\"Value\":\"Scotland (inc. Glasgow, Edinburgh, Aberdeen)\"},{\"Key\":\"Wales (inc. Cardiff, Swansea, Newport)\",\"Value\":\"Wales (inc. Cardiff, Swansea, Newport)\"}]");
        }
    }
}
