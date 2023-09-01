using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteToDeliveries : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_Deliveries_SiteId",
                table: "Deliveries",
                column: "SiteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deliveries_Sites_SiteId",
                table: "Deliveries",
                column: "SiteId",
                principalTable: "Sites",
                principalColumn: "SiteId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deliveries_Sites_SiteId",
                table: "Deliveries");

            migrationBuilder.DropIndex(
                name: "IX_Deliveries_SiteId",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Deliveries");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 8, 29, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
