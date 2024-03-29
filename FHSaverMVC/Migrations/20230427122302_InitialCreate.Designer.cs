﻿// <auto-generated />
using System;
using FHSaverMVC.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FHSaverMVC.Migrations
{
    [DbContext(typeof(DBContext))]
    [Migration("20230427122302_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FHSaverMVC.Context.Entities.Folder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ParentFolderId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("ParentFolderId");

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("FHSaverMVC.Context.Entities.Folder", b =>
                {
                    b.HasOne("FHSaverMVC.Context.Entities.Folder", "ParentFolder")
                        .WithMany("Children")
                        .HasForeignKey("ParentFolderId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("ParentFolder");
                });

            modelBuilder.Entity("FHSaverMVC.Context.Entities.Folder", b =>
                {
                    b.Navigation("Children");
                });
#pragma warning restore 612, 618
        }
    }
}
