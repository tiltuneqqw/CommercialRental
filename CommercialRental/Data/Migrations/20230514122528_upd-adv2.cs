using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialRental.Migrations
{
    public partial class updadv2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasFurniture",
                table: "Advertisments");

            migrationBuilder.AddColumn<string>(
                name: "AdditionalInformation",
                table: "Advertisments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Furniture",
                table: "Advertisments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionalInformation",
                table: "Advertisments");

            migrationBuilder.DropColumn(
                name: "Furniture",
                table: "Advertisments");

            migrationBuilder.AddColumn<bool>(
                name: "HasFurniture",
                table: "Advertisments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
