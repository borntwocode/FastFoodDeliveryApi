using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodDeliveryApi.Migrations
{
    /// <inheritdoc />
    public partial class Updated4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Suburb",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "Suburb",
                table: "RestaurantAddresses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Suburb",
                table: "UserAddresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Suburb",
                table: "RestaurantAddresses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
