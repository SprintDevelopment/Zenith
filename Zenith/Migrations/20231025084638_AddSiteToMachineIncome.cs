using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteToMachineIncome : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "MachineIncomes",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 10, 25, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_MachineIncomes_SiteId",
                table: "MachineIncomes",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_MachineIncomes_Sites_SiteId",
                table: "MachineIncomes",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "SiteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MachineIncomes_Sites_SiteId",
                table: "MachineIncomes");

            migrationBuilder.DropIndex(
                name: "IX_MachineIncomes_SiteId",
                table: "MachineIncomes");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "MachineIncomes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 10, 23, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
