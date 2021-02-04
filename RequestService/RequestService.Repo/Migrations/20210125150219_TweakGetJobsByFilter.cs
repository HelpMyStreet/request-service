﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace RequestService.Repo.Migrations
{
    public partial class TweakGetJobsByFilter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "RequestFormVariant",
                columns: new[] { "ID", "Name" },
                values: new object[] { 17, "LincolnshireVolunteers" });

            migrationBuilder.InsertData(
                schema: "Lookup",
                table: "SupportActivity",
                columns: new[] { "ID", "Name" },
                values: new object[] { 24, "VaccineSupport" });

            migrationBuilder.Sql(@"

			ALTER PROCEDURE [Request].[GetJobsByFilter]
(
	@UserID int,
	@SupportActivities varchar(100),
	@RefferingGroups  varchar(100),
	@JobStatuses varchar(100),
	@Groups varchar(100)
)
AS
BEGIN
    SET NOCOUNT ON

	select		distinct 
				j.VolunteerUserID,
				r.DateRequested,
				(select max(DateCreated) from [Request].[RequestJobStatus] rjs where j.ID = rjs.JobID) as DateStatusLastChanged,
				r.ReferringGroupId,
				cast(0.0 as float) as DistanceInMiles,
				r.PostCode,
				j.IsHealthCritical,
				j.DueDate,
				j.SupportActivityID,
				j.JobStatusID,
				j.ID as JobID,
				r.Archive,
				j.Reference,
                j.DueDateTypeId
	from		Request.Job j 
				inner join Request.Request r on j.RequestId = r.ID
				left outer join Request.JobAvailableToGroup ja on j.ID = ja.JobID
	where		(j.VolunteerUserID = @UserID or @UserID=0)
				and (j.SupportActivityID in (SELECT value FROM STRING_SPLIT( @SupportActivities, ',')) OR @SupportActivities='')
				and (r.ReferringGroupId in (SELECT value FROM STRING_SPLIT( @RefferingGroups, ',')) OR @RefferingGroups='')
				and (j.JobStatusID in (SELECT value FROM STRING_SPLIT( @JobStatuses, ',')) or @JobStatuses='')
				and (ja.GroupID in (SELECT value FROM STRING_SPLIT(@Groups, ',')) or @Groups='')
				and r.RequestType = 1
END
			");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "RequestFormVariant",
                keyColumn: "ID",
                keyValue: 17);

            migrationBuilder.DeleteData(
                schema: "Lookup",
                table: "SupportActivity",
                keyColumn: "ID",
                keyValue: 24);

			migrationBuilder.Sql(@"

			ALTER PROCEDURE [Request].[GetJobsByFilter]
(
	@UserID int,
	@SupportActivities varchar(100),
	@RefferingGroupID int,
	@JobStatuses varchar(100),
	@Groups varchar(100)
)
AS
BEGIN
    SET NOCOUNT ON

	select		distinct 
				j.VolunteerUserID,
				r.DateRequested,
				(select max(DateCreated) from [Request].[RequestJobStatus] rjs where j.ID = rjs.JobID) as DateStatusLastChanged,
				r.ReferringGroupId,
				cast(0.0 as float) as DistanceInMiles,
				r.PostCode,
				j.IsHealthCritical,
				j.DueDate,
				j.SupportActivityID,
				j.JobStatusID,
				j.ID as JobID,
				r.Archive,
				j.Reference,
                j.DueDateTypeId
	from		Request.Job j 
				inner join Request.Request r on j.RequestId = r.ID
				left outer join Request.JobAvailableToGroup ja on j.ID = ja.JobID
	where		(j.VolunteerUserID = @UserID or @UserID=0)
				and (j.SupportActivityID in (SELECT value FROM STRING_SPLIT( @SupportActivities, ',')) OR @SupportActivities='')
				and (r.ReferringGroupId = @RefferingGroupID or @RefferingGroupID=0)
				and (j.JobStatusID in (SELECT value FROM STRING_SPLIT( @JobStatuses, ',')) or @JobStatuses='')
				and (ja.GroupID in (SELECT value FROM STRING_SPLIT(@Groups, ',')) or @Groups='')
				and r.RequestType = 1
END
			");
		}
    }
}
