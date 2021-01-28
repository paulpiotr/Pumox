﻿#region using

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Pumox.Core.Database.Data;

#endregion

namespace Pumox.Core.Database.Migrations
{
    [DbContext(typeof(PumoxCoreDatabaseContext))]
    internal class PumoxCoreDatabaseContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("Pumox.Core.Models.Company", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint")
                    .UseIdentityColumn();

                b.Property<short>("EstablishmentYear")
                    .HasColumnType("smallint")
                    .HasColumnName("EstablishmentYear");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnType("varchar(64)")
                    .HasColumnName("Name");

                b.HasKey("Id");

                b.HasIndex("EstablishmentYear")
                    .HasDatabaseName("IX_Company_EstablishmentYear");

                b.HasIndex("Name")
                    .HasDatabaseName("IX_Company_Name");

                b.ToTable("Company", "pcd");
            });

            modelBuilder.Entity("Pumox.Core.Models.Employee", b =>
            {
                b.Property<long>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("bigint")
                    .UseIdentityColumn();

                b.Property<long>("CompanyId")
                    .HasColumnType("bigint")
                    .HasColumnName("CompanyId");

                b.Property<DateTime>("DateOfBirth")
                    .HasColumnType("datetime2");

                b.Property<string>("FirstName")
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnType("varchar(64)")
                    .HasColumnName("FirstName");

                b.Property<byte>("JobTitle")
                    .HasColumnType("tinyint")
                    .HasColumnName("JobTitle");

                b.Property<string>("LastName")
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnType("varchar(64)")
                    .HasColumnName("LastName");

                b.HasKey("Id");

                b.HasIndex("CompanyId");

                b.HasIndex("DateOfBirth")
                    .HasDatabaseName("IX_Employee_DateOfBirth");

                b.HasIndex("FirstName")
                    .HasDatabaseName("IX_Employee_FirstName");

                b.HasIndex("JobTitle")
                    .HasDatabaseName("IX_Employee_JobTitle");

                b.HasIndex("LastName")
                    .HasDatabaseName("IX_Employee_LastName");

                b.ToTable("Employee", "pcd");
            });

            modelBuilder.Entity("Pumox.Core.Models.Employee", b =>
            {
                b.HasOne("Pumox.Core.Models.Company", "Company")
                    .WithMany("Employees")
                    .HasForeignKey("CompanyId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Company");
            });

            modelBuilder.Entity("Pumox.Core.Models.Company", b =>
            {
                b.Navigation("Employees");
            });
#pragma warning restore 612, 618
        }
    }
}
