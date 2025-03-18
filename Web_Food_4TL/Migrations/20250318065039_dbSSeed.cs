using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Web_Food_4TL.Migrations
{
    /// <inheritdoc />
    public partial class dbSSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DanhMucs",
                keyColumn: "Id",
                keyValue: 1,
                column: "TenDanhMuc",
                value: "Starters");

            migrationBuilder.InsertData(
                table: "DanhMucs",
                columns: new[] { "Id", "TenDanhMuc" },
                values: new object[,]
                {
                    { 2, "Breakfast" },
                    { 3, "Lunch" },
                    { 4, "Dinner" }
                });

            migrationBuilder.InsertData(
                table: "MonAns",
                columns: new[] { "Id", "DanhMucId", "Gia", "MoTa", "TenMonAn" },
                values: new object[,]
                {
                    { 2, 2, 35000m, "Ngon", "Xôi" },
                    { 3, 3, 35000m, "Ngon", "Cơm xào bò" },
                    { 4, 4, 35000m, "Ngon", "Cơm gà" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MonAns",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MonAns",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MonAns",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DanhMucs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DanhMucs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DanhMucs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "DanhMucs",
                keyColumn: "Id",
                keyValue: 1,
                column: "TenDanhMuc",
                value: "Breakfast");
        }
    }
}
