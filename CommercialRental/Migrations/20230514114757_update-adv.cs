using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommercialRental.Migrations
{
    public partial class updateadv : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RequestsRent_Advertisments_AdvertismentId",
                table: "RequestsRent");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertismentId",
                table: "RequestsRent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "RequestsRent",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationDate",
                table: "Advertisments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsRented",
                table: "Advertisments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartRentDate",
                table: "Advertisments",
                type: "datetime2",
                nullable: true);

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

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "RequestsRent");

            migrationBuilder.DropColumn(
                name: "CreationDate",
                table: "Advertisments");

            migrationBuilder.DropColumn(
                name: "IsRented",
                table: "Advertisments");

            migrationBuilder.DropColumn(
                name: "StartRentDate",
                table: "Advertisments");

            migrationBuilder.AlterColumn<int>(
                name: "AdvertismentId",
                table: "RequestsRent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RequestsRent_Advertisments_AdvertismentId",
                table: "RequestsRent",
                column: "AdvertismentId",
                principalTable: "Advertisments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
