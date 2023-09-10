using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyPrice",
                table: "Mixtures");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Outgoes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MachineId",
                table: "Outgoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommonSaleUnit",
                table: "Mixtures",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommonBuyUnit",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CommonSaleUnit",
                table: "Materials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 9, 10, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_Outgoes_MachineId",
                table: "Outgoes",
                column: "MachineId");

            migrationBuilder.AddForeignKey(
                name: "FK_Outgoes_Machines_MachineId",
                table: "Outgoes",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "MachineId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outgoes_Machines_MachineId",
                table: "Outgoes");

            migrationBuilder.DropIndex(
                name: "IX_Outgoes_MachineId",
                table: "Outgoes");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Outgoes");

            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "Outgoes");

            migrationBuilder.DropColumn(
                name: "CommonSaleUnit",
                table: "Mixtures");

            migrationBuilder.DropColumn(
                name: "CommonBuyUnit",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "CommonSaleUnit",
                table: "Materials");

            migrationBuilder.AddColumn<long>(
                name: "BuyPrice",
                table: "Mixtures",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 9, 9, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
