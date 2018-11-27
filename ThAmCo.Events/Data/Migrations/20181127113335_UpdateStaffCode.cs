using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class UpdateStaffCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "thamco.events",
                table: "Staff",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "isActive",
                schema: "thamco.events",
                table: "Events",
                newName: "IsActive");

            migrationBuilder.AddColumn<string>(
                name: "StaffCode",
                schema: "thamco.events",
                table: "Staff",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Staff",
                keyColumn: "Id",
                keyValue: 1,
                column: "StaffCode",
                value: "SS1");

            migrationBuilder.UpdateData(
                schema: "thamco.events",
                table: "Staff",
                keyColumn: "Id",
                keyValue: 2,
                column: "StaffCode",
                value: "AMN2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StaffCode",
                schema: "thamco.events",
                table: "Staff");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                schema: "thamco.events",
                table: "Events",
                newName: "isActive");

            migrationBuilder.InsertData(
                schema: "thamco.events",
                table: "Staff",
                columns: new[] { "Id", "FirstName", "Surname" },
                values: new object[] { 3, "Jeremy", "Osbourne" });
        }
    }
}
