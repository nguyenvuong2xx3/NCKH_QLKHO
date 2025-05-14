using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using QLKho_NCKH.Controllers;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Controllers;
using QLKho_NCKH.InventoryItems.Dto;
using QLKho_NCKH.InventoryItems;
using QLKho_NCKH.StockTransactions.Dtos;
using QLKho_NCKH.StockTransactions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using QLKho_NCKH.EnumCustom;
using QLKho_NCKH.Web.Models.Home;

namespace QLKho_NCKH.Web.Controllers
{
    [AbpMvcAuthorize]
		public class HomeController : QLKho_NCKHControllerBase
		{
			private readonly IStockTransactionAppService _stockTransactionAppService;
			private readonly IInventoryItemAppService _inventoryItemAppService;

			public HomeController(
					IStockTransactionAppService stockTransactionAppService,
					IInventoryItemAppService inventoryItemAppService)
			{
				_stockTransactionAppService = stockTransactionAppService;
				_inventoryItemAppService = inventoryItemAppService;
			}

			public async Task<IActionResult> Index()
			{
				// Get data within a Unit of Work
				var inventoryStats = await _inventoryItemAppService.GetAllInventoryItems(new GetInventoryItemsInput());
				var transactions = await _stockTransactionAppService.GetStockTransactions(new GetStockTransactionsInput());

				// Calculate stats
				var totalItems = inventoryStats.Items.Sum(x => x.Quantity);
				var totalProducts = inventoryStats.Items.Count;
				var pendingTransactions = transactions.Items.Count(x => x.Status == TransactionStatusEnum.Pending);
				var completedTransactions = transactions.Items.Count(x => x.Status == TransactionStatusEnum.Approved);

				// Prepare view model
				var model = new DashboardViewModel
				{
					TotalItems = totalItems,
					TotalProducts = totalProducts,
					PendingTransactions = pendingTransactions,
					CompletedTransactions = completedTransactions,
					RecentTransactions = transactions.Items.OrderByDescending(x => x.CreationTime).Take(5).ToList(),
					InventoryStats = inventoryStats
				};

				return View(model);
			}
		}

		
}
