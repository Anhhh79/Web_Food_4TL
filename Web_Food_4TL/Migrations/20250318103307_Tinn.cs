using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Food_4TL.Migrations
{
    /// <inheritdoc />
    public partial class Tinn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_NguoiDungId",
                table: "TinNhans",
                column: "NguoiDungId");

            migrationBuilder.AddForeignKey(
                name: "FK_TinNhans_NguoiDungs_NguoiDungId",
                table: "TinNhans",
                column: "NguoiDungId",
                principalTable: "NguoiDungs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TinNhans_NguoiDungs_NguoiDungId",
                table: "TinNhans");

            migrationBuilder.DropIndex(
                name: "IX_TinNhans_NguoiDungId",
                table: "TinNhans");
        }
    }
}
