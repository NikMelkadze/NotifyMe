using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotifyMe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addproductName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserSavedProducts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserSavedProducts");
        }
    }
}
