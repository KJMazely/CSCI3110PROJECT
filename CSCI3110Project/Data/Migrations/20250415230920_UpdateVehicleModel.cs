using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMCSCI3110Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateVehicleModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Vehicles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "VehicleClass",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "VehicleClass",
                table: "Vehicles");
        }
    }
}
