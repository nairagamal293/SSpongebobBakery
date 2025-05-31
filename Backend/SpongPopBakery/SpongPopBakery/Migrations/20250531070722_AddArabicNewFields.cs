using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpongPopBakery.Migrations
{
    /// <inheritdoc />
    public partial class AddArabicNewFields : Migration
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
                values: new object[] { new DateTime(2025, 5, 31, 7, 7, 22, 69, DateTimeKind.Utc).AddTicks(5732), new byte[] { 154, 151, 52, 85, 73, 245, 48, 178, 161, 158, 226, 76, 62, 96, 25, 111, 234, 161, 106, 174, 223, 110, 164, 224, 157, 87, 197, 169, 249, 172, 76, 16, 184, 199, 105, 30, 146, 155, 96, 250, 40, 41, 98, 126, 134, 92, 235, 146, 251, 19, 137, 144, 241, 166, 184, 97, 143, 159, 182, 126, 49, 214, 59, 177 }, new byte[] { 165, 129, 120, 112, 16, 120, 30, 233, 194, 153, 161, 160, 64, 144, 88, 161, 30, 227, 178, 127, 142, 236, 170, 205, 84, 204, 203, 87, 67, 229, 65, 133, 223, 1, 171, 82, 121, 240, 221, 64, 191, 135, 87, 84, 211, 228, 107, 87, 142, 78, 244, 36, 68, 66, 223, 217, 216, 79, 97, 144, 26, 255, 38, 85, 236, 97, 244, 144, 246, 219, 82, 41, 152, 113, 160, 21, 39, 44, 74, 128, 63, 28, 70, 225, 4, 73, 133, 158, 77, 136, 23, 67, 72, 229, 201, 242, 252, 197, 8, 52, 131, 241, 88, 12, 204, 2, 43, 27, 1, 7, 216, 209, 99, 247, 94, 39, 190, 98, 61, 49, 115, 16, 28, 8, 251, 36, 210, 158 } });
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
                values: new object[] { new DateTime(2025, 5, 31, 6, 55, 32, 614, DateTimeKind.Utc).AddTicks(697), new byte[] { 115, 220, 174, 155, 133, 49, 31, 198, 243, 28, 157, 65, 201, 238, 246, 206, 233, 193, 11, 138, 245, 66, 232, 202, 76, 246, 223, 94, 201, 108, 166, 120, 203, 137, 88, 125, 215, 94, 187, 248, 210, 150, 118, 87, 130, 252, 103, 212, 233, 160, 164, 166, 183, 74, 158, 109, 67, 164, 159, 172, 63, 150, 190, 55 }, new byte[] { 163, 209, 171, 101, 184, 236, 198, 39, 80, 205, 55, 194, 132, 193, 213, 4, 80, 229, 76, 135, 75, 207, 249, 177, 64, 244, 36, 159, 221, 48, 135, 110, 119, 53, 253, 210, 120, 14, 103, 64, 252, 153, 4, 206, 166, 100, 24, 37, 28, 118, 227, 212, 246, 175, 187, 236, 111, 210, 62, 173, 3, 166, 19, 242, 31, 110, 62, 117, 239, 51, 40, 130, 214, 162, 7, 249, 39, 24, 206, 190, 46, 15, 146, 212, 6, 229, 201, 193, 79, 217, 33, 112, 76, 69, 196, 214, 201, 35, 39, 210, 20, 1, 130, 255, 55, 118, 55, 224, 185, 98, 85, 227, 164, 141, 255, 99, 175, 185, 64, 161, 213, 108, 229, 42, 129, 220, 103, 3 } });
        }
    }
}
