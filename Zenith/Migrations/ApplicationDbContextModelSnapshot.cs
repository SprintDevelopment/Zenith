﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Zenith.Data;

#nullable disable

namespace Zenith.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Zenith.Models.Buy", b =>
                {
                    b.Property<int>("BuyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BuyId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<short>("CompanyId")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.HasKey("BuyId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Buys");
                });

            modelBuilder.Entity("Zenith.Models.BuyItem", b =>
                {
                    b.Property<long>("BuyItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("BuyItemId"));

                    b.Property<int>("BuyId")
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<long>("UnitPrice")
                        .HasColumnType("bigint");

                    b.HasKey("BuyItemId");

                    b.HasIndex("BuyId");

                    b.HasIndex("MaterialId");

                    b.ToTable("BuyItems");
                });

            modelBuilder.Entity("Zenith.Models.Cheque", b =>
                {
                    b.Property<int>("ChequeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChequeId"));

                    b.Property<DateTime>("ChequeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("ChequeNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("ChequeValue")
                        .HasColumnType("bigint");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<DateTime>("IssueDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("SaleId")
                        .HasColumnType("int");

                    b.HasKey("ChequeId");

                    b.HasIndex("SaleId");

                    b.ToTable("Cheques");
                });

            modelBuilder.Entity("Zenith.Models.Company", b =>
                {
                    b.Property<short>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("CompanyId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int>("CompanyType")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Fax")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("TaxRegistrationNumber")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Tel")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.HasKey("CompanyId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Zenith.Models.Delivery", b =>
                {
                    b.Property<long>("DeliveryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("DeliveryId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<long>("DeliveryFee")
                        .HasColumnType("bigint");

                    b.Property<string>("DeliveryNumber")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<int>("DriverId")
                        .HasColumnType("int");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<int>("MachineId")
                        .HasColumnType("int");

                    b.Property<long>("SaleItemId")
                        .HasColumnType("bigint");

                    b.HasKey("DeliveryId");

                    b.HasIndex("DriverId");

                    b.HasIndex("MachineId");

                    b.HasIndex("SaleItemId");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("Zenith.Models.Machine", b =>
                {
                    b.Property<int>("MachineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MachineId"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<long>("DefaultDeliveryFee")
                        .HasColumnType("bigint");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("MachineId");

                    b.ToTable("Machines");
                });

            modelBuilder.Entity("Zenith.Models.Material", b =>
                {
                    b.Property<int>("MaterialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaterialId"));

                    b.Property<int>("AvailableAmount")
                        .HasColumnType("int");

                    b.Property<long>("BuyPrice")
                        .HasColumnType("bigint");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<bool>("IsMixed")
                        .HasColumnType("bit");

                    b.Property<int>("MetersPerTon")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<long>("SalePrice")
                        .HasColumnType("bigint");

                    b.HasKey("MaterialId");

                    b.ToTable("Materials");
                });

            modelBuilder.Entity("Zenith.Models.Mixture", b =>
                {
                    b.Property<int>("MixtureId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MixtureId"));

                    b.Property<long>("BuyPrice")
                        .HasColumnType("bigint");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<int>("RelatedMaterialId")
                        .HasColumnType("int");

                    b.Property<long>("SalePrice")
                        .HasColumnType("bigint");

                    b.HasKey("MixtureId");

                    b.ToTable("Mixtures");
                });

            modelBuilder.Entity("Zenith.Models.MixtureItem", b =>
                {
                    b.Property<long>("MixtureItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("MixtureItemId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<int>("MixtureId")
                        .HasColumnType("int");

                    b.Property<short>("Percent")
                        .HasColumnType("smallint");

                    b.HasKey("MixtureItemId");

                    b.HasIndex("MaterialId");

                    b.HasIndex("MixtureId");

                    b.ToTable("MixtureItems");
                });

            modelBuilder.Entity("Zenith.Models.Note", b =>
                {
                    b.Property<int>("NoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NoteId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<DateTime>("NotifyDateTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("NotifyType")
                        .HasColumnType("int");

                    b.Property<string>("Subject")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("NoteId");

                    b.ToTable("Notes");
                });

            modelBuilder.Entity("Zenith.Models.Outgo", b =>
                {
                    b.Property<int>("OutgoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("OutgoId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<short>("OutgoCategoryId")
                        .HasColumnType("smallint");

                    b.Property<long>("Value")
                        .HasColumnType("bigint");

                    b.HasKey("OutgoId");

                    b.HasIndex("OutgoCategoryId");

                    b.ToTable("Outgoes");
                });

            modelBuilder.Entity("Zenith.Models.OutgoCategory", b =>
                {
                    b.Property<short>("OutgoCategoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<short>("OutgoCategoryId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<short?>("ParentOutgoCategoryId")
                        .HasColumnType("smallint");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("OutgoCategoryId");

                    b.HasIndex("ParentOutgoCategoryId");

                    b.ToTable("OutgoCategories");
                });

            modelBuilder.Entity("Zenith.Models.Person", b =>
                {
                    b.Property<int>("PersonId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<int>("CostCenter")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Job")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.Property<long>("Salary")
                        .HasColumnType("bigint");

                    b.Property<string>("Tel")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("nvarchar(13)");

                    b.HasKey("PersonId");

                    b.ToTable("People");
                });

            modelBuilder.Entity("Zenith.Models.PersonnelAbsence", b =>
                {
                    b.Property<int>("PersonnelAbsenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PersonnelAbsenceId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("PersonnelAbsenceId");

                    b.HasIndex("PersonId");

                    b.ToTable("PersonnelAbsence");
                });

            modelBuilder.Entity("Zenith.Models.Sale", b =>
                {
                    b.Property<int>("SaleId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SaleId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<short>("CompanyId")
                        .HasColumnType("smallint");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.HasKey("SaleId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Sales");
                });

            modelBuilder.Entity("Zenith.Models.SaleItem", b =>
                {
                    b.Property<long>("SaleItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("SaleItemId"));

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Count")
                        .HasColumnType("int");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.Property<int>("SaleId")
                        .HasColumnType("int");

                    b.Property<long>("UnitPrice")
                        .HasColumnType("bigint");

                    b.HasKey("SaleItemId");

                    b.HasIndex("MaterialId");

                    b.HasIndex("SaleId");

                    b.ToTable("SaleItems");
                });

            modelBuilder.Entity("Zenith.Models.Site", b =>
                {
                    b.Property<int>("SiteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SiteId"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<short>("CompanyId")
                        .HasColumnType("smallint");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("SiteId");

                    b.HasIndex("CompanyId");

                    b.ToTable("Sites");
                });

            modelBuilder.Entity("Zenith.Models.User", b =>
                {
                    b.Property<string>("Username")
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<string>("HashedPassword")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Username");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Username = "admin",
                            CreateDateTime = new DateTime(2023, 9, 8, 0, 0, 0, 0, DateTimeKind.Local),
                            HasErrors = false,
                            HashedPassword = "b9bcda38c0de9edcda3b12bc5d91de5959e2de031a1fcc13a3860d9c39eeb3b2"
                        });
                });

            modelBuilder.Entity("Zenith.Models.UserPermission", b =>
                {
                    b.Property<int>("UserPermissionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserPermissionId"));

                    b.Property<int>("AccessLevel")
                        .HasColumnType("int");

                    b.Property<bool>("HasErrors")
                        .HasColumnType("bit");

                    b.Property<int>("PermissionType")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nvarchar(64)");

                    b.HasKey("UserPermissionId");

                    b.HasIndex("Username");

                    b.ToTable("UserPermissions");
                });

            modelBuilder.Entity("Zenith.Models.Buy", b =>
                {
                    b.HasOne("Zenith.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Zenith.Models.BuyItem", b =>
                {
                    b.HasOne("Zenith.Models.Buy", "Buy")
                        .WithMany("Items")
                        .HasForeignKey("BuyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Zenith.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Buy");

                    b.Navigation("Material");
                });

            modelBuilder.Entity("Zenith.Models.Cheque", b =>
                {
                    b.HasOne("Zenith.Models.Sale", "Sale")
                        .WithMany()
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("Zenith.Models.Delivery", b =>
                {
                    b.HasOne("Zenith.Models.Person", "Driver")
                        .WithMany()
                        .HasForeignKey("DriverId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Zenith.Models.Machine", "Machine")
                        .WithMany()
                        .HasForeignKey("MachineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Zenith.Models.SaleItem", "SaleItem")
                        .WithMany("Deliveries")
                        .HasForeignKey("SaleItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Driver");

                    b.Navigation("Machine");

                    b.Navigation("SaleItem");
                });

            modelBuilder.Entity("Zenith.Models.MixtureItem", b =>
                {
                    b.HasOne("Zenith.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Zenith.Models.Mixture", "Mixture")
                        .WithMany("Items")
                        .HasForeignKey("MixtureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("Mixture");
                });

            modelBuilder.Entity("Zenith.Models.Outgo", b =>
                {
                    b.HasOne("Zenith.Models.OutgoCategory", "OutgoCategory")
                        .WithMany()
                        .HasForeignKey("OutgoCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("OutgoCategory");
                });

            modelBuilder.Entity("Zenith.Models.OutgoCategory", b =>
                {
                    b.HasOne("Zenith.Models.OutgoCategory", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentOutgoCategoryId");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("Zenith.Models.PersonnelAbsence", b =>
                {
                    b.HasOne("Zenith.Models.Person", "Person")
                        .WithMany("PersonnelAbsences")
                        .HasForeignKey("PersonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Person");
                });

            modelBuilder.Entity("Zenith.Models.Sale", b =>
                {
                    b.HasOne("Zenith.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Zenith.Models.SaleItem", b =>
                {
                    b.HasOne("Zenith.Models.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Zenith.Models.Sale", "Sale")
                        .WithMany("Items")
                        .HasForeignKey("SaleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("Sale");
                });

            modelBuilder.Entity("Zenith.Models.Site", b =>
                {
                    b.HasOne("Zenith.Models.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Zenith.Models.UserPermission", b =>
                {
                    b.HasOne("Zenith.Models.User", "User")
                        .WithMany("Permissions")
                        .HasForeignKey("Username")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Zenith.Models.Buy", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Zenith.Models.Mixture", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Zenith.Models.Person", b =>
                {
                    b.Navigation("PersonnelAbsences");
                });

            modelBuilder.Entity("Zenith.Models.Sale", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("Zenith.Models.SaleItem", b =>
                {
                    b.Navigation("Deliveries");
                });

            modelBuilder.Entity("Zenith.Models.User", b =>
                {
                    b.Navigation("Permissions");
                });
#pragma warning restore 612, 618
        }
    }
}
