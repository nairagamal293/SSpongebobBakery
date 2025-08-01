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
    [Migration("20250531070722_AddArabicNewFields")]
    partial class AddArabicNewFields
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

                    b.Property<string>("DescriptionAr")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameAr")
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

                    b.Property<string>("DescriptionAr")
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

                    b.Property<string>("NameAr")
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
                            CreatedAt = new DateTime(2025, 5, 31, 7, 7, 22, 69, DateTimeKind.Utc).AddTicks(5732),
                            Email = "admin@bakery.com",
                            PasswordHash = new byte[] { 154, 151, 52, 85, 73, 245, 48, 178, 161, 158, 226, 76, 62, 96, 25, 111, 234, 161, 106, 174, 223, 110, 164, 224, 157, 87, 197, 169, 249, 172, 76, 16, 184, 199, 105, 30, 146, 155, 96, 250, 40, 41, 98, 126, 134, 92, 235, 146, 251, 19, 137, 144, 241, 166, 184, 97, 143, 159, 182, 126, 49, 214, 59, 177 },
                            PasswordSalt = new byte[] { 165, 129, 120, 112, 16, 120, 30, 233, 194, 153, 161, 160, 64, 144, 88, 161, 30, 227, 178, 127, 142, 236, 170, 205, 84, 204, 203, 87, 67, 229, 65, 133, 223, 1, 171, 82, 121, 240, 221, 64, 191, 135, 87, 84, 211, 228, 107, 87, 142, 78, 244, 36, 68, 66, 223, 217, 216, 79, 97, 144, 26, 255, 38, 85, 236, 97, 244, 144, 246, 219, 82, 41, 152, 113, 160, 21, 39, 44, 74, 128, 63, 28, 70, 225, 4, 73, 133, 158, 77, 136, 23, 67, 72, 229, 201, 242, 252, 197, 8, 52, 131, 241, 88, 12, 204, 2, 43, 27, 1, 7, 216, 209, 99, 247, 94, 39, 190, 98, 61, 49, 115, 16, 28, 8, 251, 36, 210, 158 },
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
