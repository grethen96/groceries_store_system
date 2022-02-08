using Microsoft.EntityFrameworkCore.Migrations;

namespace GroceriesStoreSystem.Migrations
{
    public partial class CategoryIdProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "Groceries",
                newName: "CategoryIdProduct");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryIdProduct",
                table: "Groceries",
                newName: "CategoryId");
        }
    }
}
