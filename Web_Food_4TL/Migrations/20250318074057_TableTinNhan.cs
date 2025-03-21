using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Food_4TL.Migrations
{
    /// <inheritdoc />
    public partial class TableTinNhan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TinNhans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsFromAI = table.Column<bool>(type: "bit", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    ReceiverId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TinNhans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TinNhans_NguoiDungs_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TinNhans_NguoiDungs_SenderId",
                        column: x => x.SenderId,
                        principalTable: "NguoiDungs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_ReceiverId",
                table: "TinNhans",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_TinNhans_SenderId",
                table: "TinNhans",
                column: "SenderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TinNhans");
        }
    }
}
