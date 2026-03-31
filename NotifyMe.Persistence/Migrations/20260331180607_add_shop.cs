using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NotifyMe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class add_shop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Shop",
                table: "UserSavedProducts",
                newName: "ShopId");

            migrationBuilder.CreateTable(
                name: "Shop",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shop", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Shop",
                columns: new[] { "Id", "Category", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Megatechnica" },
                    { 2, 1, "Itworks" },
                    { 3, 3, "Dressup" },
                    { 4, 0, "Europroduct" },
                    { 5, 0, "Agrohub" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSavedProducts_ShopId",
                table: "UserSavedProducts",
                column: "ShopId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSavedProducts_Shop_ShopId",
                table: "UserSavedProducts",
                column: "ShopId",
                principalTable: "Shop",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSavedProducts_Shop_ShopId",
                table: "UserSavedProducts");

            migrationBuilder.DropTable(
                name: "Shop");

            migrationBuilder.DropIndex(
                name: "IX_UserSavedProducts_ShopId",
                table: "UserSavedProducts");

            migrationBuilder.RenameColumn(
                name: "ShopId",
                table: "UserSavedProducts",
                newName: "Shop");
        }
    }
}
