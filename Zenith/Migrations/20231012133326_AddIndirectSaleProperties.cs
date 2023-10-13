using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class AddIndirectSaleProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "IndirectSellerCompanyId",
                table: "Sales",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIndirectSale",
                table: "Sales",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_IndirectSellerCompanyId",
                table: "Sales",
                column: "IndirectSellerCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Companies_IndirectSellerCompanyId",
                table: "Sales",
                column: "IndirectSellerCompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Companies_IndirectSellerCompanyId",
                table: "Sales");

            migrationBuilder.DropIndex(
                name: "IX_Sales_IndirectSellerCompanyId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "IndirectSellerCompanyId",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "IsIndirectSale",
                table: "Sales");
        }
    }
}
