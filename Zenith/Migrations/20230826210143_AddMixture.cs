using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class AddMixture : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mixtures",
                columns: table => new
                {
                    MixtureId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasErrors = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mixtures", x => x.MixtureId);
                });

            migrationBuilder.CreateTable(
                name: "MixtureItems",
                columns: table => new
                {
                    MixtureItemId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MixtureId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false),
                    Percent = table.Column<short>(type: "smallint", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HasErrors = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MixtureItems", x => x.MixtureItemId);
                    table.ForeignKey(
                        name: "FK_MixtureItems_Materials_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materials",
                        principalColumn: "MaterialId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MixtureItems_Mixtures_MixtureId",
                        column: x => x.MixtureId,
                        principalTable: "Mixtures",
                        principalColumn: "MixtureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 8, 27, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_MixtureItems_MaterialId",
                table: "MixtureItems",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_MixtureItems_MixtureId",
                table: "MixtureItems",
                column: "MixtureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MixtureItems");

            migrationBuilder.DropTable(
                name: "Mixtures");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 8, 26, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
