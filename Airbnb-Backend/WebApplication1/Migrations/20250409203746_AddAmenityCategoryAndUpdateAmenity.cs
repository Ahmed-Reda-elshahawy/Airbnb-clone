using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class AddAmenityCategoryAndUpdateAmenity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "category",
                table: "Amenities");

            migrationBuilder.AddColumn<Guid>(
                name: "categoryId",
                table: "Amenities",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AmenityCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmenityCategory", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Amenities_categoryId",
                table: "Amenities",
                column: "categoryId");

            migrationBuilder.AddForeignKey(
                name: "FK__Amenities__categ__6A30C649",
                table: "Amenities",
                column: "categoryId",
                principalTable: "AmenityCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__Amenities__categ__6A30C649",
                table: "Amenities");

            migrationBuilder.DropTable(
                name: "AmenityCategory");

            migrationBuilder.DropIndex(
                name: "IX_Amenities_categoryId",
                table: "Amenities");

            migrationBuilder.DropColumn(
                name: "categoryId",
                table: "Amenities");

            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "Amenities",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
