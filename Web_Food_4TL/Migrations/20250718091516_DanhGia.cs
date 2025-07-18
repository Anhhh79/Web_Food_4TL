using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Food_4TL.Migrations
{
    /// <inheritdoc />
    public partial class DanhGia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LyDoTuChoi",
                table: "HoaDons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Lydo",
                table: "HoaDons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "NgayNhan",
                table: "HoaDons",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "TrangThaiDonHang",
                table: "HoaDons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TrangThaiGiaoHang",
                table: "HoaDons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "DanhGias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoiDungDanhGia = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SoSao = table.Column<int>(type: "int", nullable: false),
                    NoiDungPhanHoi = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MonAnId = table.Column<int>(type: "int", nullable: false),
                    NguoiDungId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DanhGias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DanhGias_MonAns_MonAnId",
                        column: x => x.MonAnId,
                        principalTable: "MonAns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DanhGias_NguoiDungs_NguoiDungId",
                        column: x => x.NguoiDungId,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_MonAnId",
                table: "DanhGias",
                column: "MonAnId");

            migrationBuilder.CreateIndex(
                name: "IX_DanhGias_NguoiDungId",
                table: "DanhGias",
                column: "NguoiDungId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DanhGias");

            migrationBuilder.DropColumn(
                name: "LyDoTuChoi",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "Lydo",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "NgayNhan",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "TrangThaiDonHang",
                table: "HoaDons");

            migrationBuilder.DropColumn(
                name: "TrangThaiGiaoHang",
                table: "HoaDons");
        }
    }
}
