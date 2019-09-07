﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;
using Tcust.DAL;
using Tcust.Models;

namespace Tcust.Migrations
{
    [DbContext(typeof(TcustContext))]
    [Migration("20180119045856_addTerm")]
    partial class addTerm
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ApplicationCore.Entities.CountryArea", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<bool>("IsPartition");

                    b.Property<string>("Name");

                    b.Property<int>("Parent");

                    b.Property<int>("PartitionId");

                    b.HasKey("Id");

                    b.ToTable("CountryAreas");
                });

            modelBuilder.Entity("Tcust.Models.Contract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime?>("EndDate");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<DateTime?>("StartDate");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("Id");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("Tcust.Models.ContractType", b =>
                {
                    b.Property<int>("ContractId");

                    b.Property<int>("TypeId");

                    b.HasKey("ContractId", "TypeId");

                    b.HasIndex("TypeId");

                    b.ToTable("ContractType");
                });

            modelBuilder.Entity("Tcust.Models.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Name");

                    b.Property<DateTime>("StartDate");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("Id");

                    b.ToTable("Departments");
                });

            modelBuilder.Entity("Tcust.Models.DepartmentContract", b =>
                {
                    b.Property<int>("DepartmentId");

                    b.Property<int>("ContractId");

                    b.HasKey("DepartmentId", "ContractId");

                    b.HasIndex("ContractId");

                    b.ToTable("DepartmentContract");
                });

            modelBuilder.Entity("Tcust.Models.Partner", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int?>("AreaId");

                    b.Property<int>("CountryId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Description");

                    b.Property<DateTime>("LastUpdated");

                    b.Property<string>("Name");

                    b.Property<int>("Type");

                    b.Property<string>("UpdatedBy");

                    b.HasKey("Id");

                    b.ToTable("Partners");
                });

            modelBuilder.Entity("Tcust.Models.PartnerContract", b =>
                {
                    b.Property<int>("PartnerId");

                    b.Property<int>("ContractId");

                    b.HasKey("PartnerId", "ContractId");

                    b.HasIndex("ContractId");

                    b.ToTable("PartnerContract");
                });

            modelBuilder.Entity("Tcust.Models.Term", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<int>("Number");

                    b.Property<int>("TermYearId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("TermYearId");

                    b.ToTable("Terms");
                });

            modelBuilder.Entity("Tcust.Models.TermYear", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Title");

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.ToTable("TermYears");
                });

            modelBuilder.Entity("Tcust.Models.Type", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.Property<int>("Order");

                    b.HasKey("Id");

                    b.ToTable("Types");
                });

            modelBuilder.Entity("Tcust.Models.ContractType", b =>
                {
                    b.HasOne("Tcust.Models.Contract", "Contract")
                        .WithMany("ContractTypes")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Tcust.Models.Type", "Type")
                        .WithMany("ContractTypes")
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tcust.Models.DepartmentContract", b =>
                {
                    b.HasOne("Tcust.Models.Contract", "Contract")
                        .WithMany("DepartmentContracts")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Tcust.Models.Department", "Department")
                        .WithMany("DepartmentContracts")
                        .HasForeignKey("DepartmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tcust.Models.PartnerContract", b =>
                {
                    b.HasOne("Tcust.Models.Contract", "Contract")
                        .WithMany("PartnerContracts")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Tcust.Models.Partner", "Partner")
                        .WithMany("PartnerContracts")
                        .HasForeignKey("PartnerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Tcust.Models.Term", b =>
                {
                    b.HasOne("Tcust.Models.TermYear", "TermYear")
                        .WithMany("Terms")
                        .HasForeignKey("TermYearId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}