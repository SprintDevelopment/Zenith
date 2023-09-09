using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class ChangesRelatedToMixedMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "BuyPrice",
                table: "Mixtures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "RelatedMaterialId",
                table: "Mixtures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "SalePrice",
                table: "Mixtures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsMixed",
                table: "Materials",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "Mixtures");

            migrationBuilder.DropColumn(
                name: "RelatedMaterialId",
                table: "Mixtures");

            migrationBuilder.DropColumn(
                name: "SalePrice",
                table: "Mixtures");

            migrationBuilder.DropColumn(
                name: "IsMixed",
                table: "Materials");
        }
    }
}
