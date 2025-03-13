using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Web_Food_4TL.Migrations
{
    /// <inheritdoc />
    public partial class colSDT : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SoDienThoai",
                table: "HoaDons",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SoDienThoai",
                table: "HoaDons");
        }
    }
}
