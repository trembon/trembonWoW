﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using trembonWoW.Database;

#nullable disable

namespace trembonWoW.Database.Migrations
{
    [DbContext(typeof(DefaultContext))]
    partial class DefaultContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("trembonWoW.Database.Entities.BoostCharacterTemplates", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("GoldToSend")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SetToLevel")
                        .HasColumnType("integer");

                    b.Property<string>("TeleportToAlliance")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TeleportToHorde")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("BoostCharacterTemplate");
                });

            modelBuilder.Entity("trembonWoW.Database.Entities.BoostedCharacters", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AccountID")
                        .HasColumnType("integer");

                    b.Property<DateTime>("BoostedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("CharacterID")
                        .HasColumnType("integer");

                    b.Property<Guid>("TemplateID")
                        .HasColumnType("uuid");

                    b.HasKey("ID");

                    b.HasIndex("TemplateID");

                    b.ToTable("BoostedCharacters");
                });

            modelBuilder.Entity("trembonWoW.Database.Entities.ListedFile", b =>
                {
                    b.Property<Guid>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Filename")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("SortOrder")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("ListedFiles");
                });

            modelBuilder.Entity("trembonWoW.Database.Entities.BoostedCharacters", b =>
                {
                    b.HasOne("trembonWoW.Database.Entities.BoostCharacterTemplates", "Template")
                        .WithMany()
                        .HasForeignKey("TemplateID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");
                });
#pragma warning restore 612, 618
        }
    }
}
