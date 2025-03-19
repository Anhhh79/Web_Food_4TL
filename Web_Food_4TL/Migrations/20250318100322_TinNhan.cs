using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Food_4TL.Migrations
{
    /// <inheritdoc />
    public partial class TinNhan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TinNhans_NguoiDungs_ReceiverId",
                table: "TinNhans");

            migrationBuilder.DropForeignKey(
                name: "FK_TinNhans_NguoiDungs_SenderId",
                table: "TinNhans");

            migrationBuilder.DropIndex(
                name: "IX_TinNhans_ReceiverId",
                table: "TinNhans");

            migrationBuilder.DropIndex(
                name: "IX_TinNhans_SenderId",
                table: "TinNhans");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "TinNhans");

            migrationBuilder.DropColumn(
                name: "SenderId",
                table: "TinNhans");

            migrationBuilder.RenameColumn(
                name: "Timestamp",
                table: "TinNhans",
                newName: "ThoiGianGui");

            migrationBuilder.RenameColumn(
                name: "MessageText",
                table: "TinNhans",
                newName: "NoiDung");

            migrationBuilder.RenameColumn(
                name: "IsFromAI",
                table: "TinNhans",
                newName: "LaTinNhanTuKhach");

            migrationBuilder.AddColumn<string>(
                name: "NguoiDungId",
                table: "TinNhans",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NguoiDungId",
                table: "TinNhans");

            migrationBuilder.RenameColumn(
                name: "ThoiGianGui",
                table: "TinNhans",
                newName: "Timestamp");

            migrationBuilder.RenameColumn(
                name: "NoiDung",
                table: "TinNhans",
                newName: "MessageText");

            migrationBuilder.RenameColumn(
                name: "LaTinNhanTuKhach",
                table: "TinNhans",
                newName: "IsFromAI");

            migrationBuilder.AddColumn<int>(
                name: "ReceiverId",
                table: "TinNhans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SenderId",
                table: "TinNhans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_ReceiverId",
                table: "TinNhans",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_SenderId",
                table: "TinNhans",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_TinNhans_NguoiDungs_ReceiverId",
                table: "TinNhans",
                column: "ReceiverId",
                principalTable: "NguoiDungs",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TinNhans_NguoiDungs_SenderId",
                table: "TinNhans",
                column: "SenderId",
                principalTable: "NguoiDungs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
