using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheques_Sales_SaleId",
                table: "Cheques");

            migrationBuilder.DropIndex(
                name: "IX_Cheques_SaleId",
                table: "Cheques");

            migrationBuilder.RenameColumn(
                name: "SaleId",
                table: "Cheques",
                newName: "TransferDirection");

            migrationBuilder.RenameColumn(
                name: "ChequeValue",
                table: "Cheques",
                newName: "Value");

            migrationBuilder.AddColumn<int>(
                name: "ChequeState",
                table: "Cheques",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<short>(
                name: "CompanyId",
                table: "Cheques",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.CreateTable(
                name: "Cashes",
                columns: table => new
                {
                    CashId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HasErrors = table.Column<bool>(type: "bit", nullable: false),
                    TransferDirection = table.Column<int>(type: "int", nullable: false),
                    CompanyId = table.Column<short>(type: "smallint", nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    IssueDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cashes", x => x.CashId);
                    table.ForeignKey(
                        name: "FK_Cashes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 9, 12, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_CompanyId",
                table: "Cheques",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Cashes_CompanyId",
                table: "Cashes",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheques_Companies_CompanyId",
                table: "Cheques",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cheques_Companies_CompanyId",
                table: "Cheques");

            migrationBuilder.DropTable(
                name: "Cashes");

            migrationBuilder.DropIndex(
                name: "IX_Cheques_CompanyId",
                table: "Cheques");

            migrationBuilder.DropColumn(
                name: "ChequeState",
                table: "Cheques");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Cheques");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "Cheques",
                newName: "ChequeValue");

            migrationBuilder.RenameColumn(
                name: "TransferDirection",
                table: "Cheques",
                newName: "SaleId");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_Cheques_SaleId",
                table: "Cheques",
                column: "SaleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cheques_Sales_SaleId",
                table: "Cheques",
                column: "SaleId",
                principalTable: "Sales",
                principalColumn: "SaleId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
