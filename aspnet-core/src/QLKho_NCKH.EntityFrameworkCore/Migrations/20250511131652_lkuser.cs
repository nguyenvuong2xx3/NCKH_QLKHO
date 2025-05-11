using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLKho_NCKH.Migrations
{
    /// <inheritdoc />
    public partial class lkuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "AppStockTransactions",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppStockTransactions_UserId",
                table: "AppStockTransactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppStockTransactions_AbpUsers_UserId",
                table: "AppStockTransactions",
                column: "UserId",
                principalTable: "AbpUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppStockTransactions_AbpUsers_UserId",
                table: "AppStockTransactions");

            migrationBuilder.DropIndex(
                name: "IX_AppStockTransactions_UserId",
                table: "AppStockTransactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "AppStockTransactions");
        }
    }
}
