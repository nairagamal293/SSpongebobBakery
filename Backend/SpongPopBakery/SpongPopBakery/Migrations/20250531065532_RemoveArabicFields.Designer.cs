﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpongPopBakery.Data;

#nullable disable

namespace SpongPopBakery.Migrations
{
    [DbContext(typeof(BakeryDbContext))]
    [Migration("20250531065532_RemoveArabicFields")]
    partial class RemoveArabicFields
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SpongPopBakery.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("SpongPopBakery.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SpongPopBakery.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2025, 5, 31, 6, 55, 32, 614, DateTimeKind.Utc).AddTicks(697),
                            Email = "admin@bakery.com",
                            PasswordHash = new byte[] { 115, 220, 174, 155, 133, 49, 31, 198, 243, 28, 157, 65, 201, 238, 246, 206, 233, 193, 11, 138, 245, 66, 232, 202, 76, 246, 223, 94, 201, 108, 166, 120, 203, 137, 88, 125, 215, 94, 187, 248, 210, 150, 118, 87, 130, 252, 103, 212, 233, 160, 164, 166, 183, 74, 158, 109, 67, 164, 159, 172, 63, 150, 190, 55 },
                            PasswordSalt = new byte[] { 163, 209, 171, 101, 184, 236, 198, 39, 80, 205, 55, 194, 132, 193, 213, 4, 80, 229, 76, 135, 75, 207, 249, 177, 64, 244, 36, 159, 221, 48, 135, 110, 119, 53, 253, 210, 120, 14, 103, 64, 252, 153, 4, 206, 166, 100, 24, 37, 28, 118, 227, 212, 246, 175, 187, 236, 111, 210, 62, 173, 3, 166, 19, 242, 31, 110, 62, 117, 239, 51, 40, 130, 214, 162, 7, 249, 39, 24, 206, 190, 46, 15, 146, 212, 6, 229, 201, 193, 79, 217, 33, 112, 76, 69, 196, 214, 201, 35, 39, 210, 20, 1, 130, 255, 55, 118, 55, 224, 185, 98, 85, 227, 164, 141, 255, 99, 175, 185, 64, 161, 213, 108, 229, 42, 129, 220, 103, 3 },
                            Role = "Admin",
                            Username = "admin"
                        });
                });

            modelBuilder.Entity("SpongPopBakery.Models.Product", b =>
                {
                    b.HasOne("SpongPopBakery.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}
