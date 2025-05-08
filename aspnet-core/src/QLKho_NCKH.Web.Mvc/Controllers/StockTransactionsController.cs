using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.EnumCustom;
using QLKho_NCKH.InventoryItems;
using QLKho_NCKH.InventoryItems.Dto;
using QLKho_NCKH.StockTransactionDetails;
using QLKho_NCKH.StockTransactionDetails.Dto;
using QLKho_NCKH.StockTransactions;
using QLKho_NCKH.StockTransactions.Dtos;
using QLKho_NCKH.Web.Models.StockTransactions;
using QLKho_NCKH.Web.Models.Suppliers;
using System.Threading.Tasks;
using YourProject.Domain.Transactions;

namespace QLKho_NCKH.Web.Controllers
{
	public class StockTransactionsController : QLKho_NCKHControllerBase
	{
		private readonly IStockTransactionAppService _stockTransactionAppService;
		private readonly IStockTransactionDetailAppService _stockTransactionDetailAppService;
		private readonly IInventoryItemAppService _inventoryItemAppService;
		public StockTransactionsController(IStockTransactionAppService stockTransactionAppService, 
			IStockTransactionDetailAppService stockTransactionDetailAppService, 
			IInventoryItemAppService inventoryItemAppService)
		{
			_stockTransactionAppService = stockTransactionAppService;
			_stockTransactionDetailAppService = stockTransactionDetailAppService;
			_inventoryItemAppService = inventoryItemAppService;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult CreateImport()
		{
			return PartialView("_CreateImportModal");
		}

		public IActionResult Edit(int stockTransactionId)
		{
			var stockTransaction = _stockTransactionAppService.GetStockTransaction(stockTransactionId);

			var viewmodel = new StockTransactionListViewModel
			{
				Id = stockTransaction.Result.Id,
				TransactionType = stockTransaction.Result.TransactionType,
				TransactionCode = stockTransaction.Result.TransactionCode,
				TransactionDate = stockTransaction.Result.TransactionDate,
				FromWarehouseId = stockTransaction.Result.FromWarehouseId ?? 0,
				ToWarehouseId = stockTransaction.Result.ToWarehouseId ?? 0,
				SupplierId = stockTransaction.Result.SupplierId ?? 0,
				ReferenceNumber = stockTransaction.Result.ReferenceNumber,
				Note = stockTransaction.Result.Note,
				FromWarehouseName = stockTransaction.Result.FromWarehouseName,
				ToWarehouseName = stockTransaction.Result.ToWarehouseName,
				SupplierName = stockTransaction.Result.SupplierName,
				Status = stockTransaction.Result.Status
			};

			return PartialView("_EditStockTransactionModal", viewmodel);
		}
		public async Task<IActionResult> UpdateImportStockTransactions([FromBody] StockTransactionUpdateInput input)
		{
			 await _stockTransactionAppService.Update(input);
			
			//var query = await _stockTransactionAppService.GetStockTransaction(input.Id);
			var querydetail = await _stockTransactionDetailAppService.GetStockTransactionDetail(input.Id);
			var inventoryItemInput = new InventoryItemCreatingInput
			{
				ProductId = querydetail.ProductId,
				StorageLocationId = querydetail.StorageLocationId,
				Quantity = querydetail.Quantity,
				UnitPrice = querydetail.UnitPrice
			};
			await _inventoryItemAppService.CreateInventoryItem(inventoryItemInput);
			return Ok(new
			{
				success = true,
				message = "Updated successfully",
				data = (object)null // nếu có dữ liệu cần gửi về, thay thế null
			});
		}
	}
}
