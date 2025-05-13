using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.EnumCustom;
using QLKho_NCKH.InventoryItems;
using QLKho_NCKH.InventoryItems.Dto;
using QLKho_NCKH.StockTransactionDetails;
using QLKho_NCKH.StockTransactions;
using QLKho_NCKH.StockTransactions.Dtos;
using QLKho_NCKH.Web.Models.StockTransactions;
using QLKho_NCKH.Web.Models.Users;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace QLKho_NCKH.Web.Controllers
{
	public class StockTransactionsController : QLKho_NCKHControllerBase
	{
		private readonly IStockTransactionAppService _stockTransactionAppService;
		private readonly IStockTransactionDetailAppService _stockTransactionDetailAppService;
		private readonly IInventoryItemAppService _inventoryItemAppService;
		private readonly IRepository<StockTransactionDetail, int> _stockTransactionDetailRepository;

		public StockTransactionsController(IStockTransactionAppService stockTransactionAppService, 
			IStockTransactionDetailAppService stockTransactionDetailAppService,
			IRepository<StockTransactionDetail, int> stockTransactionDetailRepository,
			IInventoryItemAppService inventoryItemAppService)
		{
			_stockTransactionAppService = stockTransactionAppService;
			_stockTransactionDetailRepository = stockTransactionDetailRepository;
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

			if (stockTransaction.Result.TransactionType == TransactionType.Import) {
				var viewmodel = new StockTransactionListViewModel
				{
					Id = stockTransaction.Result.Id,
					TransactionType = stockTransaction.Result.TransactionType,
					TransactionCode = stockTransaction.Result.TransactionCode,
					TransactionDate = stockTransaction.Result.TransactionDate,
					FromWarehouseId = stockTransaction.Result.FromWarehouseId ?? 0,
					ToWarehouseId = stockTransaction.Result.ToWarehouseId ?? 0,
					//SupplierId = stockTransaction.Result.SupplierId ?? 0,
					ReferenceNumber = stockTransaction.Result.ReferenceNumber,
					Note = stockTransaction.Result.Note,
					FromWarehouseName = stockTransaction.Result.FromWarehouseName,
					ToWarehouseName = stockTransaction.Result.ToWarehouseName,
					SupplierName = stockTransaction.Result.SupplierName,
					Status = stockTransaction.Result.Status,
					DetailProduct = stockTransaction.Result.DetailProduct
				};
				return PartialView("_EditStockTransactionModal", viewmodel);
			}
			if(stockTransaction.Result.TransactionType == TransactionType.Export)
			{
				var viewmodel = new StockTransactionListViewModel
				{
					Id = stockTransaction.Result.Id,
					TransactionType = stockTransaction.Result.TransactionType,
					TransactionCode = stockTransaction.Result.TransactionCode,
					TransactionDate = stockTransaction.Result.TransactionDate,
					FromWarehouseId = stockTransaction.Result.FromWarehouseId ?? 0,
					ToWarehouseId = stockTransaction.Result.ToWarehouseId ?? 0,
					CustomerName = stockTransaction.Result.CustomerName,
					ReferenceNumber = stockTransaction.Result.ReferenceNumber,
					Note = stockTransaction.Result.Note,
					FromWarehouseName = stockTransaction.Result.FromWarehouseName,
					ToWarehouseName = stockTransaction.Result.ToWarehouseName,
					SupplierName = stockTransaction.Result.SupplierName,
					Status = stockTransaction.Result.Status,
					DetailProduct = stockTransaction.Result.DetailProduct

				};
				return PartialView("_EditStockTransactionExportModal", viewmodel);
			}
			return Ok(new
			{
				success = true,
				message = "Updated successfully",
				data = (object)null // nếu có dữ liệu cần gửi về, thay thế null
			});
		}
		public async Task<IActionResult> UpdateImportStockTransactions([FromBody] StockTransactionUpdateInput input)
		{
			try
			{
				// 1. Cập nhật giao dịch
				await _stockTransactionAppService.Update(input);

				// 2. Lấy TẤT CẢ chi tiết giao dịch
				var transactionDetails = await _stockTransactionDetailRepository
						.GetAllListAsync(x => x.StockTransactionId == input.Id);

				if (!transactionDetails.Any())
				{
					return Ok(new { success = true, message = "Updated successfully (no items to inventory)" });
				}

				// 3. Cập nhật inventory cho từng sản phẩm
				foreach (var detail in transactionDetails)
				{
					var inventoryItemInput = new InventoryItemCreatingInput
					{
						ProductId = detail.ProductId,
						StorageLocationId = detail.StorageLocationId,
						Quantity = detail.Quantity,
						UnitPrice = detail.UnitPrice
					};

					await _inventoryItemAppService.CreateInventoryItem(inventoryItemInput);
				}

				return Ok(new { success = true, message = "Updated and inventory updated successfully" });
			}
			catch (Exception ex)
			{
				// Ghi log lỗi ở đây
				return StatusCode(500, new { success = false, message = ex.Message });
			}
		}
		public async Task<IActionResult> GetCustomerByStockTransactions (int id)
		{
			var getuser = await _stockTransactionAppService.GetCustomerByIdStockTransaction(id);
			var viewmodel = new UserViewModel
			{
				Id = getuser.Id,
				FullName = getuser.Name,
				EmailAddress = getuser.EmailAddress,
				//PhoneNumber = getuser.Result.PhoneNumber,
				//Address = getuser.Result.Address,
				//CompanyName = getuser.Result.CompanyName,
			};
			return PartialView("_CustomerDetailModal", viewmodel);
		}
	}
}
