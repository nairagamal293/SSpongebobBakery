using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpongPopBakery.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "PasswordSalt", "Role", "Username" },
                values: new object[] { 1, new DateTime(2025, 5, 28, 7, 14, 20, 283, DateTimeKind.Utc).AddTicks(6480), "admin@bakery.com", new byte[] { 232, 192, 100, 132, 172, 211, 12, 109, 239, 63, 146, 22, 178, 25, 37, 81, 88, 147, 35, 255, 56, 138, 144, 182, 234, 233, 228, 28, 27, 105, 106, 22, 196, 127, 31, 246, 225, 143, 239, 66, 248, 88, 11, 91, 32, 232, 36, 23, 164, 119, 80, 147, 206, 172, 250, 197, 51, 119, 128, 211, 59, 232, 72, 68 }, new byte[] { 55, 77, 30, 138, 94, 248, 240, 125, 193, 227, 202, 219, 239, 9, 42, 83, 251, 156, 42, 19, 14, 26, 65, 111, 120, 32, 46, 103, 17, 153, 124, 68, 175, 253, 88, 189, 82, 16, 96, 47, 250, 124, 102, 213, 173, 161, 205, 37, 45, 225, 251, 89, 193, 81, 183, 212, 132, 189, 193, 77, 212, 81, 97, 217, 61, 136, 144, 9, 107, 120, 174, 80, 54, 121, 15, 253, 250, 190, 185, 173, 131, 138, 41, 184, 251, 41, 19, 2, 146, 68, 180, 98, 114, 250, 206, 61, 128, 68, 17, 122, 112, 56, 136, 107, 23, 174, 224, 1, 197, 108, 199, 10, 1, 18, 231, 140, 225, 49, 96, 153, 245, 213, 58, 63, 120, 115, 196, 34 }, "Admin", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
