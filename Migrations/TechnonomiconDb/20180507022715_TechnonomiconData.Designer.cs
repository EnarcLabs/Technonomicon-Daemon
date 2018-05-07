﻿// <auto-generated />
using EnarcLabs.Technonomicon.Daemon.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace EnarcLabs.Technonomicon.Daemon.Migrations.TechnonomiconDb
{
    [DbContext(typeof(TechnonomiconDbContext))]
    [Migration("20180507022715_TechnonomiconData")]
    partial class TechnonomiconData
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("EnarcLabs.Technonomicon.Daemon.Models.Event", b =>
                {
                    b.Property<Guid>("EventId")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("EventPostPostId");

                    b.Property<Guid?>("NextEventEventId");

                    b.HasKey("EventId");

                    b.HasIndex("EventPostPostId");

                    b.HasIndex("NextEventEventId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("EnarcLabs.Technonomicon.Daemon.Models.Post", b =>
                {
                    b.Property<Guid>("PostId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Body");

                    b.Property<Guid?>("CreatorUserId");

                    b.Property<Guid?>("PostId1");

                    b.Property<string>("Title");

                    b.HasKey("PostId");

                    b.HasIndex("CreatorUserId");

                    b.HasIndex("PostId1");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("EnarcLabs.Technonomicon.Daemon.Models.Tag", b =>
                {
                    b.Property<Guid>("TagId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<Guid?>("ParentTagTagId");

                    b.Property<Guid?>("TagPostPostId");

                    b.HasKey("TagId");

                    b.HasIndex("ParentTagTagId");

                    b.HasIndex("TagPostPostId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("EnarcLabs.Technonomicon.Daemon.Models.User", b =>
                {
                    b.Property<Guid>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email");

                    b.Property<string>("Username");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("EnarcLabs.Technonomicon.Daemon.Models.Event", b =>
                {
                    b.HasOne("EnarcLabs.Technonomicon.Daemon.Models.Post", "EventPost")
                        .WithMany()
                        .HasForeignKey("EventPostPostId");

                    b.HasOne("EnarcLabs.Technonomicon.Daemon.Models.Event", "NextEvent")
                        .WithMany()
                        .HasForeignKey("NextEventEventId");
                });

            modelBuilder.Entity("EnarcLabs.Technonomicon.Daemon.Models.Post", b =>
                {
                    b.HasOne("EnarcLabs.Technonomicon.Daemon.Models.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorUserId");

                    b.HasOne("EnarcLabs.Technonomicon.Daemon.Models.Post")
                        .WithMany("RelatedPosts")
                        .HasForeignKey("PostId1");
                });

            modelBuilder.Entity("EnarcLabs.Technonomicon.Daemon.Models.Tag", b =>
                {
                    b.HasOne("EnarcLabs.Technonomicon.Daemon.Models.Tag", "ParentTag")
                        .WithMany()
                        .HasForeignKey("ParentTagTagId");

                    b.HasOne("EnarcLabs.Technonomicon.Daemon.Models.Post", "TagPost")
                        .WithMany()
                        .HasForeignKey("TagPostPostId");
                });
#pragma warning restore 612, 618
        }
    }
}
