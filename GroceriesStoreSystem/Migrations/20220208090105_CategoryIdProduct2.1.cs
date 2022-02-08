using Microsoft.EntityFrameworkCore.Migrations;

namespace GroceriesStoreSystem.Migrations
{
    public partial class CategoryIdProduct21 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NeedToAddQuantity",
                table: "Groceries",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NeedToAddQuantity",
                table: "Groceries");
        }
    }
}
