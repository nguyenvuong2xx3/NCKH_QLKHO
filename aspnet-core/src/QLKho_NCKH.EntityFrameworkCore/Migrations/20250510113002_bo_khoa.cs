using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLKho_NCKH.Migrations
{
    /// <inheritdoc />
    public partial class bo_khoa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppOrderDetails_AppInventoryItems_InventoryItemId",
                table: "AppOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_AppOrderDetails_InventoryItemId",
                table: "AppOrderDetails");

            migrationBuilder.DropColumn(
                name: "InventoryItemId",
                table: "AppOrderDetails");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "InventoryItemId",
                table: "AppOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppOrderDetails_InventoryItemId",
                table: "AppOrderDetails",
                column: "InventoryItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppOrderDetails_AppInventoryItems_InventoryItemId",
                table: "AppOrderDetails",
                column: "InventoryItemId",
                principalTable: "AppInventoryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
