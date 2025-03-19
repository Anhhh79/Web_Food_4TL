using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Food_4TL.Migrations
{
    /// <inheritdoc />
    public partial class tinnnn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NguoiGuiId",
                table: "TinNhans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NguoiNhanId",
                table: "TinNhans",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_NguoiGuiId",
                table: "TinNhans",
                column: "NguoiGuiId");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_NguoiNhanId",
                table: "TinNhans",
                column: "NguoiNhanId");

            migrationBuilder.AddForeignKey(
                name: "FK_TinNhans_NguoiDungs_NguoiGuiId",
                table: "TinNhans",
                column: "NguoiGuiId",
                principalTable: "NguoiDungs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TinNhans_NguoiDungs_NguoiNhanId",
                table: "TinNhans",
                column: "NguoiNhanId",
                principalTable: "NguoiDungs",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TinNhans_NguoiDungs_NguoiGuiId",
                table: "TinNhans");

            migrationBuilder.DropForeignKey(
                name: "FK_TinNhans_NguoiDungs_NguoiNhanId",
                table: "TinNhans");

            migrationBuilder.DropIndex(
                name: "IX_TinNhans_NguoiGuiId",
                table: "TinNhans");

            migrationBuilder.DropIndex(
                name: "IX_TinNhans_NguoiNhanId",
                table: "TinNhans");

            migrationBuilder.DropColumn(
                name: "NguoiGuiId",
                table: "TinNhans");

            migrationBuilder.DropColumn(
                name: "NguoiNhanId",
                table: "TinNhans");
        }
    }
}
