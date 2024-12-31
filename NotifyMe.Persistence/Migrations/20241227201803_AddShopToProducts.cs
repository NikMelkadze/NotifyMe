using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotifyMe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddShopToProducts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Shop",
                table: "UserSavedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Shop",
                table: "UserSavedProducts");
        }
    }
}
