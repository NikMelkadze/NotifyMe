using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotifyMe.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class notificationType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationType",
                table: "UserSavedProducts",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationType",
                table: "UserSavedProducts");
        }
    }
}
