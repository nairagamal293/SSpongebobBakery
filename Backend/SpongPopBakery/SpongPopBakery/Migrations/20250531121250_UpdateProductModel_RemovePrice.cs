using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SpongPopBakery.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductModel_RemovePrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "ProductSize",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameAr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSize", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSize_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 5, 31, 12, 12, 48, 406, DateTimeKind.Utc).AddTicks(3152), new byte[] { 249, 163, 3, 10, 234, 130, 195, 152, 254, 63, 108, 163, 103, 66, 63, 82, 150, 36, 222, 223, 166, 62, 108, 213, 183, 5, 97, 247, 215, 73, 156, 73, 254, 172, 46, 111, 222, 160, 206, 158, 91, 89, 189, 73, 123, 115, 112, 57, 7, 58, 69, 125, 205, 187, 71, 106, 79, 42, 7, 165, 220, 252, 128, 110 }, new byte[] { 174, 88, 47, 102, 203, 37, 203, 233, 211, 209, 36, 35, 12, 25, 77, 206, 253, 135, 34, 129, 222, 62, 169, 151, 122, 14, 200, 91, 19, 5, 178, 54, 22, 230, 155, 188, 110, 206, 251, 166, 233, 141, 131, 111, 160, 134, 231, 121, 55, 238, 57, 90, 21, 136, 109, 66, 55, 44, 213, 8, 93, 63, 176, 6, 252, 245, 164, 221, 165, 20, 29, 131, 18, 83, 165, 26, 51, 160, 195, 233, 86, 245, 47, 240, 254, 29, 11, 17, 242, 53, 5, 150, 195, 197, 102, 98, 116, 6, 209, 111, 191, 103, 211, 144, 194, 85, 105, 13, 149, 106, 76, 76, 148, 117, 166, 254, 63, 226, 20, 199, 155, 144, 64, 149, 134, 157, 74, 55 } });

            migrationBuilder.CreateIndex(
                name: "IX_ProductSize_ProductId",
                table: "ProductSize",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductSize");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2025, 5, 31, 7, 7, 22, 69, DateTimeKind.Utc).AddTicks(5732), new byte[] { 154, 151, 52, 85, 73, 245, 48, 178, 161, 158, 226, 76, 62, 96, 25, 111, 234, 161, 106, 174, 223, 110, 164, 224, 157, 87, 197, 169, 249, 172, 76, 16, 184, 199, 105, 30, 146, 155, 96, 250, 40, 41, 98, 126, 134, 92, 235, 146, 251, 19, 137, 144, 241, 166, 184, 97, 143, 159, 182, 126, 49, 214, 59, 177 }, new byte[] { 165, 129, 120, 112, 16, 120, 30, 233, 194, 153, 161, 160, 64, 144, 88, 161, 30, 227, 178, 127, 142, 236, 170, 205, 84, 204, 203, 87, 67, 229, 65, 133, 223, 1, 171, 82, 121, 240, 221, 64, 191, 135, 87, 84, 211, 228, 107, 87, 142, 78, 244, 36, 68, 66, 223, 217, 216, 79, 97, 144, 26, 255, 38, 85, 236, 97, 244, 144, 246, 219, 82, 41, 152, 113, 160, 21, 39, 44, 74, 128, 63, 28, 70, 225, 4, 73, 133, 158, 77, 136, 23, 67, 72, 229, 201, 242, 252, 197, 8, 52, 131, 241, 88, 12, 204, 2, 43, 27, 1, 7, 216, 209, 99, 247, 94, 39, 190, 98, 61, 49, 115, 16, 28, 8, 251, 36, 210, 158 } });
        }
    }
}
