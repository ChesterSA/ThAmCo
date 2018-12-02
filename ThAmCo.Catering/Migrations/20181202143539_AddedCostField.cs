using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Catering.Migrations
{
    public partial class AddedCostField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Cost",
                schema: "thamco.catering",
                table: "Menus",
                nullable: false,
                defaultValue: 0.0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                schema: "thamco.catering",
                table: "Menus");
        }
    }
}
