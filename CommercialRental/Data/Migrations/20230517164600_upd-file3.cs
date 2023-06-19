using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialRental.Migrations
{
    public partial class updfile3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdvertismentId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdvertismentId",
                table: "Files");
        }
    }
}
