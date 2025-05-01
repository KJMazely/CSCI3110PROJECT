using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMCSCI3110Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleSpecs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CargoSize",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DoorCount",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Gearbox",
                table: "Vehicles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "MPG",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeatAmount",
                table: "Vehicles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CargoSize",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "DoorCount",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Gearbox",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "MPG",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "SeatAmount",
                table: "Vehicles");
        }
    }
}
