using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class ChangesInOutgoes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MachineId",
                table: "Outgoes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "MachineIncomeValue",
                table: "Outgoes",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MachineId",
                table: "Outgoes");

            migrationBuilder.DropColumn(
                name: "MachineIncomeValue",
                table: "Outgoes");
        }
    }
}
