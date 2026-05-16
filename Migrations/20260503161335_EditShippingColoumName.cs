using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce_Proj.Migrations
{
    /// <inheritdoc />
    public partial class EditShippingColoumName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActualDate",
                table: "Shippings",
                newName: "ExpectedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ExpectedDate",
                table: "Shippings",
                newName: "ActualDate");
        }
    }
}
