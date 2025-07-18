using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Food_4TL.Migrations
{
    /// <inheritdoc />
    public partial class dbbb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VaiTros",
                keyColumn: "Id",
                keyValue: 2,
                column: "TenVaiTro",
                value: "Quản Lý");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "VaiTros",
                keyColumn: "Id",
                keyValue: 2,
                column: "TenVaiTro",
                value: "Quản lý");
        }
    }
}
