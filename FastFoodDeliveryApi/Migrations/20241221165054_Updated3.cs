using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FastFoodDeliveryApi.Migrations
{
    /// <inheritdoc />
    public partial class Updated3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_Addresses_UserAddressId",
                table: "Restaurants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "RestaurantAddresses");

            migrationBuilder.DropColumn(
                name: "HouseNumber",
                table: "Addresses");

            migrationBuilder.RenameTable(
                name: "Addresses",
                newName: "UserAddresses");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "RestaurantAddresses",
                newName: "Suburb");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "RestaurantAddresses",
                newName: "Road");

            migrationBuilder.RenameColumn(
                name: "Street",
                table: "UserAddresses",
                newName: "Suburb");

            migrationBuilder.RenameColumn(
                name: "District",
                table: "UserAddresses",
                newName: "Road");

            migrationBuilder.RenameIndex(
                name: "IX_Addresses_UserId",
                table: "UserAddresses",
                newName: "IX_UserAddresses_UserId");

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "RestaurantAddresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Residential",
                table: "RestaurantAddresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "County",
                table: "UserAddresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Residential",
                table: "UserAddresses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAddresses",
                table: "UserAddresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_UserAddresses_UserAddressId",
                table: "Restaurants",
                column: "UserAddressId",
                principalTable: "UserAddresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAddresses_Users_UserId",
                table: "UserAddresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurants_UserAddresses_UserAddressId",
                table: "Restaurants");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAddresses_Users_UserId",
                table: "UserAddresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAddresses",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "County",
                table: "RestaurantAddresses");

            migrationBuilder.DropColumn(
                name: "Residential",
                table: "RestaurantAddresses");

            migrationBuilder.DropColumn(
                name: "County",
                table: "UserAddresses");

            migrationBuilder.DropColumn(
                name: "Residential",
                table: "UserAddresses");

            migrationBuilder.RenameTable(
                name: "UserAddresses",
                newName: "Addresses");

            migrationBuilder.RenameColumn(
                name: "Suburb",
                table: "RestaurantAddresses",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Road",
                table: "RestaurantAddresses",
                newName: "District");

            migrationBuilder.RenameColumn(
                name: "Suburb",
                table: "Addresses",
                newName: "Street");

            migrationBuilder.RenameColumn(
                name: "Road",
                table: "Addresses",
                newName: "District");

            migrationBuilder.RenameIndex(
                name: "IX_UserAddresses_UserId",
                table: "Addresses",
                newName: "IX_Addresses_UserId");

            migrationBuilder.AddColumn<int>(
                name: "HouseNumber",
                table: "RestaurantAddresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "HouseNumber",
                table: "Addresses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Addresses",
                table: "Addresses",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Users_UserId",
                table: "Addresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurants_Addresses_UserAddressId",
                table: "Restaurants",
                column: "UserAddressId",
                principalTable: "Addresses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
