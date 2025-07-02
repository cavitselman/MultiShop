using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MS.Cargo.DAL.Migrations
{
    /// <inheritdoc />
    public partial class mig6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderDetailId2",
                table: "CargoDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId2",
                table: "CargoDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
