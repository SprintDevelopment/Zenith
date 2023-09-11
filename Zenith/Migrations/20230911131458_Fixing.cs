using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class Fixing : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OutgoCategories_OutgoCategories_ParentOutgoCategoryId",
                table: "OutgoCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_Outgoes_Companies_CompanyId",
                table: "Outgoes");

            migrationBuilder.DropForeignKey(
                name: "FK_Outgoes_Machines_MachineId",
                table: "Outgoes");

            migrationBuilder.DropIndex(
                name: "IX_Outgoes_MachineId",
                table: "Outgoes");

            migrationBuilder.DropIndex(
                name: "IX_OutgoCategories_ParentOutgoCategoryId",
                table: "OutgoCategories");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Outgoes");

            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "Outgoes");

            migrationBuilder.DropColumn(
                name: "ParentOutgoCategoryId",
                table: "OutgoCategories");

            migrationBuilder.AlterColumn<short>(
                name: "CompanyId",
                table: "Outgoes",
                type: "smallint",
                nullable: true,
                oldClrType: typeof(short),
                oldType: "smallint");

            migrationBuilder.CreateTable(
                name: "MachineOutgoes",
                columns: table => new
                {
                    OutgoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MachineId = table.Column<int>(type: "int", nullable: false),
                    HasErrors = table.Column<bool>(type: "bit", nullable: false),
                    OutgoCategoryId = table.Column<short>(type: "smallint", nullable: false),
                    CompanyId = table.Column<short>(type: "smallint", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Value = table.Column<long>(type: "bigint", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MachineOutgoes", x => x.OutgoId);
                    table.ForeignKey(
                        name: "FK_MachineOutgoes_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId");
                    table.ForeignKey(
                        name: "FK_MachineOutgoes_Machines_MachineId",
                        column: x => x.MachineId,
                        principalTable: "Machines",
                        principalColumn: "MachineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MachineOutgoes_OutgoCategories_OutgoCategoryId",
                        column: x => x.OutgoCategoryId,
                        principalTable: "OutgoCategories",
                        principalColumn: "OutgoCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Username",
                keyValue: "admin",
                column: "CreateDateTime",
                value: new DateTime(2023, 9, 11, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.CreateIndex(
                name: "IX_MachineOutgoes_CompanyId",
                table: "MachineOutgoes",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineOutgoes_MachineId",
                table: "MachineOutgoes",
                column: "MachineId");

            migrationBuilder.CreateIndex(
                name: "IX_MachineOutgoes_OutgoCategoryId",
                table: "MachineOutgoes",
                column: "OutgoCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Outgoes_Companies_CompanyId",
                table: "Outgoes",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Outgoes_Companies_CompanyId",
                table: "Outgoes");

            migrationBuilder.DropTable(
                name: "MachineOutgoes");

            migrationBuilder.AlterColumn<short>(
                name: "CompanyId",
                table: "Outgoes",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0,
                oldClrType: typeof(short),
                oldType: "smallint",
                oldNullable: true);

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

            migrationBuilder.AddColumn<short>(
                name: "ParentOutgoCategoryId",
                table: "OutgoCategories",
                type: "smallint",
                nullable: true);

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

            migrationBuilder.CreateIndex(
                name: "IX_OutgoCategories_ParentOutgoCategoryId",
                table: "OutgoCategories",
                column: "ParentOutgoCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_OutgoCategories_OutgoCategories_ParentOutgoCategoryId",
                table: "OutgoCategories",
                column: "ParentOutgoCategoryId",
                principalTable: "OutgoCategories",
                principalColumn: "OutgoCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Outgoes_Companies_CompanyId",
                table: "Outgoes",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Outgoes_Machines_MachineId",
                table: "Outgoes",
                column: "MachineId",
                principalTable: "Machines",
                principalColumn: "MachineId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
