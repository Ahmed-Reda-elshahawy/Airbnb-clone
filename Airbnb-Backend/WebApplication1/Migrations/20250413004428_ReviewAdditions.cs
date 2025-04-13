using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class ReviewAdditions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "valueRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "rating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "locationRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "communicationRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "cleanlinessRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "checkInRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "accuracyRating",
                table: "Reviews",
                type: "decimal(3,1)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "hostReplyDate",
                table: "Reviews",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_AccuracyRating",
                table: "Reviews",
                sql: "[accuracyRating] BETWEEN 0 AND 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_CheckInRating",
                table: "Reviews",
                sql: "[checkInRating] BETWEEN 0 AND 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_CleanlinessRating",
                table: "Reviews",
                sql: "[cleanlinessRating] BETWEEN 0 AND 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_CommunicationRating",
                table: "Reviews",
                sql: "[communicationRating] BETWEEN 0 AND 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_LocationRating",
                table: "Reviews",
                sql: "[locationRating] BETWEEN 0 AND 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_Rating",
                table: "Reviews",
                sql: "[rating] BETWEEN 0 AND 5");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_ValueRating",
                table: "Reviews",
                sql: "[valueRating] BETWEEN 0 AND 5");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_AccuracyRating",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_CheckInRating",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_CleanlinessRating",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_CommunicationRating",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_LocationRating",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_Rating",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_ValueRating",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "hostReplyDate",
                table: "Reviews");

            migrationBuilder.AlterColumn<int>(
                name: "valueRating",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "rating",
                table: "Reviews",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)");

            migrationBuilder.AlterColumn<int>(
                name: "locationRating",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "communicationRating",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "cleanlinessRating",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "checkInRating",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "accuracyRating",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(3,1)",
                oldNullable: true);
        }
    }
}
