using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class Additions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "valueRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "locationRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "communicationRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "cleanlinessRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "checkInRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "accuracyRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "icon",
                table: "PropertyTypes",
                type: "varchar(100)",
                unicode: false,
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "verificationStatusId",
                table: "Listings",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "AdditionalInformation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    listingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    data = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false),
                    description = table.Column<string>(type: "varchar(max)", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Additio__3214EC07F0A1500F", x => x.Id);
                    table.ForeignKey(
                        name: "FK__Additiona__listi__7D439ABD",
                        column: x => x.listingId,
                        principalTable: "Listings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Listings_verificationStatusId",
                table: "Listings",
                column: "verificationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AdditionalInformation_listingId",
                table: "AdditionalInformation",
                column: "listingId");

            migrationBuilder.AddForeignKey(
                name: "FK__Listing__verificat__4BAC3F29",
                table: "Listings",
                column: "verificationStatusId",
                principalTable: "VerificationStatus",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Listing__verificat__4BAC3F29",
                table: "Listings");

            migrationBuilder.DropTable(
                name: "AdditionalInformation");

            migrationBuilder.DropIndex(
                name: "IX_Listings_verificationStatusId",
                table: "Listings");

            migrationBuilder.DropColumn(
                name: "icon",
                table: "PropertyTypes");

            migrationBuilder.DropColumn(
                name: "verificationStatusId",
                table: "Listings");

            migrationBuilder.AlterColumn<decimal>(
                name: "valueRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)");

            migrationBuilder.AlterColumn<decimal>(
                name: "locationRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)");

            migrationBuilder.AlterColumn<decimal>(
                name: "communicationRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)");

            migrationBuilder.AlterColumn<decimal>(
                name: "cleanlinessRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)");

            migrationBuilder.AlterColumn<decimal>(
                name: "checkInRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)");

            migrationBuilder.AlterColumn<decimal>(
                name: "accuracyRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)");
        }
    }
}
