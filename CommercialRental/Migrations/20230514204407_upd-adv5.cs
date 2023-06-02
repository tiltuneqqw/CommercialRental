using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialRental.Migrations
{
    public partial class updadv5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Checked",
                table: "Advertisments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Checked",
                table: "Advertisments");
        }
    }
}
