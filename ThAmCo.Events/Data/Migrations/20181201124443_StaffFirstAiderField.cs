using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class StaffFirstAiderField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "FirstAider",
                schema: "thamco.events",
                table: "Staff",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Staff",
                keyColumn: "Id",
                keyValue: 1,
                column: "FirstAider",
                value: true);

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Staff",
                keyColumn: "Id",
                keyValue: 4,
                column: "FirstAider",
                value: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstAider",
                schema: "thamco.events",
                table: "Staff");
        }
    }
}
