using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpongPopBakery.Migrations
{
    /// <inheritdoc />
    public partial class AddContactMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 6, 3, 13, 36, 5, 837, DateTimeKind.Utc).AddTicks(3677), new byte[] { 53, 151, 47, 128, 53, 190, 78, 197, 243, 202, 5, 134, 43, 205, 93, 134, 74, 44, 63, 204, 28, 77, 94, 76, 188, 74, 32, 57, 211, 212, 194, 42, 188, 15, 252, 57, 127, 196, 145, 71, 216, 209, 191, 125, 155, 22, 99, 32, 110, 135, 130, 243, 161, 182, 139, 174, 186, 159, 90, 87, 48, 152, 103, 187 }, new byte[] { 97, 71, 158, 170, 211, 51, 147, 123, 247, 26, 146, 22, 158, 3, 68, 27, 45, 43, 53, 78, 33, 30, 146, 174, 1, 200, 109, 137, 29, 108, 103, 56, 54, 187, 176, 18, 159, 3, 236, 230, 213, 136, 65, 76, 249, 95, 226, 190, 24, 56, 129, 40, 35, 51, 75, 45, 80, 72, 22, 86, 93, 72, 66, 203, 194, 48, 204, 122, 110, 204, 196, 29, 241, 106, 194, 57, 128, 114, 82, 101, 178, 194, 142, 133, 247, 98, 57, 127, 50, 100, 100, 33, 206, 29, 129, 131, 139, 58, 247, 208, 161, 52, 241, 231, 216, 145, 201, 213, 191, 128, 17, 119, 32, 78, 36, 112, 214, 190, 183, 168, 38, 225, 189, 38, 205, 207, 3, 47 } });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 5, 31, 12, 12, 48, 406, DateTimeKind.Utc).AddTicks(3152), new byte[] { 249, 163, 3, 10, 234, 130, 195, 152, 254, 63, 108, 163, 103, 66, 63, 82, 150, 36, 222, 223, 166, 62, 108, 213, 183, 5, 97, 247, 215, 73, 156, 73, 254, 172, 46, 111, 222, 160, 206, 158, 91, 89, 189, 73, 123, 115, 112, 57, 7, 58, 69, 125, 205, 187, 71, 106, 79, 42, 7, 165, 220, 252, 128, 110 }, new byte[] { 174, 88, 47, 102, 203, 37, 203, 233, 211, 209, 36, 35, 12, 25, 77, 206, 253, 135, 34, 129, 222, 62, 169, 151, 122, 14, 200, 91, 19, 5, 178, 54, 22, 230, 155, 188, 110, 206, 251, 166, 233, 141, 131, 111, 160, 134, 231, 121, 55, 238, 57, 90, 21, 136, 109, 66, 55, 44, 213, 8, 93, 63, 176, 6, 252, 245, 164, 221, 165, 20, 29, 131, 18, 83, 165, 26, 51, 160, 195, 233, 86, 245, 47, 240, 254, 29, 11, 17, 242, 53, 5, 150, 195, 197, 102, 98, 116, 6, 209, 111, 191, 103, 211, 144, 194, 85, 105, 13, 149, 106, 76, 76, 148, 117, 166, 254, 63, 226, 20, 199, 155, 144, 64, 149, 134, 157, 74, 55 } });
        }
    }
}
