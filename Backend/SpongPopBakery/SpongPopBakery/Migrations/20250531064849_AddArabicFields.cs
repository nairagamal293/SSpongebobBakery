using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpongPopBakery.Migrations
{
    /// <inheritdoc />
    public partial class AddArabicFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionAr",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NameAr",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 5, 31, 6, 48, 48, 286, DateTimeKind.Utc).AddTicks(5389), new byte[] { 164, 188, 85, 79, 79, 70, 216, 156, 167, 75, 236, 189, 44, 89, 162, 229, 42, 105, 51, 227, 190, 73, 222, 219, 249, 74, 45, 8, 242, 134, 161, 70, 135, 158, 131, 76, 204, 227, 161, 40, 16, 80, 231, 209, 224, 183, 222, 224, 178, 79, 46, 168, 77, 33, 91, 145, 137, 24, 67, 97, 140, 8, 95, 197 }, new byte[] { 52, 35, 237, 252, 233, 218, 223, 104, 2, 52, 247, 35, 238, 122, 198, 82, 123, 190, 55, 112, 153, 138, 143, 36, 179, 114, 124, 187, 4, 241, 66, 130, 176, 29, 64, 125, 207, 214, 72, 234, 6, 225, 243, 55, 72, 150, 43, 38, 105, 61, 130, 83, 4, 115, 250, 34, 25, 240, 138, 219, 150, 106, 42, 87, 177, 178, 41, 58, 171, 188, 227, 112, 136, 63, 225, 238, 189, 97, 90, 130, 182, 127, 120, 95, 231, 87, 47, 201, 107, 101, 55, 202, 91, 47, 77, 172, 125, 90, 40, 171, 146, 11, 223, 159, 246, 124, 188, 51, 211, 105, 15, 202, 48, 146, 27, 203, 228, 78, 98, 60, 142, 174, 165, 5, 135, 251, 216, 100 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DescriptionAr",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "NameAr",
                table: "Categories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 5, 28, 7, 14, 20, 283, DateTimeKind.Utc).AddTicks(6480), new byte[] { 232, 192, 100, 132, 172, 211, 12, 109, 239, 63, 146, 22, 178, 25, 37, 81, 88, 147, 35, 255, 56, 138, 144, 182, 234, 233, 228, 28, 27, 105, 106, 22, 196, 127, 31, 246, 225, 143, 239, 66, 248, 88, 11, 91, 32, 232, 36, 23, 164, 119, 80, 147, 206, 172, 250, 197, 51, 119, 128, 211, 59, 232, 72, 68 }, new byte[] { 55, 77, 30, 138, 94, 248, 240, 125, 193, 227, 202, 219, 239, 9, 42, 83, 251, 156, 42, 19, 14, 26, 65, 111, 120, 32, 46, 103, 17, 153, 124, 68, 175, 253, 88, 189, 82, 16, 96, 47, 250, 124, 102, 213, 173, 161, 205, 37, 45, 225, 251, 89, 193, 81, 183, 212, 132, 189, 193, 77, 212, 81, 97, 217, 61, 136, 144, 9, 107, 120, 174, 80, 54, 121, 15, 253, 250, 190, 185, 173, 131, 138, 41, 184, 251, 41, 19, 2, 146, 68, 180, 98, 114, 250, 206, 61, 128, 68, 17, 122, 112, 56, 136, 107, 23, 174, 224, 1, 197, 108, 199, 10, 1, 18, 231, 140, 225, 49, 96, 153, 245, 213, 58, 63, 120, 115, 196, 34 } });
        }
    }
}
