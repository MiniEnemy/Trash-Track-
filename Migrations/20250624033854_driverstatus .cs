using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trash_Track.Migrations
{
    /// <inheritdoc />
    public partial class driverstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DriverPickupStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<int>(type: "int", nullable: false),
                    ScheduleId = table.Column<int>(type: "int", nullable: false),
                    CompletedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DriverPickupStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DriverPickupStatuses_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DriverPickupStatuses_PickupSchedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "PickupSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DriverPickupStatuses_DriverId",
                table: "DriverPickupStatuses",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_DriverPickupStatuses_ScheduleId",
                table: "DriverPickupStatuses",
                column: "ScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DriverPickupStatuses");
        }
    }
}
