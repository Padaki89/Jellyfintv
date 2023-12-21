﻿// <auto-generated />
using System;
using Jellyfin.Server.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Jellyfin.Server.Implementations.Migrations
{
    [DbContext(typeof(JellyfinDbContext))]
    [Migration("20230526173516_RemoveEasyPassword")]
    partial class RemoveEasyPassword
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Jellyfin.Data.Entities.AccessSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DayOfWeek")
                        .HasColumnType("INTEGER");

                    b.Property<double>("EndHour")
                        .HasColumnType("REAL");

                    b.Property<double>("StartHour")
                        .HasColumnType("REAL");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AccessSchedules");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.ActivityLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("ItemId")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<int>("LogSeverity")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("Overview")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ShortOverview")
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DateCreated");

                    b.ToTable("ActivityLogs");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.CustomItemDisplayPreferences", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Client")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "ItemId", "Client", "Key")
                        .IsUnique();

                    b.ToTable("CustomItemDisplayPreferences");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.DisplayPreferences", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("ChromecastVersion")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Client")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<string>("DashboardTheme")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EnableNextVideoInfoOverlay")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("IndexBy")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("TEXT");

                    b.Property<int>("ScrollDirection")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ShowBackdrop")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ShowSidebar")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SkipBackwardLength")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SkipForwardLength")
                        .HasColumnType("INTEGER");

                    b.Property<string>("TvHome")
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "ItemId", "Client")
                        .IsUnique();

                    b.ToTable("DisplayPreferences");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.HomeSection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DisplayPreferencesId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("DisplayPreferencesId");

                    b.ToTable("HomeSection");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.ImageInfo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("ImageInfos");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.ItemDisplayPreferences", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Client")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<int?>("IndexBy")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ItemId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("RememberIndexing")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("RememberSorting")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SortBy")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<int>("SortOrder")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<int>("ViewType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("ItemDisplayPreferences");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Kind")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("Permission_Permissions_Guid")
                        .HasColumnType("TEXT");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Value")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "Kind")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.Preference", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Kind")
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("Preference_Preferences_Guid")
                        .HasColumnType("TEXT");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(65535)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId", "Kind")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.ToTable("Preferences");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.Security.ApiKey", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateLastActivity")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AccessToken")
                        .IsUnique();

                    b.ToTable("ApiKeys");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.Security.Device", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AccessToken")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AppName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<string>("AppVersion")
                        .IsRequired()
                        .HasMaxLength(32)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateLastActivity")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("AccessToken", "DateLastActivity");

                    b.HasIndex("DeviceId", "DateLastActivity");

                    b.HasIndex("UserId", "DeviceId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.Security.DeviceOptions", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("CustomName")
                        .HasColumnType("TEXT");

                    b.Property<string>("DeviceId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId")
                        .IsUnique();

                    b.ToTable("DeviceOptions");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("AudioLanguagePreference")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("AuthenticationProviderId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<bool>("DisplayCollectionsView")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("DisplayMissingEpisodes")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EnableAutoLogin")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EnableLocalPassword")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EnableNextEpisodeAutoPlay")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("EnableUserPreferenceAccess")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HidePlayedInLatest")
                        .HasColumnType("INTEGER");

                    b.Property<long>("InternalId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("InvalidLoginAttemptCount")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastActivityDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("LastLoginDate")
                        .HasColumnType("TEXT");

                    b.Property<int?>("LoginAttemptsBeforeLockout")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxActiveSessions")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("MaxParentalAgeRating")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("MustUpdatePassword")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .HasMaxLength(65535)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordResetProviderId")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<bool>("PlayDefaultAudioTrack")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("RememberAudioSelections")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("RememberSubtitleSelections")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("RemoteClientBitrateLimit")
                        .HasColumnType("INTEGER");

                    b.Property<uint>("RowVersion")
                        .IsConcurrencyToken()
                        .HasColumnType("INTEGER");

                    b.Property<string>("SubtitleLanguagePreference")
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<int>("SubtitleMode")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SyncPlayAccess")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT")
                        .UseCollation("NOCASE");

                    b.HasKey("Id");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.AccessSchedule", b =>
                {
                    b.HasOne("Jellyfin.Data.Entities.User", null)
                        .WithMany("AccessSchedules")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.DisplayPreferences", b =>
                {
                    b.HasOne("Jellyfin.Data.Entities.User", null)
                        .WithMany("DisplayPreferences")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.HomeSection", b =>
                {
                    b.HasOne("Jellyfin.Data.Entities.DisplayPreferences", null)
                        .WithMany("HomeSections")
                        .HasForeignKey("DisplayPreferencesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.ImageInfo", b =>
                {
                    b.HasOne("Jellyfin.Data.Entities.User", null)
                        .WithOne("ProfileImage")
                        .HasForeignKey("Jellyfin.Data.Entities.ImageInfo", "UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.ItemDisplayPreferences", b =>
                {
                    b.HasOne("Jellyfin.Data.Entities.User", null)
                        .WithMany("ItemDisplayPreferences")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.Permission", b =>
                {
                    b.HasOne("Jellyfin.Data.Entities.User", null)
                        .WithMany("Permissions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.Preference", b =>
                {
                    b.HasOne("Jellyfin.Data.Entities.User", null)
                        .WithMany("Preferences")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.Security.Device", b =>
                {
                    b.HasOne("Jellyfin.Data.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.DisplayPreferences", b =>
                {
                    b.Navigation("HomeSections");
                });

            modelBuilder.Entity("Jellyfin.Data.Entities.User", b =>
                {
                    b.Navigation("AccessSchedules");

                    b.Navigation("DisplayPreferences");

                    b.Navigation("ItemDisplayPreferences");

                    b.Navigation("Permissions");

                    b.Navigation("Preferences");

                    b.Navigation("ProfileImage");
                });
#pragma warning restore 612, 618
        }
    }
}
