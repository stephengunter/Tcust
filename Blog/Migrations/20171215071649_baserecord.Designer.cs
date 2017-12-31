﻿// <auto-generated />
using Blog.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Blog.Migrations
{
    [DbContext(typeof(BlogContext))]
    [Migration("20171215071649_baserecord")]
    partial class baserecord
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Blog.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Blog.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Content");

                    b.Property<string>("ContentId");

                    b.Property<int>("CreateMonth");

                    b.Property<int>("CreateYear");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<int>("DisplayOrder");

                    b.Property<bool>("Down");

                    b.Property<DateTime>("LastUpdated")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("getdate()");

                    b.Property<string>("Summary");

                    b.Property<string>("Title");

                    b.Property<bool>("Top");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Blog.Models.PostCategory", b =>
                {
                    b.Property<int>("PostId");

                    b.Property<int>("CategoryId");

                    b.HasKey("PostId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("PostCategory");
                });

            modelBuilder.Entity("Blog.Models.UploadFile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Height");

                    b.Property<string>("ItemOID");

                    b.Property<string>("Name");

                    b.Property<string>("PS");

                    b.Property<string>("Path");

                    b.Property<int>("PostId");

                    b.Property<string>("PreviewPath");

                    b.Property<bool>("Top");

                    b.Property<string>("Type");

                    b.Property<int>("Width");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.ToTable("UploadFiles");
                });

            modelBuilder.Entity("Blog.Models.PostCategory", b =>
                {
                    b.HasOne("Blog.Models.Category", "Category")
                        .WithMany("PostCategories")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Blog.Models.Post", "Post")
                        .WithMany("PostCategories")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Blog.Models.UploadFile", b =>
                {
                    b.HasOne("Blog.Models.Post", "Post")
                        .WithMany("UploadFiles")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
