using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ThAmCo.Catering.Migrations
{
    public partial class CreatedDbContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "thamco.catering");

            migrationBuilder.CreateTable(
                name: "Menus",
                schema: "thamco.catering",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Starter = table.Column<string>(nullable: true),
                    Main = table.Column<string>(nullable: false),
                    Dessert = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Booking",
                schema: "thamco.catering",
                columns: table => new
                {
                    MenuId = table.Column<int>(nullable: false),
                    EventId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Booking", x => new { x.MenuId, x.EventId });
                    table.UniqueConstraint("AK_Booking_EventId_MenuId", x => new { x.EventId, x.MenuId });
                    table.ForeignKey(
                        name: "FK_Booking_Menus_MenuId",
                        column: x => x.MenuId,
                        principalSchema: "thamco.catering",
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "thamco.catering",
                table: "Menus",
                columns: new[] { "Id", "Dessert", "Main", "Starter" },
                values: new object[,]
                {
                    { 1, "Ice Cream", "Roast Dinner", "Prawn Cocktail" },
                    { 2, "Chocolate Fudge Cake", "Burger & Chips", "Soup" },
                    { 3, "Apple Pie", "Chilli Con Carne", "Onion Rings" },
                    { 4, "Jam Roly-Poly", "Spaghetti Bolognese", "Nachos" }
                });

            migrationBuilder.InsertData(
                schema: "thamco.catering",
                table: "Booking",
                columns: new[] { "MenuId", "EventId" },
                values: new object[] { 1, 1 });

            migrationBuilder.InsertData(
                schema: "thamco.catering",
                table: "Booking",
                columns: new[] { "MenuId", "EventId" },
                values: new object[] { 2, 2 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Booking",
                schema: "thamco.catering");

            migrationBuilder.DropTable(
                name: "Menus",
                schema: "thamco.catering");
        }
    }
}
