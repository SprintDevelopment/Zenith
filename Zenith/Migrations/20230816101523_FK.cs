using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class FK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_OutgoCategories_ParentOutgoCategoryId",
                table: "OutgoCategories",
                column: "ParentOutgoCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoCategories_OutgoCategories_ParentOutgoCategoryId",
                table: "OutgoCategories",
                column: "ParentOutgoCategoryId",
                principalTable: "OutgoCategories",
                principalColumn: "OutgoCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutgoCategories_OutgoCategories_ParentOutgoCategoryId",
                table: "OutgoCategories");

            migrationBuilder.DropIndex(
                name: "IX_OutgoCategories_ParentOutgoCategoryId",
                table: "OutgoCategories");
        }
    }
}
