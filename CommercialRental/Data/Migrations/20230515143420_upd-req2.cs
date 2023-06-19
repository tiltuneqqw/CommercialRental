using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialRental.Migrations
{
    public partial class updreq2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestsRent_Advertisments_AdvertismentId",
                table: "RequestsRent");

            migrationBuilder.DropIndex(
                name: "IX_RequestsRent_AdvertismentId",
                table: "RequestsRent");

            migrationBuilder.DropColumn(
                name: "AdvertismentId",
                table: "RequestsRent");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertismentId",
                table: "RequestsRent",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestsRent_AdvertismentId",
                table: "RequestsRent",
                column: "AdvertismentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestsRent_Advertisments_AdvertismentId",
                table: "RequestsRent",
                column: "AdvertismentId",
                principalTable: "Advertisments",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestsRent_Advertisments_AdvertismentId",
                table: "RequestsRent");

            migrationBuilder.DropIndex(
                name: "IX_RequestsRent_AdvertismentId",
                table: "RequestsRent");

            migrationBuilder.AlterColumn<string>(
                name: "AdvertismentId",
                table: "RequestsRent",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdvertismentId",
                table: "RequestsRent",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RequestsRent_AdvertismentId",
                table: "RequestsRent",
                column: "AdvertismentId");

            migrationBuilder.AddForeignKey(
                name: "FK_RequestsRent_Advertisments_AdvertismentId",
                table: "RequestsRent",
                column: "AdvertismentId",
                principalTable: "Advertisments",
                principalColumn: "Id");
        }
    }
}
