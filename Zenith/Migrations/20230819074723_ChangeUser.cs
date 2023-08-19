using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class ChangeUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "BuyItem",
                newName: "UnitPrice");

            migrationBuilder.AlterColumn<byte[]>(
                name: "AvatarImageBytes",
                table: "Users",
                type: "Image",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "Image");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "BuyItem",
                newName: "Price");

            migrationBuilder.AlterColumn<byte[]>(
                name: "AvatarImageBytes",
                table: "Users",
                type: "Image",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "Image",
                oldNullable: true);
        }
    }
}
