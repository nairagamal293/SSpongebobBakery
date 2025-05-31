using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpongPopBakery.Migrations
{
    /// <inheritdoc />
    public partial class RemoveArabicFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { new DateTime(2025, 5, 31, 6, 55, 32, 614, DateTimeKind.Utc).AddTicks(697), new byte[] { 115, 220, 174, 155, 133, 49, 31, 198, 243, 28, 157, 65, 201, 238, 246, 206, 233, 193, 11, 138, 245, 66, 232, 202, 76, 246, 223, 94, 201, 108, 166, 120, 203, 137, 88, 125, 215, 94, 187, 248, 210, 150, 118, 87, 130, 252, 103, 212, 233, 160, 164, 166, 183, 74, 158, 109, 67, 164, 159, 172, 63, 150, 190, 55 }, new byte[] { 163, 209, 171, 101, 184, 236, 198, 39, 80, 205, 55, 194, 132, 193, 213, 4, 80, 229, 76, 135, 75, 207, 249, 177, 64, 244, 36, 159, 221, 48, 135, 110, 119, 53, 253, 210, 120, 14, 103, 64, 252, 153, 4, 206, 166, 100, 24, 37, 28, 118, 227, 212, 246, 175, 187, 236, 111, 210, 62, 173, 3, 166, 19, 242, 31, 110, 62, 117, 239, 51, 40, 130, 214, 162, 7, 249, 39, 24, 206, 190, 46, 15, 146, 212, 6, 229, 201, 193, 79, 217, 33, 112, 76, 69, 196, 214, 201, 35, 39, 210, 20, 1, 130, 255, 55, 118, 55, 224, 185, 98, 85, 227, 164, 141, 255, 99, 175, 185, 64, 161, 213, 108, 229, 42, 129, 220, 103, 3 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
