using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyInitialCreditValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "InitialCreditValue",
                table: "Companies",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 11, 9, 0, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialCreditValue",
                table: "Companies");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 11, 4, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
