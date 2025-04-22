using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    /// <inheritdoc />
    public partial class jh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wishlist_userId",
                table: "Wishlist");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "PaymentMethods",
                newName: "stripeId");

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

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastMessageAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFromUser = table.Column<bool>(type: "bit", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ConversationId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_userId",
                table: "Wishlist",
                column: "userId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ConversationId",
                table: "ChatMessages",
                column: "ConversationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Wishlist_userId",
                table: "Wishlist");

            migrationBuilder.DropColumn(
                name: "fullRefundDays",
                table: "CancellationPolicies");

            migrationBuilder.DropColumn(
                name: "partialRefundDays",
                table: "CancellationPolicies");

            migrationBuilder.DropColumn(
                name: "partialRefundPercentage",
                table: "CancellationPolicies");

            migrationBuilder.DropColumn(
                name: "paymentIntentId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "paymentTimeOut",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "stripeId",
                table: "PaymentMethods",
                newName: "description");

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_userId",
                table: "Wishlist",
                column: "userId");
        }
    }
}
