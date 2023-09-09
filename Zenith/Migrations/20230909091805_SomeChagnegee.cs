using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class SomeChagnegee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "CompanyId",
                table: "Outgoes",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<long>(
                name: "CreditValue",
                table: "Companies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Outgoes_CompanyId",
                table: "Outgoes",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Outgoes_Companies_CompanyId",
                table: "Outgoes",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outgoes_Companies_CompanyId",
                table: "Outgoes");

            migrationBuilder.DropIndex(
                name: "IX_Outgoes_CompanyId",
                table: "Outgoes");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Outgoes");

            migrationBuilder.DropColumn(
                name: "CreditValue",
                table: "Companies");
        }
    }
}
