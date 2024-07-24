﻿// <auto-generated />
using System;
using HealthTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HealthTracker.Migrations
{
    [DbContext(typeof(HealthTrackerContext))]
    [Migration("20240724170232_target_columnchanged")]
    partial class target_columnchanged
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.32")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("HealthTracker.Models.DBModels.HealthLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("HealthStatus")
                        .HasColumnType("int");

                    b.Property<int>("PreferenceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated_at")
                        .HasColumnType("datetime2");

                    b.Property<float>("value")
                        .HasColumnType("real");

                    b.HasKey("Id");

                    b.HasIndex("PreferenceId");

                    b.ToTable("HealthLogs");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.IdealData", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"), 1L, 1);

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("HealthStatus")
                        .HasColumnType("int");

                    b.Property<float>("MaxVal")
                        .HasColumnType("real");

                    b.Property<int>("MetricId")
                        .HasColumnType("int");

                    b.Property<float>("MinVal")
                        .HasColumnType("real");

                    b.Property<DateTime>("Updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.HasIndex("MetricId");

                    b.ToTable("IdealDatas");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.Metric", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("MetricType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MetricUnit")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Metrics");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.MonitorPreference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CoachId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("MetricId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CoachId");

                    b.HasIndex("MetricId");

                    b.ToTable("MonitorPreferences");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.Suggestion", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CoachId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Updated_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CoachId");

                    b.HasIndex("UserId");

                    b.ToTable("Suggestions");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.Target", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("PreferenceId")
                        .HasColumnType("int");

                    b.Property<DateTime>("TargetDate")
                        .HasColumnType("datetime2");

                    b.Property<float>("TargetMaxValue")
                        .HasColumnType("real");

                    b.Property<float>("TargetMinValue")
                        .HasColumnType("real");

                    b.Property<int>("TargetStatus")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("PreferenceId");

                    b.ToTable("Targets");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.User", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("Age")
                        .HasColumnType("int");

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated_at")
                        .HasColumnType("datetime2");

                    b.Property<bool>("is_preferenceSet")
                        .HasColumnType("bit");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.UserDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<byte[]>("PasswordEncrypted")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordHashKey")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated_at")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("UsersDetails");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.UserPreference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Created_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("MetricId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Updated_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("MetricId");

                    b.HasIndex("UserId");

                    b.ToTable("UserPreferences");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.HealthLog", b =>
                {
                    b.HasOne("HealthTracker.Models.DBModels.UserPreference", "HealthLogForPreference")
                        .WithMany("healthLogsOfUser")
                        .HasForeignKey("PreferenceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("HealthLogForPreference");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.IdealData", b =>
                {
                    b.HasOne("HealthTracker.Models.DBModels.Metric", "Metric")
                        .WithMany()
                        .HasForeignKey("MetricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Metric");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.MonitorPreference", b =>
                {
                    b.HasOne("HealthTracker.Models.DBModels.User", "MonitorPreferenceForCoach")
                        .WithMany("MonitorPreferences")
                        .HasForeignKey("CoachId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HealthTracker.Models.DBModels.Metric", "Metric")
                        .WithMany()
                        .HasForeignKey("MetricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Metric");

                    b.Navigation("MonitorPreferenceForCoach");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.Suggestion", b =>
                {
                    b.HasOne("HealthTracker.Models.DBModels.User", "SuggestionByCoach")
                        .WithMany("SuggestionsByCoach")
                        .HasForeignKey("CoachId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HealthTracker.Models.DBModels.User", "SuggestionForUser")
                        .WithMany("SuggestionsForUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("SuggestionByCoach");

                    b.Navigation("SuggestionForUser");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.Target", b =>
                {
                    b.HasOne("HealthTracker.Models.DBModels.UserPreference", "TargetForUserPreference")
                        .WithMany("TargetsForUserPreference")
                        .HasForeignKey("PreferenceId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("TargetForUserPreference");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.User", b =>
                {
                    b.HasOne("HealthTracker.Models.DBModels.UserDetail", "UserDetailsForUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserDetailsForUser");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.UserPreference", b =>
                {
                    b.HasOne("HealthTracker.Models.DBModels.Metric", "Metric")
                        .WithMany()
                        .HasForeignKey("MetricId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HealthTracker.Models.DBModels.User", "PreferenceForUser")
                        .WithMany("UserPreferences")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Metric");

                    b.Navigation("PreferenceForUser");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.User", b =>
                {
                    b.Navigation("MonitorPreferences");

                    b.Navigation("SuggestionsByCoach");

                    b.Navigation("SuggestionsForUser");

                    b.Navigation("UserPreferences");
                });

            modelBuilder.Entity("HealthTracker.Models.DBModels.UserPreference", b =>
                {
                    b.Navigation("TargetsForUserPreference");

                    b.Navigation("healthLogsOfUser");
                });
#pragma warning restore 612, 618
        }
    }
}
