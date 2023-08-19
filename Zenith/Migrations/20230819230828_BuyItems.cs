using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class BuyItems : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyItem_Buys_BuyId",
                table: "BuyItem");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItem_Materials_MaterialId",
                table: "BuyItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BuyItem",
                table: "BuyItem");

            migrationBuilder.RenameTable(
                name: "BuyItem",
                newName: "BuyItems");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItem_MaterialId",
                table: "BuyItems",
                newName: "IX_BuyItems_MaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItem_BuyId",
                table: "BuyItems",
                newName: "IX_BuyItems_BuyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BuyItems",
                table: "BuyItems",
                column: "BuyItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItems_Buys_BuyId",
                table: "BuyItems",
                column: "BuyId",
                principalTable: "Buys",
                principalColumn: "BuyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItems_Materials_MaterialId",
                table: "BuyItems",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BuyItems_Buys_BuyId",
                table: "BuyItems");

            migrationBuilder.DropForeignKey(
                name: "FK_BuyItems_Materials_MaterialId",
                table: "BuyItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BuyItems",
                table: "BuyItems");

            migrationBuilder.RenameTable(
                name: "BuyItems",
                newName: "BuyItem");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItems_MaterialId",
                table: "BuyItem",
                newName: "IX_BuyItem_MaterialId");

            migrationBuilder.RenameIndex(
                name: "IX_BuyItems_BuyId",
                table: "BuyItem",
                newName: "IX_BuyItem_BuyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BuyItem",
                table: "BuyItem",
                column: "BuyItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItem_Buys_BuyId",
                table: "BuyItem",
                column: "BuyId",
                principalTable: "Buys",
                principalColumn: "BuyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BuyItem_Materials_MaterialId",
                table: "BuyItem",
                column: "MaterialId",
                principalTable: "Materials",
                principalColumn: "MaterialId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
