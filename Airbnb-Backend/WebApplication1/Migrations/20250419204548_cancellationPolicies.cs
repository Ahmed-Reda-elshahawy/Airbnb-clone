using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class cancellationPolicies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "fullRefundDays",
                table: "CancellationPolicies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "partialRefundDays",
                table: "CancellationPolicies",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "partialRefundPercentage",
                table: "CancellationPolicies",
                type: "decimal(5,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "fullRefundDays",
                table: "CancellationPolicies");

            migrationBuilder.DropColumn(
                name: "partialRefundDays",
                table: "CancellationPolicies");

            migrationBuilder.DropColumn(
                name: "partialRefundPercentage",
                table: "CancellationPolicies");
        }
    }
}
