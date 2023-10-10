using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class BulkChanges2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CashState",
                table: "Sales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CashState",
                table: "SalaryPayments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CashState",
                table: "Outgoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CashState",
                table: "MachineOutgoes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CashState",
                table: "Deliveries",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CashState",
                table: "Buys",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CashState",
                table: "Sales");

            migrationBuilder.DropColumn(
                name: "CashState",
                table: "SalaryPayments");

            migrationBuilder.DropColumn(
                name: "CashState",
                table: "Outgoes");

            migrationBuilder.DropColumn(
                name: "CashState",
                table: "MachineOutgoes");

            migrationBuilder.DropColumn(
                name: "CashState",
                table: "Deliveries");

            migrationBuilder.DropColumn(
                name: "CashState",
                table: "Buys");
        }
    }
}
