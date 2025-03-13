using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Food_4TL.Migrations
{
    /// <inheritdoc />
    public partial class editHoadonchitiet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonChiTiets_HoaDons_NguoiDungId",
                table: "HoaDonChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonChiTiets_NguoiDungId",
                table: "HoaDonChiTiets");

            migrationBuilder.DropColumn(
                name: "NguoiDungId",
                table: "HoaDonChiTiets");

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiets_HoaDonId",
                table: "HoaDonChiTiets",
                column: "HoaDonId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonChiTiets_HoaDons_HoaDonId",
                table: "HoaDonChiTiets",
                column: "HoaDonId",
                principalTable: "HoaDons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HoaDonChiTiets_HoaDons_HoaDonId",
                table: "HoaDonChiTiets");

            migrationBuilder.DropIndex(
                name: "IX_HoaDonChiTiets_HoaDonId",
                table: "HoaDonChiTiets");

            migrationBuilder.AddColumn<int>(
                name: "NguoiDungId",
                table: "HoaDonChiTiets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_HoaDonChiTiets_NguoiDungId",
                table: "HoaDonChiTiets",
                column: "NguoiDungId");

            migrationBuilder.AddForeignKey(
                name: "FK_HoaDonChiTiets_HoaDons_NguoiDungId",
                table: "HoaDonChiTiets",
                column: "NguoiDungId",
                principalTable: "HoaDons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
