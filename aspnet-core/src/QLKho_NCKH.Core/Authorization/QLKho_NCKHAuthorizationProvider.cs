using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace QLKho_NCKH.Authorization
{
    public class QLKho_NCKHAuthorizationProvider : AuthorizationProvider
    {
		public override void SetPermissions(IPermissionDefinitionContext context)
		{
			// Tạo nhóm permission chung
			var administration = context.CreatePermission(PermissionNames.Pages_Tenants, L("Tenants"));

			// Quản lý người dùng
			var users = context.CreatePermission(PermissionNames.Pages_Users, L("Users"));
			users.CreateChildPermission(PermissionNames.Pages_Users_Create, L("CreateNewUser"));
			users.CreateChildPermission(PermissionNames.Pages_Users_Edit, L("EditUser"));
			users.CreateChildPermission(PermissionNames.Pages_Users_Delete, L("DeleteUser"));
			users.CreateChildPermission(PermissionNames.Pages_Users_Activation, L("Activation"));

			// Quản lý vai trò
			var roles = context.CreatePermission(PermissionNames.Pages_Roles, L("Roles"));
			roles.CreateChildPermission(PermissionNames.Pages_Roles_Create, L("CreateNewRole"));
			roles.CreateChildPermission(PermissionNames.Pages_Roles_Edit, L("EditRole"));
			roles.CreateChildPermission(PermissionNames.Pages_Roles_Delete, L("DeleteRole"));

			// Quản lý sản phẩm
			var products = context.CreatePermission(PermissionNames.Pages_Products, L("Products"));
			products.CreateChildPermission(PermissionNames.Pages_Products_Create, L("CreateProduct"));
			products.CreateChildPermission(PermissionNames.Pages_Products_Edit, L("EditProduct"));
			products.CreateChildPermission(PermissionNames.Pages_Products_Delete, L("DeleteProduct"));

			// Quản lý danh mục
			var categories = context.CreatePermission(PermissionNames.Pages_Categories, L("Categories"));
			categories.CreateChildPermission(PermissionNames.Pages_Categories_Create, L("CreateCategory"));
			categories.CreateChildPermission(PermissionNames.Pages_Categories_Edit, L("EditCategory"));
			categories.CreateChildPermission(PermissionNames.Pages_Categories_Delete, L("DeleteCategory"));

			// Quản lý nhà cung cấp
			var suppliers = context.CreatePermission(PermissionNames.Pages_Suppliers, L("Suppliers"));
			suppliers.CreateChildPermission(PermissionNames.Pages_Suppliers_Create, L("CreateSupplier"));
			suppliers.CreateChildPermission(PermissionNames.Pages_Suppliers_Edit, L("EditSupplier"));
			suppliers.CreateChildPermission(PermissionNames.Pages_Suppliers_Delete, L("DeleteSupplier"));

			// Quản lý kho
			var warehouses = context.CreatePermission(PermissionNames.Pages_Warehouses, L("Warehouses"));
			warehouses.CreateChildPermission(PermissionNames.Pages_Warehouses_Create, L("CreateWarehouse"));
			warehouses.CreateChildPermission(PermissionNames.Pages_Warehouses_Edit, L("EditWarehouse"));
			warehouses.CreateChildPermission(PermissionNames.Pages_Warehouses_Delete, L("DeleteWarehouse"));

			// Quản lý vị trí lưu trữ
			var storageLocations = context.CreatePermission(PermissionNames.Pages_StorageLocations, L("StorageLocations"));
			storageLocations.CreateChildPermission(PermissionNames.Pages_StorageLocations_Create, L("CreateStorageLocation"));
			storageLocations.CreateChildPermission(PermissionNames.Pages_StorageLocations_Edit, L("EditStorageLocation"));
			storageLocations.CreateChildPermission(PermissionNames.Pages_StorageLocations_Delete, L("DeleteStorageLocation"));

			// Quản lý giao dịch kho
			var stockTransactions = context.CreatePermission(PermissionNames.Pages_StockTransactions, L("StockTransactions"));
			stockTransactions.CreateChildPermission(PermissionNames.Pages_StockTransactions_Create, L("CreateStockTransaction"));
			stockTransactions.CreateChildPermission(PermissionNames.Pages_StockTransactions_Edit, L("EditStockTransaction"));
			stockTransactions.CreateChildPermission(PermissionNames.Pages_StockTransactions_Delete, L("DeleteStockTransaction"));

			// Quản lý khách hàng
			var customers = context.CreatePermission(PermissionNames.Pages_Customers, L("Customers"));
			customers.CreateChildPermission(PermissionNames.Pages_Customers_Create, L("CreateCustomer"));
			customers.CreateChildPermission(PermissionNames.Pages_Customers_Edit, L("EditCustomer"));
			customers.CreateChildPermission(PermissionNames.Pages_Customers_Delete, L("DeleteCustomer"));

			// Các permission khác
			context.CreatePermission(PermissionNames.Pages_Reports, L("Reports"));
			context.CreatePermission(PermissionNames.Pages_Dashboard, L("Dashboard"));
		}

		private static ILocalizableString L(string name)
		{
			return new LocalizableString(name, QLKho_NCKHConsts.LocalizationSourceName);
		}
	}
}
