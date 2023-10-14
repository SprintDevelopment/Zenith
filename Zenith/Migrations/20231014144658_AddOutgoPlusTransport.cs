using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class AddOutgoPlusTransport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RelatedOutgoPlusTransportId",
                table: "Outgoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RelatedOutgoPlusTransportId",
                table: "MachineIncomes",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 10, 14, 0, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RelatedOutgoPlusTransportId",
                table: "Outgoes");

            migrationBuilder.DropColumn(
                name: "RelatedOutgoPlusTransportId",
                table: "MachineIncomes");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 10, 13, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
