using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Trash_Track.Migrations
{
    /// <inheritdoc />
    public partial class wardname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Wards",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Dilli Bazaar");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 2,
                column: "Name",
                value: "Maitidevi");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 3,
                column: "Name",
                value: "Gaushala");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 4,
                column: "Name",
                value: "Gyaneshwor");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 5,
                column: "Name",
                value: "Baneshwor");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 6,
                column: "Name",
                value: "Tinkune");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 7,
                column: "Name",
                value: "Sinamangal");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 8,
                column: "Name",
                value: "Tilganga");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 9,
                column: "Name",
                value: "Old Baneshwor");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 10,
                column: "Name",
                value: "New Baneshwor");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 11,
                column: "Name",
                value: "Minbhawan");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 12,
                column: "Name",
                value: "Shantinagar");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 13,
                column: "Name",
                value: "Anamnagar");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 14,
                column: "Name",
                value: "Babarmahal");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 15,
                column: "Name",
                value: "Tripureshwor");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 16,
                column: "Name",
                value: "Thapathali");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 17,
                column: "Name",
                value: "Teku");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 18,
                column: "Name",
                value: "Kalimati");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 19,
                column: "Name",
                value: "Balkhu");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 20,
                column: "Name",
                value: "Kuleshwor");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 21,
                column: "Name",
                value: "Chhetrapati");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 22,
                column: "Name",
                value: "Indra Chowk");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 23,
                column: "Name",
                value: "Ason");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 24,
                column: "Name",
                value: "Basantapur");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 25,
                column: "Name",
                value: "Thamel");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 26,
                column: "Name",
                value: "Lazimpat");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 27,
                column: "Name",
                value: "Maharajgunj");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 28,
                column: "Name",
                value: "Baluwatar");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 29,
                column: "Name",
                value: "Budhanilkantha");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 30,
                column: "Name",
                value: "Gongabu");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 31,
                column: "Name",
                value: "Tokha");

            migrationBuilder.UpdateData(
                table: "Wards",
                keyColumn: "Id",
                keyValue: 32,
                column: "Name",
                value: "Samakhusi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Wards");
        }
    }
}
