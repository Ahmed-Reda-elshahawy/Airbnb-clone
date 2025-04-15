using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class Modifications2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Bookings__curren__1332DBDC",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_currencyId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "currencyId",
                table: "Bookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "currencyId",
                table: "Bookings",
                type: "int",
                nullable: true,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_currencyId",
                table: "Bookings",
                column: "currencyId");

            migrationBuilder.AddForeignKey(
                name: "FK__Bookings__curren__1332DBDC",
                table: "Bookings",
                column: "currencyId",
                principalTable: "Currencies",
                principalColumn: "Id");
        }
    }
}
