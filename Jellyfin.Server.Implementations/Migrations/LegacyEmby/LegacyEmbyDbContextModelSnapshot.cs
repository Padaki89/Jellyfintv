﻿// <auto-generated />
using System;
using Jellyfin.Server.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Jellyfin.Server.Implementations.Migrations.LegacyEmby
{
    [DbContext(typeof(LibraryDbContext))]
    partial class LegacyEmbyDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.13");

            modelBuilder.Entity("Jellyfin.Data.Entities.UserItemData", b =>
                {
                    b.Property<Guid>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Key")
                        .HasColumnType("TEXT");

                    b.Property<int?>("AudioStreamIndex")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsFavorite")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("LastPlayedDate")
                        .HasColumnType("TEXT");

                    b.Property<bool?>("Likes")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PlayCount")
                        .HasColumnType("INTEGER");

                    b.Property<long>("PlaybackPositionTicks")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Played")
                        .HasColumnType("INTEGER");

                    b.Property<double?>("Rating")
                        .HasColumnType("REAL");

                    b.Property<int?>("SubtitleStreamIndex")
                        .HasColumnType("INTEGER");

                    b.HasKey("UserId", "Key");

                    b.ToTable("UserDatas");
                });
#pragma warning restore 612, 618
        }
    }
}
