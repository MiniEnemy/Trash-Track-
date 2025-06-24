using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Trash_Track.Migrations
{
    /// <inheritdoc />
    public partial class overrideup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ContactNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WardNumber = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Drivers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drivers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    No = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickupOverrides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WardId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OverrideDay = table.Column<int>(type: "int", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NewTime = table.Column<TimeSpan>(type: "time", nullable: true),
                    IsCancelled = table.Column<bool>(type: "bit", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickupOverrides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickupOverrides_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PickupOverrides_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PickupSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WardId = table.Column<int>(type: "int", nullable: false),
                    PickupDay = table.Column<int>(type: "int", nullable: false),
                    PickupTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    DriverId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PickupSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PickupSchedules_Drivers_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PickupSchedules_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReporterName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReporterUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WardId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AssignedDriverId = table.Column<int>(type: "int", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Drivers_AssignedDriverId",
                        column: x => x.AssignedDriverId,
                        principalTable: "Drivers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Drivers",
                columns: new[] { "Id", "Contact", "Name" },
                values: new object[,]
                {
                    { 1, "9801000001", "Ram Bahadur" },
                    { 2, "9801000002", "Shyam Lal" },
                    { 3, "9801000003", "Sita Thapa" },
                    { 4, "9801000004", "Gopal Basnet" },
                    { 5, "9801000005", "Nisha Shrestha" }
                });

            migrationBuilder.InsertData(
                table: "Wards",
                columns: new[] { "Id", "Name", "No" },
                values: new object[,]
                {
                    { 1, "Dilli Bazaar", 1 },
                    { 2, "Maitidevi", 2 },
                    { 3, "Gaushala", 3 },
                    { 4, "Gyaneshwor", 4 },
                    { 5, "Baneshwor", 5 },
                    { 6, "Tinkune", 6 },
                    { 7, "Sinamangal", 7 },
                    { 8, "Tilganga", 8 },
                    { 9, "Old Baneshwor", 9 },
                    { 10, "New Baneshwor", 10 },
                    { 11, "Minbhawan", 11 },
                    { 12, "Shantinagar", 12 },
                    { 13, "Anamnagar", 13 },
                    { 14, "Babarmahal", 14 },
                    { 15, "Tripureshwor", 15 },
                    { 16, "Thapathali", 16 },
                    { 17, "Teku", 17 },
                    { 18, "Kalimati", 18 },
                    { 19, "Balkhu", 19 },
                    { 20, "Kuleshwor", 20 },
                    { 21, "Chhetrapati", 21 },
                    { 22, "Indra Chowk", 22 },
                    { 23, "Ason", 23 },
                    { 24, "Basantapur", 24 },
                    { 25, "Thamel", 25 },
                    { 26, "Lazimpat", 26 },
                    { 27, "Maharajgunj", 27 },
                    { 28, "Baluwatar", 28 },
                    { 29, "Budhanilkantha", 29 },
                    { 30, "Gongabu", 30 },
                    { 31, "Tokha", 31 },
                    { 32, "Samakhusi", 32 }
                });

            migrationBuilder.InsertData(
                table: "PickupSchedules",
                columns: new[] { "Id", "DriverId", "PickupDay", "PickupTime", "WardId" },
                values: new object[,]
                {
                    { 1, null, 0, new TimeSpan(0, 6, 0, 0, 0), 1 },
                    { 2, null, 1, new TimeSpan(0, 6, 0, 0, 0), 2 },
                    { 3, null, 2, new TimeSpan(0, 6, 0, 0, 0), 3 },
                    { 4, null, 3, new TimeSpan(0, 6, 0, 0, 0), 4 },
                    { 5, null, 4, new TimeSpan(0, 6, 0, 0, 0), 5 },
                    { 6, null, 5, new TimeSpan(0, 6, 0, 0, 0), 6 },
                    { 7, null, 6, new TimeSpan(0, 6, 0, 0, 0), 7 },
                    { 8, null, 0, new TimeSpan(0, 6, 0, 0, 0), 8 },
                    { 9, null, 1, new TimeSpan(0, 6, 0, 0, 0), 9 },
                    { 10, null, 2, new TimeSpan(0, 6, 0, 0, 0), 10 },
                    { 11, null, 3, new TimeSpan(0, 6, 0, 0, 0), 11 },
                    { 12, null, 4, new TimeSpan(0, 6, 0, 0, 0), 12 },
                    { 13, null, 5, new TimeSpan(0, 6, 0, 0, 0), 13 },
                    { 14, null, 6, new TimeSpan(0, 6, 0, 0, 0), 14 },
                    { 15, null, 0, new TimeSpan(0, 6, 0, 0, 0), 15 },
                    { 16, null, 1, new TimeSpan(0, 6, 0, 0, 0), 16 },
                    { 17, null, 2, new TimeSpan(0, 6, 0, 0, 0), 17 },
                    { 18, null, 3, new TimeSpan(0, 6, 0, 0, 0), 18 },
                    { 19, null, 4, new TimeSpan(0, 6, 0, 0, 0), 19 },
                    { 20, null, 5, new TimeSpan(0, 6, 0, 0, 0), 20 },
                    { 21, null, 6, new TimeSpan(0, 6, 0, 0, 0), 21 },
                    { 22, null, 0, new TimeSpan(0, 6, 0, 0, 0), 22 },
                    { 23, null, 1, new TimeSpan(0, 6, 0, 0, 0), 23 },
                    { 24, null, 2, new TimeSpan(0, 6, 0, 0, 0), 24 },
                    { 25, null, 3, new TimeSpan(0, 6, 0, 0, 0), 25 },
                    { 26, null, 4, new TimeSpan(0, 6, 0, 0, 0), 26 },
                    { 27, null, 5, new TimeSpan(0, 6, 0, 0, 0), 27 },
                    { 28, null, 6, new TimeSpan(0, 6, 0, 0, 0), 28 },
                    { 29, null, 0, new TimeSpan(0, 6, 0, 0, 0), 29 },
                    { 30, null, 1, new TimeSpan(0, 6, 0, 0, 0), 30 },
                    { 31, null, 2, new TimeSpan(0, 6, 0, 0, 0), 31 },
                    { 32, null, 3, new TimeSpan(0, 6, 0, 0, 0), 32 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PickupOverrides_DriverId",
                table: "PickupOverrides",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupOverrides_WardId",
                table: "PickupOverrides",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupSchedules_DriverId",
                table: "PickupSchedules",
                column: "DriverId");

            migrationBuilder.CreateIndex(
                name: "IX_PickupSchedules_WardId",
                table: "PickupSchedules",
                column: "WardId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AssignedDriverId",
                table: "Reports",
                column: "AssignedDriverId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_WardId",
                table: "Reports",
                column: "WardId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "PickupOverrides");

            migrationBuilder.DropTable(
                name: "PickupSchedules");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Drivers");

            migrationBuilder.DropTable(
                name: "Wards");
        }
    }
}
