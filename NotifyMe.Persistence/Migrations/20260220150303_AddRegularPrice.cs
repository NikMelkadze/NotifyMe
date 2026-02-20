using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotifyMe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddRegularPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NewPrice",
                table: "UserSavedProducts",
                newName: "DiscountedPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "RegularPrice",
                table: "UserSavedProducts",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RegularPrice",
                table: "UserSavedProducts");

            migrationBuilder.RenameColumn(
                name: "DiscountedPrice",
                table: "UserSavedProducts",
                newName: "NewPrice");
        }
    }
}
