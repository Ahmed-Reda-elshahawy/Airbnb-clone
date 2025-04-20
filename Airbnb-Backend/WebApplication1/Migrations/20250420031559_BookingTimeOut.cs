using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class BookingTimeOut : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "paymentIntentId",
                table: "Bookings",
                type: "varchar(255)",
                unicode: false,
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "paymentTimeOut",
                table: "Bookings",
                type: "datetime",
                nullable: false,
                defaultValueSql: "DATEADD(MINUTE, 15, GETDATE())");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "paymentIntentId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "paymentTimeOut",
                table: "Bookings");
        }
    }
}
