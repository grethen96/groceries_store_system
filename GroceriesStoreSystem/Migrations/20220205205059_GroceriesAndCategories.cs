using Microsoft.EntityFrameworkCore.Migrations;

namespace GroceriesStoreSystem.Migrations
{
    public partial class GroceriesAndCategories : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Groceries");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Groceries",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Groceries",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductCategoryCategoryId",
                table: "Groceries",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Supplier",
                table: "Groceries",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    BrandId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrandName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.BrandId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategory",
                columns: table => new
                {
                    CategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategory", x => x.CategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Groceries_ProductCategoryCategoryId",
                table: "Groceries",
                column: "ProductCategoryCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groceries_ProductCategory_ProductCategoryCategoryId",
                table: "Groceries",
                column: "ProductCategoryCategoryId",
                principalTable: "ProductCategory",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groceries_ProductCategory_ProductCategoryCategoryId",
                table: "Groceries");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "ProductCategory");

            migrationBuilder.DropIndex(
                name: "IX_Groceries_ProductCategoryCategoryId",
                table: "Groceries");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Groceries");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Groceries");

            migrationBuilder.DropColumn(
                name: "ProductCategoryCategoryId",
                table: "Groceries");

            migrationBuilder.DropColumn(
                name: "Supplier",
                table: "Groceries");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Groceries",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
