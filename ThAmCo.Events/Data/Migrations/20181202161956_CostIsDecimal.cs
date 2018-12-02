using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class CostIsDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                schema: "thamco.events",
                table: "FoodMenuViewModel",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "VenueCost",
                schema: "thamco.events",
                table: "Events",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<decimal>(
                name: "FoodCost",
                schema: "thamco.events",
                table: "Events",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FoodCost", "VenueCost" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FoodCost", "VenueCost" },
                values: new object[] { 0m, 0m });

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FoodCost", "VenueCost" },
                values: new object[] { 0m, 0m });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                schema: "thamco.events",
                table: "FoodMenuViewModel",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "VenueCost",
                schema: "thamco.events",
                table: "Events",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<double>(
                name: "FoodCost",
                schema: "thamco.events",
                table: "Events",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "FoodCost", "VenueCost" },
                values: new object[] { 0.0, 0.0 });

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Events",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "FoodCost", "VenueCost" },
                values: new object[] { 0.0, 0.0 });

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Events",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "FoodCost", "VenueCost" },
                values: new object[] { 0.0, 0.0 });
        }
    }
}
