using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class ChangesMachinesAndDeliveries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "OwnerCompanyId",
                table: "Machines",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelatedTaxiMachineOutgoId",
                table: "Deliveries",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 10, 31, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_Machines_OwnerCompanyId",
                table: "Machines",
                column: "OwnerCompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Machines_Companies_OwnerCompanyId",
                table: "Machines",
                column: "OwnerCompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Machines_Companies_OwnerCompanyId",
                table: "Machines");

            migrationBuilder.DropIndex(
                name: "IX_Machines_OwnerCompanyId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "OwnerCompanyId",
                table: "Machines");

            migrationBuilder.DropColumn(
                name: "RelatedTaxiMachineOutgoId",
                table: "Deliveries");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 10, 25, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
