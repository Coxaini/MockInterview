﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MockInterview.Interviews.DataAccess;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MockInterview.Interviews.DataAccess.Migrations
{
    [DbContext(typeof(InterviewsDbContext))]
    [Migration("20240111230334_AddTagsPropertyToInterview")]
    partial class AddTagsPropertyToInterview
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("interviews")
                .HasAnnotation("ProductVersion", "7.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MockInterview.Interviews.Domain.Entities.Interview", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("FirstMemberId")
                        .HasColumnType("uuid");

                    b.Property<string>("ProgrammingLanguage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid>("SecondMemberId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string[]>("Tags")
                        .IsRequired()
                        .HasColumnType("text[]");

                    b.HasKey("Id");

                    b.HasIndex("FirstMemberId");

                    b.HasIndex("SecondMemberId");

                    b.ToTable("Interviews", "interviews");
                });

            modelBuilder.Entity("MockInterview.Interviews.Domain.Entities.Question", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("AuthorId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DifficultyLevel")
                        .HasColumnType("integer");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProgrammingLanguage")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Questions", "interviews");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Question");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("MockInterview.Interviews.Domain.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users", "interviews");
                });

            modelBuilder.Entity("MockInterview.Interviews.Domain.Entities.InterviewQuestion", b =>
                {
                    b.HasBaseType("MockInterview.Interviews.Domain.Entities.Question");

                    b.Property<string>("Feedback")
                        .HasColumnType("text");

                    b.Property<Guid>("InterviewId")
                        .HasColumnType("uuid");

                    b.Property<int>("OrderIndex")
                        .HasColumnType("integer");

                    b.Property<int?>("Rating")
                        .HasColumnType("integer");

                    b.HasIndex("InterviewId");

                    b.HasDiscriminator().HasValue("InterviewQuestion");
                });

            modelBuilder.Entity("MockInterview.Interviews.Domain.Entities.Interview", b =>
                {
                    b.HasOne("MockInterview.Interviews.Domain.Entities.User", "FirstMember")
                        .WithMany()
                        .HasForeignKey("FirstMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MockInterview.Interviews.Domain.Entities.User", "SecondMember")
                        .WithMany()
                        .HasForeignKey("SecondMemberId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("FirstMember");

                    b.Navigation("SecondMember");
                });

            modelBuilder.Entity("MockInterview.Interviews.Domain.Entities.Question", b =>
                {
                    b.HasOne("MockInterview.Interviews.Domain.Entities.User", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("MockInterview.Interviews.Domain.Entities.InterviewQuestion", b =>
                {
                    b.HasOne("MockInterview.Interviews.Domain.Entities.Interview", null)
                        .WithMany("Questions")
                        .HasForeignKey("InterviewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MockInterview.Interviews.Domain.Entities.Interview", b =>
                {
                    b.Navigation("Questions");
                });
#pragma warning restore 612, 618
        }
    }
}
