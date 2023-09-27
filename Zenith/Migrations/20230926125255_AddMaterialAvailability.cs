using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class AddMaterialAvailability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MaterialAvailabilities",
                columns: table => new
                {
                    MaterialAvailabilityId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    BuyPrice = table.Column<float>(type: "real", nullable: false),
                    TotalBoughtCount = table.Column<float>(type: "real", nullable: false),
                    AvailableCount = table.Column<float>(type: "real", nullable: false),
                    HasErrors = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaterialAvailabilities", x => x.MaterialAvailabilityId);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 9, 26, 0, 0, 0, 0, DateTimeKind.Local));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MaterialAvailabilities");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 9, 23, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
