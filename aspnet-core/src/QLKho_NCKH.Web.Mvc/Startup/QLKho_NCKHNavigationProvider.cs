using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using QLKho_NCKH.Authorization;

namespace QLKho_NCKH.Web.Startup
{
	/// <summary>
	/// This class defines menus for the application.
	/// </summary>
	public class QLKho_NCKHNavigationProvider : NavigationProvider
	{
		public override void SetNavigation(INavigationProviderContext context)
		{
			context.Manager.MainMenu
							.AddItem(
											new MenuItemDefinition(
															PageNames.Home,
															L("HomePage"),
															url: "",
															icon: "fas fa-home",
															requiresAuthentication: true
											)
							)
							.AddItem(
											new MenuItemDefinition(
															PageNames.Tenants,
															L("Tenants"),
															url: "Tenants",
															icon: "fas fa-building",
															permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Tenants)
											)
							)
							.AddItem(
											new MenuItemDefinition(
															PageNames.Users,
															L("Users"),
															url: "Users",
															icon: "fas fa-users",
															permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Users)
											)
							)
							.AddItem(
											new MenuItemDefinition(
															PageNames.Roles,
															L("Roles"),
															url: "Roles",
															icon: "fas fa-theater-masks",
															permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Roles)
											)
							)
							// Product Management - Parent menu with always visible children
							.AddItem(
											new MenuItemDefinition(
															"ProductManagement",
															L("ProductManagement"),
															icon: "fas fa-boxes"
											).AddItem(
															new MenuItemDefinition(
																			PageNames.Products,
																			L("Products"),
																			url: "Products",
																			icon: "fas fa-box",
																			permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Products)
															)
											).AddItem(
															new MenuItemDefinition(
																			PageNames.Categories,
																			L("Categories"),
																			url: "Categories",
																			icon: "fas fa-tags",
																			permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Categories)
															)
											)
							)
							// Inventory Management - Parent menu with always visible children
							.AddItem(
											new MenuItemDefinition(
															"InventoryManagement",
															L("InventoryManagement"),
															icon: "fas fa-warehouse"
											).AddItem(
															new MenuItemDefinition(
																			PageNames.Suppliers,
																			L("Suppliers"),
																			url: "Suppliers",
																			icon: "fas fa-truck",
																			permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Suppliers)
															)
											).AddItem(
															new MenuItemDefinition(
																			PageNames.Warehouses,
																			L("Warehouses"),
																			url: "Warehouses",
																			icon: "fas fa-warehouse",
																			permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Warehouses)
															)
											).AddItem(
															new MenuItemDefinition(
																			PageNames.StorageLocations,
																			L("StorageLocations"),
																			url: "StorageLocations",
																			icon: "fas fa-map-marker-alt",
																			permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_StorageLocations)
															)
											).AddItem(
															new MenuItemDefinition(
																			PageNames.StockTransactions,
																			L("StockTransactions"),
																			url: "StockTransactions",
																			icon: "fas fa-exchange-alt",
																			permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_StockTransactions)
															)
											).AddItem(
															new MenuItemDefinition(
																			PageNames.InventoryItems,
																			L("InventoryItems"),
																			url: "InventoryItems",
																			icon: "fas fa-exchange-alt",
																			permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_InventoryItems)
															)
							//.AddItem(
							//				new MenuItemDefinition(
							//								PageNames.Customers,
							//								L("Customers"),
							//								url: "Customers",
							//								icon: "fas fa-users"
							//				//permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Customers)
							//				)
							//)
							//).AddItem(
							//				new MenuItemDefinition(
							//								PageNames.Customers,
							//								L("Customers"),
							//								url: "Customers",
							//								icon: "fas fa-users"
							//				//permissionDependency: new SimplePermissionDependency(PermissionNames.Pages_Customers)
							//				)
							)
							)
							.AddItem(
											new MenuItemDefinition(
															PageNames.About,
															L("About"),
															url: "About",
															icon: "fas fa-info-circle"
											)
							);
		}

		private static ILocalizableString L(string name)
		{
			return new LocalizableString(name, QLKho_NCKHConsts.LocalizationSourceName);
		}
	}
}