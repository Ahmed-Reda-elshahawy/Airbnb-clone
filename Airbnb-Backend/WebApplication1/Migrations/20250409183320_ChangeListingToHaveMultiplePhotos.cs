using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class ChangeListingToHaveMultiplePhotos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__ListingPh__listi__6477ECF3",
                table: "ListingPhotos");

            migrationBuilder.AddForeignKey(
                name: "FK_ListingPhotos_Listing",
                table: "ListingPhotos",
                column: "listingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ListingPhotos_Listing",
                table: "ListingPhotos");

            migrationBuilder.AddForeignKey(
                name: "FK__ListingPh__listi__6477ECF3",
                table: "ListingPhotos",
                column: "listingId",
                principalTable: "Listings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
