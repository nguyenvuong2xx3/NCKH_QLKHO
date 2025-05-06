using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QLKho_NCKH.Migrations
{
    /// <inheritdoc />
    public partial class doitenkho1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "FK_Categories_Categories_ParentId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_AppStorageLocations_StorageLocationId",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Products_ProductId",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_AppSuppliers_SupplierId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactionDetails_AppStockTransactions_StockTransactionId",
                table: "StockTransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactionDetails_AppStorageLocations_StorageLocationId",
                table: "StockTransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_StockTransactionDetails_Products_ProductId",
                table: "StockTransactionDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StockTransactionDetails",
                table: "StockTransactionDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                table: "Products");

            migrationBuilder.DropPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Warehouses",
                newName: "AppWarehouses");

            migrationBuilder.RenameTable(
                name: "StockTransactionDetails",
                newName: "AppStockTransactionDetails");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "AppProducts");

            migrationBuilder.RenameTable(
                name: "InventoryItems",
                newName: "AppInventoryItems");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "AppCategories");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransactionDetails_StorageLocationId",
                table: "AppStockTransactionDetails",
                newName: "IX_AppStockTransactionDetails_StorageLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransactionDetails_ProductId",
                table: "AppStockTransactionDetails",
                newName: "IX_AppStockTransactionDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_StockTransactionDetails_BatchNumber",
                table: "AppStockTransactionDetails",
                newName: "IX_AppStockTransactionDetails_BatchNumber");

            migrationBuilder.RenameIndex(
                name: "IX_Products_SupplierId",
                table: "AppProducts",
                newName: "IX_AppProducts_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Code",
                table: "AppProducts",
                newName: "IX_AppProducts_Code");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "AppProducts",
                newName: "IX_AppProducts_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItems_StorageLocationId",
                table: "AppInventoryItems",
                newName: "IX_AppInventoryItems_StorageLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_InventoryItems_ProductId_StorageLocationId",
                table: "AppInventoryItems",
                newName: "IX_AppInventoryItems_ProductId_StorageLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_ParentId",
                table: "AppCategories",
                newName: "IX_AppCategories_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppWarehouses",
                table: "AppWarehouses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppStockTransactionDetails",
                table: "AppStockTransactionDetails",
                columns: new[] { "StockTransactionId", "ProductId", "StorageLocationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppProducts",
                table: "AppProducts",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppInventoryItems",
                table: "AppInventoryItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AppCategories",
                table: "AppCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppCategories_AppCategories_ParentId",
                table: "AppCategories",
                column: "ParentId",
                principalTable: "AppCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppInventoryItems_AppProducts_ProductId",
                table: "AppInventoryItems",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppInventoryItems_AppStorageLocations_StorageLocationId",
                table: "AppInventoryItems",
                column: "StorageLocationId",
                principalTable: "AppStorageLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppProducts_AppCategories_CategoryId",
                table: "AppProducts",
                column: "CategoryId",
                principalTable: "AppCategories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppProducts_AppSuppliers_SupplierId",
                table: "AppProducts",
                column: "SupplierId",
                principalTable: "AppSuppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppStockTransactionDetails_AppProducts_ProductId",
                table: "AppStockTransactionDetails",
                column: "ProductId",
                principalTable: "AppProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppStockTransactionDetails_AppStockTransactions_StockTransactionId",
                table: "AppStockTransactionDetails",
                column: "StockTransactionId",
                principalTable: "AppStockTransactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AppStockTransactionDetails_AppStorageLocations_StorageLocationId",
                table: "AppStockTransactionDetails",
                column: "StorageLocationId",
                principalTable: "AppStorageLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AppStockTransactions_AppWarehouses_FromWarehouseId",
                table: "AppStockTransactions",
                column: "FromWarehouseId",
                principalTable: "AppWarehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppStockTransactions_AppWarehouses_ToWarehouseId",
                table: "AppStockTransactions",
                column: "ToWarehouseId",
                principalTable: "AppWarehouses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AppStorageLocations_AppWarehouses_WarehouseId",
                table: "AppStorageLocations",
                column: "WarehouseId",
                principalTable: "AppWarehouses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppCategories_AppCategories_ParentId",
                table: "AppCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_AppInventoryItems_AppProducts_ProductId",
                table: "AppInventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AppInventoryItems_AppStorageLocations_StorageLocationId",
                table: "AppInventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProducts_AppCategories_CategoryId",
                table: "AppProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_AppProducts_AppSuppliers_SupplierId",
                table: "AppProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_AppStockTransactionDetails_AppProducts_ProductId",
                table: "AppStockTransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_AppStockTransactionDetails_AppStockTransactions_StockTransactionId",
                table: "AppStockTransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_AppStockTransactionDetails_AppStorageLocations_StorageLocationId",
                table: "AppStockTransactionDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_AppStockTransactions_AppWarehouses_FromWarehouseId",
                table: "AppStockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_AppStockTransactions_AppWarehouses_ToWarehouseId",
                table: "AppStockTransactions");

            migrationBuilder.DropForeignKey(
                name: "FK_AppStorageLocations_AppWarehouses_WarehouseId",
                table: "AppStorageLocations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppWarehouses",
                table: "AppWarehouses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppStockTransactionDetails",
                table: "AppStockTransactionDetails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppProducts",
                table: "AppProducts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppInventoryItems",
                table: "AppInventoryItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AppCategories",
                table: "AppCategories");

            migrationBuilder.RenameTable(
                name: "AppWarehouses",
                newName: "Warehouses");

            migrationBuilder.RenameTable(
                name: "AppStockTransactionDetails",
                newName: "StockTransactionDetails");

            migrationBuilder.RenameTable(
                name: "AppProducts",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "AppInventoryItems",
                newName: "InventoryItems");

            migrationBuilder.RenameTable(
                name: "AppCategories",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_AppStockTransactionDetails_StorageLocationId",
                table: "StockTransactionDetails",
                newName: "IX_StockTransactionDetails_StorageLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_AppStockTransactionDetails_ProductId",
                table: "StockTransactionDetails",
                newName: "IX_StockTransactionDetails_ProductId");

            migrationBuilder.RenameIndex(
                name: "IX_AppStockTransactionDetails_BatchNumber",
                table: "StockTransactionDetails",
                newName: "IX_StockTransactionDetails_BatchNumber");

            migrationBuilder.RenameIndex(
                name: "IX_AppProducts_SupplierId",
                table: "Products",
                newName: "IX_Products_SupplierId");

            migrationBuilder.RenameIndex(
                name: "IX_AppProducts_Code",
                table: "Products",
                newName: "IX_Products_Code");

            migrationBuilder.RenameIndex(
                name: "IX_AppProducts_CategoryId",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_AppInventoryItems_StorageLocationId",
                table: "InventoryItems",
                newName: "IX_InventoryItems_StorageLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_AppInventoryItems_ProductId_StorageLocationId",
                table: "InventoryItems",
                newName: "IX_InventoryItems_ProductId_StorageLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_AppCategories_ParentId",
                table: "Categories",
                newName: "IX_Categories_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Warehouses",
                table: "Warehouses",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StockTransactionDetails",
                table: "StockTransactionDetails",
                columns: new[] { "StockTransactionId", "ProductId", "StorageLocationId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                table: "Products",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InventoryItems",
                table: "InventoryItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

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
                name: "FK_Categories_Categories_ParentId",
                table: "Categories",
                column: "ParentId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_AppStorageLocations_StorageLocationId",
                table: "InventoryItems",
                column: "StorageLocationId",
                principalTable: "AppStorageLocations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Products_ProductId",
                table: "InventoryItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_AppSuppliers_SupplierId",
                table: "Products",
                column: "SupplierId",
                principalTable: "AppSuppliers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryId",
                table: "Products",
                column: "CategoryId",
                principalTable: "Categories",
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

            migrationBuilder.AddForeignKey(
                name: "FK_StockTransactionDetails_Products_ProductId",
                table: "StockTransactionDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
