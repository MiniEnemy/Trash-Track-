using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trash_Track.Migrations
{
    /// <inheritdoc />
    public partial class reportup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OverrideDay",
                table: "PickupOverrides",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverrideDay",
                table: "PickupOverrides");
        }
    }
}
