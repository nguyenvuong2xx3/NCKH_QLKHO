using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLKho_NCKH.Migrations
{
    /// <inheritdoc />
    public partial class lienketkho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCarts_AppInventoryItems_InventoryItemId",
                table: "AppCarts");

            migrationBuilder.RenameColumn(
                name: "InventoryItemId",
                table: "AppCarts",
                newName: "ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AppCarts_InventoryItemId",
                table: "AppCarts",
                newName: "IX_AppCarts_ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCarts_AppProducts_ProductId",
                table: "AppCarts",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCarts_AppProducts_ProductId",
                table: "AppCarts");

            migrationBuilder.RenameColumn(
                name: "ProductId",
                table: "AppCarts",
                newName: "InventoryItemId");

            migrationBuilder.RenameIndex(
                name: "IX_AppCarts_ProductId",
                table: "AppCarts",
                newName: "IX_AppCarts_InventoryItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCarts_AppInventoryItems_InventoryItemId",
                table: "AppCarts",
                column: "InventoryItemId",
                principalTable: "AppInventoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
