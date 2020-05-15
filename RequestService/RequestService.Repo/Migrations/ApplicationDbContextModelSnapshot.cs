﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RequestService.Repo;

namespace RequestService.Repo.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.Job", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Details")
                        .IsUnicode(false);

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsHealthCritical");

                    b.Property<byte?>("JobStatusId")
                        .HasColumnName("JobStatusID");

                    b.Property<int>("RequestId");

                    b.Property<byte>("SupportActivityId")
                        .HasColumnName("SupportActivityID");

                    b.Property<int?>("VolunteerUserId")
                        .HasColumnName("VolunteerUserID");

                    b.HasKey("Id");

                    b.HasIndex("RequestId");

                    b.ToTable("Job","Request");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AddressLine1")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("AddressLine2")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("AddressLine3")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("EmailAddress")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("FirstName")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .IsUnicode(false);

                    b.Property<string>("Locality")
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("MobilePhone")
                        .HasMaxLength(15)
                        .IsUnicode(false);

                    b.Property<string>("OtherPhone")
                        .HasMaxLength(15)
                        .IsUnicode(false);

                    b.Property<string>("Postcode")
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.ToTable("Person","RequestPersonal");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.PersonalDetails", b =>
                {
                    b.Property<int>("RequestId")
                        .HasColumnName("RequestID");

                    b.Property<string>("FurtherDetails")
                        .IsRequired()
                        .HasMaxLength(200)
                        .IsUnicode(false);

                    b.Property<bool>("OnBehalfOfAnother");

                    b.Property<string>("RequestorEmailAddress")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("RequestorFirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("RequestorLastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.Property<string>("RequestorPhoneNumber")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false);

                    b.HasKey("RequestId");

                    b.ToTable("PersonalDetails","RequestPersonal");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.Request", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("ID")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool?>("AcceptedTerms");

                    b.Property<bool>("CommunicationSent");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnName("CreatedByUserID");

                    b.Property<DateTime>("DateRequested")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<bool?>("ForRequestor");

                    b.Property<byte?>("FulfillableStatus");

                    b.Property<bool>("IsFulfillable");

                    b.Property<string>("OtherDetails")
                        .IsUnicode(false);

                    b.Property<int?>("PersonIdRecipient")
                        .HasColumnName("PersonID_Recipient");

                    b.Property<int?>("PersonIdRequester")
                        .HasColumnName("PersonID_Requester");

                    b.Property<string>("PostCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.Property<bool?>("ReadPrivacyNotice");

                    b.Property<string>("SpecialCommunicationNeeds")
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("PersonIdRecipient");

                    b.HasIndex("PersonIdRequester");

                    b.ToTable("Request","Request");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.RequestJobStatus", b =>
                {
                    b.Property<int>("JobId")
                        .HasColumnName("JobID");

                    b.Property<DateTime>("DateCreated")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<byte>("JobStatusId")
                        .HasColumnName("JobStatusID");

                    b.Property<int?>("CreatedByUserId")
                        .HasColumnName("CreatedByUserID");

                    b.Property<int?>("VolunteerUserId")
                        .HasColumnName("VolunteerUserID");

                    b.HasKey("JobId", "DateCreated", "JobStatusId");

                    b.ToTable("RequestJobStatus","Request");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.SupportActivities", b =>
                {
                    b.Property<int>("RequestId")
                        .HasColumnName("RequestID");

                    b.Property<int>("ActivityId")
                        .HasColumnName("ActivityID");

                    b.HasKey("RequestId", "ActivityId");

                    b.ToTable("SupportActivities","Request");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.Job", b =>
                {
                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Request", "NewRequest")
                        .WithMany("Job")
                        .HasForeignKey("RequestId")
                        .HasConstraintName("FK_NewRequest_NewRequestID");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.PersonalDetails", b =>
                {
                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Request", "Request")
                        .WithOne("PersonalDetails")
                        .HasForeignKey("RequestService.Repo.EntityFramework.Entities.PersonalDetails", "RequestId")
                        .HasConstraintName("FK_PersonalDetails_RequestID");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.Request", b =>
                {
                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Person", "PersonIdRecipientNavigation")
                        .WithMany("RequestPersonIdRecipientNavigation")
                        .HasForeignKey("PersonIdRecipient")
                        .HasConstraintName("FK_RequestPersonal_Person_PersonID_Recipient");

                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Person", "PersonIdRequesterNavigation")
                        .WithMany("RequestPersonIdRequesterNavigation")
                        .HasForeignKey("PersonIdRequester")
                        .HasConstraintName("FK_RequestPersonal_Person_PersonID_Requester");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.RequestJobStatus", b =>
                {
                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Job", "Job")
                        .WithMany("RequestJobStatus")
                        .HasForeignKey("JobId")
                        .HasConstraintName("FK_Job_JobID");
                });

            modelBuilder.Entity("RequestService.Repo.EntityFramework.Entities.SupportActivities", b =>
                {
                    b.HasOne("RequestService.Repo.EntityFramework.Entities.Request", "Request")
                        .WithMany("SupportActivities")
                        .HasForeignKey("RequestId")
                        .HasConstraintName("FK_SupportActivities_RequestID");
                });
#pragma warning restore 612, 618
        }
    }
}
