using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialRental.Migrations
{
    public partial class updadv4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisments_Users_UserId",
                table: "Advertisments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Advertisments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisments_Users_UserId",
                table: "Advertisments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Advertisments_Users_UserId",
                table: "Advertisments");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Advertisments",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Advertisments_Users_UserId",
                table: "Advertisments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
