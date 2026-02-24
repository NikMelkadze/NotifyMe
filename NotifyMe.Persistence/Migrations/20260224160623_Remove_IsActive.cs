using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotifyMe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Remove_IsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserSavedProducts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserSavedProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
