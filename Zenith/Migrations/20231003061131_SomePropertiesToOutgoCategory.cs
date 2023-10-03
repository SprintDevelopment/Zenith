using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class SomePropertiesToOutgoCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CountUnitTitle",
                table: "OutgoCategories",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "ReservedAmount",
                table: "OutgoCategories",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "AccountId", "Balance", "ChequeValue", "Comment", "CostCenter", "CreditValue", "HasErrors", "Name" },
                values: new object[] { (short)3, 0f, 0f, "", 1, 0f, false, "Consumables Account" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 10, 3, 0, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Accounts",
                keyColumn: "AccountId",
                keyValue: (short)3);

            migrationBuilder.DropColumn(
                name: "CountUnitTitle",
                table: "OutgoCategories");

            migrationBuilder.DropColumn(
                name: "ReservedAmount",
                table: "OutgoCategories");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 9, 28, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
