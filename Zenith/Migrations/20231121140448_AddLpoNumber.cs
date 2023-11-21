using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class AddLpoNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LpoNumber",
                table: "Deliveries",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 11, 21, 0, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LpoNumber",
                table: "Deliveries");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 11, 9, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
