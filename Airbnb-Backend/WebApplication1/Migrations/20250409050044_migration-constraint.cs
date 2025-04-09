using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class migrationconstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"ALTER TABLE Listings
DROP CONSTRAINT FK__Listings__roomTy__5BE2A6F2;
ALTER TABLE Listings
Add CONSTRAINT FK__Listings__roomTy__5BE2A6F2
FOREIGN KEY (RoomTypeId)
REFERENCES RoomTypes(Id)"
            );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
