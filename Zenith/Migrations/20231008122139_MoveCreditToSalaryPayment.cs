using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class MoveCreditToSalaryPayment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "People");

            migrationBuilder.AddColumn<float>(
                name: "Credit",
                table: "SalaryPayments",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Credit",
                table: "SalaryPayments");

            migrationBuilder.AddColumn<float>(
                name: "Credit",
                table: "People",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
