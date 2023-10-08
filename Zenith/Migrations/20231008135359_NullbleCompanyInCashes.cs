using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class NullbleCompanyInCashes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cashes_Companies_CompanyId",
                table: "Cashes");

            migrationBuilder.DropForeignKey(
                name: "FK_Cheques_Companies_CompanyId",
                table: "Cheques");

            migrationBuilder.AlterColumn<short>(
                name: "CompanyId",
                table: "Cheques",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AlterColumn<short>(
                name: "CompanyId",
                table: "Cashes",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.AddForeignKey(
                name: "FK_Cashes_Companies_CompanyId",
                table: "Cashes",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheques_Companies_CompanyId",
                table: "Cheques",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cashes_Companies_CompanyId",
                table: "Cashes");

            migrationBuilder.DropForeignKey(
                name: "FK_Cheques_Companies_CompanyId",
                table: "Cheques");

            migrationBuilder.AlterColumn<short>(
                name: "CompanyId",
                table: "Cheques",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AlterColumn<short>(
                name: "CompanyId",
                table: "Cashes",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Cashes_Companies_CompanyId",
                table: "Cashes",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cheques_Companies_CompanyId",
                table: "Cheques",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
