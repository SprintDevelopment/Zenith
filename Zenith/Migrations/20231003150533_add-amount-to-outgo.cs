using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class addamounttooutgo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Amount",
                table: "Outgoes",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Amount",
                table: "MachineOutgoes",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Outgoes");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "MachineOutgoes");
        }
    }
}
