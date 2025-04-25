using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class availability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isAvailable",
                table: "AvailabilityCalendar",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true,
                oldDefaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "isAvailable",
                table: "AvailabilityCalendar",
                type: "bit",
                nullable: true,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);
        }
    }
}
