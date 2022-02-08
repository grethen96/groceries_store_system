using Microsoft.EntityFrameworkCore.Migrations;

namespace GroceriesStoreSystem.Migrations
{
    public partial class CategoryIdProduct2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryNameView",
                table: "Groceries",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryNameView",
                table: "Groceries");
        }
    }
}
