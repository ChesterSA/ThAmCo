using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Catering.Migrations
{
    public partial class CostIsDecimal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Cost",
                schema: "thamco.catering",
                table: "Menus",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.UpdateData(
                schema: "thamco.catering",
                table: "Menus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Cost",
                value: 10.40m);

            migrationBuilder.UpdateData(
                schema: "thamco.catering",
                table: "Menus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Cost",
                value: 15.50m);

            migrationBuilder.UpdateData(
                schema: "thamco.catering",
                table: "Menus",
                keyColumn: "Id",
                keyValue: 3,
                column: "Cost",
                value: 12.30m);

            migrationBuilder.UpdateData(
                schema: "thamco.catering",
                table: "Menus",
                keyColumn: "Id",
                keyValue: 4,
                column: "Cost",
                value: 19.95m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Cost",
                schema: "thamco.catering",
                table: "Menus",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.UpdateData(
                schema: "thamco.catering",
                table: "Menus",
                keyColumn: "Id",
                keyValue: 1,
                column: "Cost",
                value: 10.4);

            migrationBuilder.UpdateData(
                schema: "thamco.catering",
                table: "Menus",
                keyColumn: "Id",
                keyValue: 2,
                column: "Cost",
                value: 15.5);

            migrationBuilder.UpdateData(
                schema: "thamco.catering",
                table: "Menus",
                keyColumn: "Id",
                keyValue: 3,
                column: "Cost",
                value: 12.3);

            migrationBuilder.UpdateData(
                schema: "thamco.catering",
                table: "Menus",
                keyColumn: "Id",
                keyValue: 4,
                column: "Cost",
                value: 19.95);
        }
    }
}
