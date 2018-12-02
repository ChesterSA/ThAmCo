using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Events.Data.Migrations
{
    public partial class EventCostDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FoodCost",
                schema: "thamco.events",
                table: "Events",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Menu",
                schema: "thamco.events",
                table: "Events",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "VenueCost",
                schema: "thamco.events",
                table: "Events",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "FoodMenuViewModel",
                schema: "thamco.events",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Starter = table.Column<string>(nullable: true),
                    Main = table.Column<string>(nullable: false),
                    Dessert = table.Column<string>(nullable: true),
                    Cost = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodMenuViewModel", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodMenuViewModel",
                schema: "thamco.events");

            migrationBuilder.DropColumn(
                name: "FoodCost",
                schema: "thamco.events",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Menu",
                schema: "thamco.events",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "VenueCost",
                schema: "thamco.events",
                table: "Events");
        }
    }
}
