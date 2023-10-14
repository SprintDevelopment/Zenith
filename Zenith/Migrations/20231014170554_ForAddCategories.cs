using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Zenith.Migrations
{
    /// <inheritdoc />
    public partial class ForAddCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "IncomeCategory",
                columns: new[] { "IncomeCategoryId", "Comment", "CostCenter", "CountUnitTitle", "HasErrors", "Title" },
                values: new object[] { (short)1, "", 1, "Time(s)", false, "Transportation related incomes" });

            migrationBuilder.InsertData(
                table: "OutgoCategories",
                columns: new[] { "OutgoCategoryId", "ApproxUnitPrice", "Comment", "CostCenter", "CountUnitTitle", "HasErrors", "ReservedAmount", "Title" },
                values: new object[] { (short)1, 0f, "", 0, "Time(s)", false, 0f, "Transportation related outgoes" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "IncomeCategory",
                keyColumn: "IncomeCategoryId",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                table: "OutgoCategories",
                keyColumn: "OutgoCategoryId",
                keyValue: (short)1);
        }
    }
}
