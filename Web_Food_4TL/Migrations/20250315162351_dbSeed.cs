using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Web_Food_4TL.Migrations
{
    /// <inheritdoc />
    public partial class dbSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VaiTros",
                columns: new[] { "Id", "TenVaiTro" },
                values: new object[,]
                {
                    { 1, "Khách Hàng" },
                    { 2, "Quản lý" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VaiTros",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "VaiTros",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
