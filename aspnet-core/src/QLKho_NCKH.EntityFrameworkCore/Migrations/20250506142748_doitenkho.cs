using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLKho_NCKH.Migrations
{
    /// <inheritdoc />
    public partial class doitenkho : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_StorageLocations_StorageLocationId",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransaction_Suppliers_SupplierId",
                table: "StockTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransaction_Warehouses_FromWarehouseId",
                table: "StockTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransaction_Warehouses_ToWarehouseId",
                table: "StockTransaction");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactionDetails_StockTransaction_StockTransactionId",
                table: "StockTransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactionDetails_StorageLocations_StorageLocationId",
                table: "StockTransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StorageLocations_Warehouses_WarehouseId",
                table: "StorageLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StorageLocations",
                table: "StorageLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockTransaction",
                table: "StockTransaction");

            migrationBuilder.RenameTable(
                name: "Suppliers",
                newName: "AppSuppliers");

            migrationBuilder.RenameTable(
                name: "StorageLocations",
                newName: "AppStorageLocations");

            migrationBuilder.RenameTable(
                name: "StockTransaction",
                newName: "AppStockTransactions");

            migrationBuilder.RenameIndex(
                name: "IX_StorageLocations_WarehouseId",
                table: "AppStorageLocations",
                newName: "IX_AppStorageLocations_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransaction_ToWarehouseId",
                table: "AppStockTransactions",
                newName: "IX_AppStockTransactions_ToWarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransaction_SupplierId",
                table: "AppStockTransactions",
                newName: "IX_AppStockTransactions_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransaction_FromWarehouseId",
                table: "AppStockTransactions",
                newName: "IX_AppStockTransactions_FromWarehouseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppSuppliers",
                table: "AppSuppliers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppStorageLocations",
                table: "AppStorageLocations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppStockTransactions",
                table: "AppStockTransactions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppStockTransactions_AppSuppliers_SupplierId",
                table: "AppStockTransactions",
                column: "SupplierId",
                principalTable: "AppSuppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppStockTransactions_Warehouses_FromWarehouseId",
                table: "AppStockTransactions",
                column: "FromWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppStockTransactions_Warehouses_ToWarehouseId",
                table: "AppStockTransactions",
                column: "ToWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppStorageLocations_Warehouses_WarehouseId",
                table: "AppStorageLocations",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_AppStorageLocations_StorageLocationId",
                table: "InventoryItems",
                column: "StorageLocationId",
                principalTable: "AppStorageLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AppSuppliers_SupplierId",
                table: "Products",
                column: "SupplierId",
                principalTable: "AppSuppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactionDetails_AppStockTransactions_StockTransactionId",
                table: "StockTransactionDetails",
                column: "StockTransactionId",
                principalTable: "AppStockTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactionDetails_AppStorageLocations_StorageLocationId",
                table: "StockTransactionDetails",
                column: "StorageLocationId",
                principalTable: "AppStorageLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppStockTransactions_AppSuppliers_SupplierId",
                table: "AppStockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_AppStockTransactions_Warehouses_FromWarehouseId",
                table: "AppStockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_AppStockTransactions_Warehouses_ToWarehouseId",
                table: "AppStockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_AppStorageLocations_Warehouses_WarehouseId",
                table: "AppStorageLocations");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_AppStorageLocations_StorageLocationId",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AppSuppliers_SupplierId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactionDetails_AppStockTransactions_StockTransactionId",
                table: "StockTransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactionDetails_AppStorageLocations_StorageLocationId",
                table: "StockTransactionDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppSuppliers",
                table: "AppSuppliers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppStorageLocations",
                table: "AppStorageLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppStockTransactions",
                table: "AppStockTransactions");

            migrationBuilder.RenameTable(
                name: "AppSuppliers",
                newName: "Suppliers");

            migrationBuilder.RenameTable(
                name: "AppStorageLocations",
                newName: "StorageLocations");

            migrationBuilder.RenameTable(
                name: "AppStockTransactions",
                newName: "StockTransaction");

            migrationBuilder.RenameIndex(
                name: "IX_AppStorageLocations_WarehouseId",
                table: "StorageLocations",
                newName: "IX_StorageLocations_WarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_AppStockTransactions_ToWarehouseId",
                table: "StockTransaction",
                newName: "IX_StockTransaction_ToWarehouseId");

            migrationBuilder.RenameIndex(
                name: "IX_AppStockTransactions_SupplierId",
                table: "StockTransaction",
                newName: "IX_StockTransaction_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_AppStockTransactions_FromWarehouseId",
                table: "StockTransaction",
                newName: "IX_StockTransaction_FromWarehouseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Suppliers",
                table: "Suppliers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StorageLocations",
                table: "StorageLocations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockTransaction",
                table: "StockTransaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_StorageLocations_StorageLocationId",
                table: "InventoryItems",
                column: "StorageLocationId",
                principalTable: "StorageLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Suppliers_SupplierId",
                table: "Products",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransaction_Suppliers_SupplierId",
                table: "StockTransaction",
                column: "SupplierId",
                principalTable: "Suppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransaction_Warehouses_FromWarehouseId",
                table: "StockTransaction",
                column: "FromWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransaction_Warehouses_ToWarehouseId",
                table: "StockTransaction",
                column: "ToWarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactionDetails_StockTransaction_StockTransactionId",
                table: "StockTransactionDetails",
                column: "StockTransactionId",
                principalTable: "StockTransaction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactionDetails_StorageLocations_StorageLocationId",
                table: "StockTransactionDetails",
                column: "StorageLocationId",
                principalTable: "StorageLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StorageLocations_Warehouses_WarehouseId",
                table: "StorageLocations",
                column: "WarehouseId",
                principalTable: "Warehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
