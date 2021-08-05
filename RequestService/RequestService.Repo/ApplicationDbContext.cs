﻿using RequestService.Repo.EntityFramework.Entities;
using Microsoft.EntityFrameworkCore;
using HelpMyStreet.PostcodeCoordinates.EF.Extensions;
using HelpMyStreet.PostcodeCoordinates.EF.Entities;
using Question = RequestService.Repo.EntityFramework.Entities.Question;
using Job = RequestService.Repo.EntityFramework.Entities.Job;
using RequestService.Repo.Helpers;
using Microsoft.Data.SqlClient;
using RequestService.Repo.Extensions;

namespace RequestService.Repo
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            SqlConnection conn = (SqlConnection)Database.GetDbConnection();
            conn.AddAzureToken();
        }

        public virtual DbSet<Job> Job { get; set; }
        public virtual DbSet<Person> Person { get; set; }
        public virtual DbSet<PersonalDetails> PersonalDetails { get; set; }
        public virtual DbSet<Request> Request { get; set; }
        public virtual DbSet<RequestJobStatus> RequestJobStatus { get; set; }
        public virtual DbSet<SupportActivities> SupportActivities { get; set; }
        public virtual DbSet<DailyReport> DailyReport { get; set; }
        public virtual DbSet<PostcodeEntity> Postcode { get; set; }
        public virtual DbSet<ActivityQuestions> ActivityQuestions { get; set; }
        public virtual DbSet<Question> Question { get; set; }
        public virtual DbSet<JobAvailableToGroup> JobAvailableToGroup { get; set; }
        public virtual DbSet<JobQuestions> JobQuestions { get; set; }

        public virtual DbSet<EnumSupportActivities> EnumSupportActivities { get; set; }
        public virtual DbSet<EnumJobStatuses> EnumJobStatuses { get; set; }
        public virtual DbSet<EnumRequestFormVariants> EnumRequestFormVariants { get; set; }
        public virtual DbSet<EnumQuestionTypes> EnumQuestionTypes { get; set; }
        public virtual DbSet<EnumRequestFormStages> EnumRequestFormStages { get; set; }
        public virtual DbSet<EnumQuestions> EnumQuestions { get; set; }
        public virtual DbSet<EnumRequestTypes> EnumRequestTypes { get; set; }
        public virtual DbSet<EnumDueDateTypes> EnumDueDateTypes { get; set; }
        public virtual DbSet<EnumFrequency> EnumFrequency { get; set; }
        public virtual DbSet<Shift> Shift { get; set; }
        public virtual DbSet<QueryJobHeader> JobHeader { get; set; }
        public virtual DbSet<QueryAllJobs> QueryAllJobs { get; set; }
        public virtual DbSet<RequestSubmission> RequestSubmission { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QueryJobHeader>().HasNoKey().ToView("view_that_does_not_exist");

            modelBuilder.Entity<QueryAllJobs>().HasNoKey().ToView("view_that_does_not_exist2");

            modelBuilder.Entity<DailyReport>().HasNoKey().ToQuery(() => DailyReport.FromSqlRaw("TwoHourlyReport"));

            modelBuilder.Entity<Shift>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.ToTable("Shift", "Request");

                entity.Property(e => e.RequestId).ValueGeneratedNever();

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Request)
                    .WithOne(p => p.Shift)
                    .HasForeignKey<Shift>(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Shift_RequestID");
            });

            modelBuilder.Entity<EnumSupportActivities>(entity =>
            {
                entity.ToTable("SupportActivity", "Lookup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.SetEnumSupportActivityData();
            });

            modelBuilder.Entity<EnumDueDateTypes>(entity =>
            {
                entity.ToTable("DueDateType", "Lookup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.SetEnumDueDateTypeData();
            });

            modelBuilder.Entity<EnumFrequency>(entity =>
            {
                entity.ToTable("Frequency", "Lookup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.SetEnumFrequencyData();
            });

            modelBuilder.Entity<EnumRequestTypes>(entity =>
            {
                entity.ToTable("RequestType", "Lookup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.SetEnumRequestTypeData();
            });

            modelBuilder.Entity<EnumJobStatuses>(entity =>
            {
                entity.ToTable("JobStatus", "Lookup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.SetEnumJobStatusData();
            });

            modelBuilder.Entity<EnumRequestFormVariants>(entity =>
            {
                entity.ToTable("RequestFormVariant", "Lookup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.SetEnumRequestFormVariantsData();
            });

            modelBuilder.Entity<EnumQuestionTypes>(entity =>
            {
                entity.ToTable("QuestionType", "Lookup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.SetEnumQuestionTypeData();
            });

            modelBuilder.Entity<EnumRequestFormStages>(entity =>
            {
                entity.ToTable("RequestFormStage", "Lookup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.SetEnumRequestFormStagesData();
            });

            modelBuilder.Entity<EnumQuestions>(entity =>
            {
                entity.ToTable("Question", "Lookup");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.SetEnumQuestionsData();
            });


            modelBuilder.Entity<Job>(entity =>
            {
                entity.ToTable("Job", "Request");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Details).IsUnicode(false);

                entity.Property(e => e.DueDate).HasColumnType("datetime");

                entity.Property(e => e.NotBeforeDate).HasColumnType("datetime");

                entity.Property(e => e.SupportActivityId).HasColumnName("SupportActivityID");

                entity.Property(e => e.VolunteerUserId).HasColumnName("VolunteerUserID");

                entity.Property(e => e.JobStatusId).HasColumnName("JobStatusID");
                
                entity.Property(e => e.DueDateTypeId).ValueGeneratedNever();

                entity.HasOne(d => d.NewRequest)
                    .WithMany(p => p.Job)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NewRequest_NewRequestID");
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person", "RequestPersonal");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AddressLine1)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AddressLine2)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.AddressLine3)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Locality)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.MobilePhone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.OtherPhone)
                    .HasMaxLength(15)
                    .IsUnicode(false);

                entity.Property(e => e.Postcode)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PersonalDetails>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.ToTable("PersonalDetails", "RequestPersonal");

                entity.Property(e => e.RequestId)
                    .HasColumnName("RequestID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FurtherDetails)
                    .IsRequired()
                    .HasMaxLength(200)
                    .IsUnicode(false);

                entity.Property(e => e.RequestorEmailAddress)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RequestorFirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RequestorLastName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.RequestorPhoneNumber)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.HasOne(d => d.Request)
                    .WithOne(p => p.PersonalDetails)
                    .HasForeignKey<PersonalDetails>(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PersonalDetails_RequestID");
            });


            modelBuilder.Entity<Request>(entity =>
            {
                entity.ToTable("Request", "Request");

                entity.HasIndex(e => e.Guid)
                    .HasName("UC_Guid")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.Property(e => e.DateRequested)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Guid).HasDefaultValueSql("(newid())");

                entity.Property(e => e.OtherDetails).IsUnicode(false);
                entity.Property(e => e.OrganisationName).HasMaxLength(255).IsUnicode(false);
                entity.Property(e => e.PersonIdRecipient).HasColumnName("PersonID_Recipient");

                entity.Property(e => e.PersonIdRequester).HasColumnName("PersonID_Requester");

                entity.Property(e => e.PostCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);

                entity.Property(e => e.SpecialCommunicationNeeds).IsUnicode(false);

                entity.HasOne(d => d.PersonIdRecipientNavigation)
                    .WithMany(p => p.RequestPersonIdRecipientNavigation)
                    .HasForeignKey(d => d.PersonIdRecipient)
                    .HasConstraintName("FK_RequestPersonal_Person_PersonID_Recipient");

                entity.HasOne(d => d.PersonIdRequesterNavigation)
                    .WithMany(p => p.RequestPersonIdRequesterNavigation)
                    .HasForeignKey(d => d.PersonIdRequester)
                    .HasConstraintName("FK_RequestPersonal_Person_PersonID_Requester");
            });

            modelBuilder.Entity<RequestJobStatus>(entity =>
            {
                entity.HasKey(e => new { e.JobId, e.DateCreated, e.JobStatusId });

                entity.ToTable("RequestJobStatus", "Request");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.DateCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.JobStatusId).HasColumnName("JobStatusID");

                entity.Property(e => e.VolunteerUserId).HasColumnName("VolunteerUserID");

                entity.Property(e => e.CreatedByUserId).HasColumnName("CreatedByUserID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.RequestJobStatus)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Job_JobID");
            });

            modelBuilder.Entity<SupportActivities>(entity =>
            {
                entity.HasKey(e => new { e.RequestId, e.ActivityId });

                entity.ToTable("SupportActivities", "Request");

                entity.Property(e => e.RequestId).HasColumnName("RequestID");

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.HasOne(d => d.Request)
                    .WithMany(p => p.SupportActivities)
                    .HasForeignKey(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SupportActivities_RequestID");
            });
            modelBuilder.Entity<ActivityQuestions>(entity =>
            {
                entity.HasKey(e => new { e.ActivityId, e.QuestionId, e.RequestFormVariantId });

                entity.ToTable("ActivityQuestions", "QuestionSet");

                entity.Property(e => e.ActivityId).HasColumnName("ActivityID");

                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.RequestFormVariantId).HasColumnName("RequestFormVariantID");

                entity.Property(e => e.RequestFormStageId).HasColumnName("RequestFormStageID");

                entity.Property(e => e.Location).HasColumnName("Location");

                entity.Property(e => e.Subtext).HasColumnName("Subtext");

                entity.Property(e => e.PlaceholderText).HasColumnName("PlaceholderText");

                entity.SetActivityQuestionData();

                entity.Property(e => e.Order)
                .IsRequired()
                .ValueGeneratedNever();

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.ActivityQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);                
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("Question", "QuestionSet");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AdditionalData).IsUnicode(false);

                entity.Property(e => e.Name)
                   .IsRequired()
                   .HasMaxLength(500)
                   .IsUnicode(false);

                entity.SetQuestionData();
              
            });
            modelBuilder.Entity<JobQuestions>(entity =>
            {
                entity.HasKey(e => new { e.JobId, e.QuestionId });

                entity.ToTable("JobQuestions", "Request");

                entity.Property(e => e.JobId).HasColumnName("JobID");
                entity.Property(e => e.QuestionId).HasColumnName("QuestionID");

                entity.Property(e => e.Answer)
                    .IsRequired()
                    .IsUnicode(false);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.JobQuestions)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.ClientSetNull);

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobQuestions)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<JobAvailableToGroup>(entity =>
            {
                entity.HasKey(e => new { e.JobId, e.GroupId });

                entity.ToTable("JobAvailableToGroup", "Request");

                entity.Property(e => e.JobId).HasColumnName("JobID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobAvailableToGroup)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_JobAvailableToGroup_JobID");
            });

            modelBuilder.Entity<RequestSubmission>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.ToTable("RequestSubmission", "Request");

                entity.Property(e => e.RequestId)
                    .HasColumnName("RequestID")
                    .ValueGeneratedNever();

                entity.Property(e => e.FreqencyId).HasColumnName("FreqencyID");

                entity.HasOne(d => d.Request)
                    .WithOne(p => p.RequestSubmission)
                    .HasForeignKey<RequestSubmission>(d => d.RequestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RequestSubmission_RequestID");
            });

            modelBuilder.SetupPostcodeCoordinateTables();
            modelBuilder.SetupPostcodeCoordinateDefaultIndexes();
        }
      
    }
}
