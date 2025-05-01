using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KMCSCI3110Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class updateManytoMany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleFeatures",
                table: "VehicleFeatures");

            migrationBuilder.DropIndex(
                name: "IX_VehicleFeatures_VehicleId",
                table: "VehicleFeatures");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "VehicleFeatures");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleFeatures",
                table: "VehicleFeatures",
                columns: new[] { "VehicleId", "FeatureId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_VehicleFeatures",
                table: "VehicleFeatures");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "VehicleFeatures",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VehicleFeatures",
                table: "VehicleFeatures",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleFeatures_VehicleId",
                table: "VehicleFeatures",
                column: "VehicleId");
        }
    }
}
