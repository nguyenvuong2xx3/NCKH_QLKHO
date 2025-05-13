using Abp.Domain.Uow;
using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Carts;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.OrderDetails;
using QLKho_NCKH.Orders;
using QLKho_NCKH.Products;
using System.Linq;
using System.Threading.Tasks;
using QLKho_NCKH.StockTransactions;
using QLKho_NCKH.StockTransactionDetails;
using QLKho_NCKH.Web.Models;
using QLKho_NCKH.StockTransactions.Dtos;
using System.Collections.Generic;
using QLKho_NCKH.EnumCustom;
using Abp.Application.Services.Dto;
using System;

namespace QLKho_NCKH.Web.Controllers
{
	public class OrdersController : QLKho_NCKHControllerBase
	{
		private readonly IOrderAppService _orderAppService;
		private readonly IOrderDetailAppService _orderDetailAppService;
		private readonly ICartAppService _cartAppService;
		private readonly IProductAppService _productAppService;
		private readonly IStockTransactionAppService _stockTransactionAppService;
		private readonly IStockTransactionDetailAppService _stockTransactionDetailAppService;




		public OrdersController(IOrderAppService orderAppService
			, IOrderDetailAppService orderDetailAppService
			, ICartAppService cartAppService
			, IProductAppService productAppService
			, IStockTransactionDetailAppService stockTransactionDetailAppService
			, IStockTransactionAppService stockTransactionAppService
			)
		{
			_orderAppService = orderAppService;
			_orderDetailAppService = orderDetailAppService;
			_cartAppService = cartAppService;
			_productAppService = productAppService;
			_stockTransactionDetailAppService = stockTransactionDetailAppService;
			_stockTransactionAppService = stockTransactionAppService;
		}

		//public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequestDto input)
		//{
		//	if (input == null || input.Items == null || !input.Items.Any())
		//	{
		//		return Json(new { success = false, message = "Dữ liệu không hợp lệ!" });
		//	}

		//	try
		//	{
		//		// Tạo đơn hàng
		//		var newOrder = await _orderAppService.CreateOrder(new CreateOrderDto
		//		{
		//			UserId = input.UserId,
		//			NameUser = input.NameUser,
		//			TotalAmount = input.TotalAmount,
		//			DiscountAmount = input.DiscountAmount,
		//			PaymentMethod = input.PaymentMethod,
		//			OrderStatus = 0
		//		});
		//		// Lưu từng sản phẩm vào bảng OrderDetails
		//		foreach (var item in input.Items)
		//		{
		//			await _orderDetailAppService.CreateOrderDetail(new OrderDetails.Dto.OrderDetailDto
		//			{
		//				OrderId = newOrder,
		//				ProductId = item.ProductId,
		//				Quantity = item.Quantity,
		//				UnitPrice = item.UnitPrice,
		//				DiscountPrice = item.DiscountPrice,
		//			});
		//		}

		//		await _cartAppService.ClearCart(input.UserId);

		//		// Trả về OrderId để hiển thị trong trang thành công
		//		return Json(new { orderId = newOrder });
		//	}
		//	catch (Exception ex)
		//	{
		//		return StatusCode(500, "Lỗi khi đặt hàng: " + ex.Message);
		//	}
		//}

		//public async Task<IActionResult> Success(int orderId)
		//{
		//	//var order = await _orderAppService.GetOrderById(orderId);
		//	//var orderDetail = await _orderDetailAppService.GetOrderListById(orderId);
		//	//var productIds = orderDetail.Select(od => od.ProductId).ToList();
		//	////var products = await _productAppService.GetProductByIds(productIds);

		//	//var model = new OrderViewSuccess
		//	//{
		//	//	Order = order,
		//	//	OrderListDetail = orderDetail,
		//	//	//ProductList = products,
		//	//};

		//	return View("Success", model);
		//}
		public IActionResult Success(string orderCode, int userId, TransactionStatusEnum? status)
		{
			ViewBag.OrderCode = orderCode ?? "Đang cập nhật";
			ViewBag.UserId = userId;
			ViewBag.Status = status ?? TransactionStatusEnum.Pending;
			return View();
		}

		// OrdersController.cs
		//public async Task<IActionResult> History(GetStockTransactionsInput input)
		//{

		//	// Lấy thông tin đơn hàng
		//	var order = await _stockTransactionAppService.GetStockTransactions(input);

		//	var orderListId = order.Items.Select(x => x.Id).ToList();

		//	foreach (var item in order.Items)
		//	{
		//		item.DetailProduct = await _stockTransactionDetailAppService.GetStockTransactionDetailById(item.Id);
		//	}

		//	// Lấy thông tin khách hàng
		//	//var customer = await _stockTransactionAppService.GetCustomerByIdStockTransaction(id);

		//	var model = new OrderDetailViewModel
		//	{
		//		Order = order,
		//		OrderDetails = order.DetailProduct.ToList(),
		//		Customer = customer
		//	};

		//	return View(model);
		//}

		//public async Task<IActionResult> History(GetStockTransactionsInput input)
		//{
		//	try
		//	{
		//		// Kiểm tra và khởi tạo input nếu null
		//		input ??= new GetStockTransactionsInput();

		//		// Get stock transactions with filtering
		//		var transactions = await _stockTransactionAppService.GetStockTransactions(input)
		//				?? new PagedResultDto<StockTransactionListDto>();

		//		// Prepare list to hold transaction details with products
		//		var transactionDetails = new List<StockTransactionDetailViewModel>();

		//		if (transactions.Items != null)
		//		{
		//			foreach (var transaction in transactions.Items)
		//			{
		//				if (transaction == null) continue;

		//				// Get the detailed transaction with products
		//				var detailedTransaction = await _stockTransactionAppService.GetStockTransaction(transaction.Id);

		//				if (detailedTransaction?.DetailProduct == null) continue;

		//				var products = detailedTransaction.DetailProduct
		//						.Where(p => p != null)
		//						.Select(p => new TransactionProductViewModel
		//						{
		//							ProductId = p.ProductId,
		//							ProductName = p.ProductName ?? "Không có tên",
		//							ProductCode = p.ProductCode ?? "N/A",
		//							Quantity = p.Quantity,
		//							UnitPrice = p.UnitPrice,
		//							TotalPrice = p.Quantity * p.UnitPrice,
		//							StorageLocation = p.StorageLocationCode ?? "N/A",
		//							BatchNumber = p.BatchNumber ?? "N/A",
		//							ExpiryDate = p.ExpiryDate
		//						}).ToList();

		//				transactionDetails.Add(new StockTransactionDetailViewModel
		//				{
		//					TransactionId = transaction.Id,
		//					TransactionCode = transaction.TransactionCode ?? "N/A",
		//					TransactionDate = transaction.TransactionDate,
		//					TransactionType = transaction.TransactionType,
		//					Status = transaction.Status,
		//					FromWarehouse = transaction.FromWarehouseName ?? "N/A",
		//					ToWarehouse = transaction.ToWarehouseName ?? "N/A",
		//					Products = products
		//				});
		//			}
		//		}

		//		var model = new StockTransactionHistoryViewModel
		//		{
		//			Transactions = transactionDetails,
		//			Filter = input.Filter,
		//			StartTime = input.StartTime,
		//			EndTime = input.EndTime,
		//			Status = input.Status
		//		};

		//		return View(model);
		//	}
		//	catch (Exception )
		//	{
		//		// Log lỗi ở đây
		//		// _logger.LogError(ex, "Error in History action");

		//		// Trả về trang lỗi hoặc view trống
		//		return View(new StockTransactionHistoryViewModel());
		//	}
		//}

		//public async Task<IActionResult> History(GetStockTransactionsInput input)
		//{
		//	try
		//	{
		//		input ??= new GetStockTransactionsInput();

		//		// Đảm bảo UserId được set
		//		if (input.UserId == 0 && AbpSession.UserId.HasValue)
		//		{
		//			input.UserId = (int)AbpSession.UserId.Value;
		//		}

		//		var transactions = await _stockTransactionAppService.GetStockTransactions(input)
		//				?? new PagedResultDto<StockTransactionListDto>();

		//		var transactionDetails = transactions.Items?
		//				.Where(t => t != null)
		//				.Select(async t =>
		//				{
		//					var detailed = await _stockTransactionAppService.GetStockTransaction(t.Id);
		//					return new StockTransactionDetailViewModel
		//					{
		//						TransactionId = t.Id,
		//						TransactionCode = t.TransactionCode ?? "N/A",
		//						TransactionDate = t.TransactionDate,
		//						TransactionType = t.TransactionType,
		//						Status = t.Status,
		//						FromWarehouse = t.FromWarehouseName ?? "N/A",
		//						ToWarehouse = t.ToWarehouseName ?? "N/A",
		//						Products = detailed?.DetailProduct?
		//									.Where(p => p != null)
		//									.Select(p => new TransactionProductViewModel
		//									{
		//										ProductId = p.ProductId,
		//										ProductName = p.ProductName ?? "Không có tên",
		//										ProductCode = p.ProductCode ?? "N/A",
		//										Quantity = p.Quantity,
		//										UnitPrice = p.UnitPrice,
		//										TotalPrice = p.Quantity * p.UnitPrice,
		//										StorageLocation = p.StorageLocationCode ?? "N/A",
		//										BatchNumber = p.BatchNumber ?? "N/A",
		//										ExpiryDate = p.ExpiryDate
		//									}).ToList() ?? new List<TransactionProductViewModel>()
		//					};
		//				})
		//				.Select(t => t.Result)
		//				.Where(t => t != null)
		//				.ToList();

		//		var model = new StockTransactionHistoryViewModel
		//		{
		//			Transactions = transactionDetails ?? new List<StockTransactionDetailViewModel>(),
		//			Filter = input.Filter,
		//			StartTime = input.StartTime,
		//			EndTime = input.EndTime,
		//			Status = input.Status
		//		};

		//		return View(model);
		//	}
		//	catch (Exception ex)
		//	{
		//		// Log lỗi
		//		Logger.Error("Error in History action", ex);
		//		return View(new StockTransactionHistoryViewModel());
		//	}
		//}

		//public async Task<IActionResult> History(GetStockTransactionsInput input)
		//{
		//	try
		//	{
		//		input ??= new GetStockTransactionsInput();

		//		// Đảm bảo UserId được set
		//		if (input.UserId == 0 && AbpSession.UserId.HasValue)
		//		{
		//			input.UserId = AbpSession.UserId.Value;
		//		}

		//		var model = new StockTransactionHistoryViewModel
		//		{
		//			Transactions = (await GetTransactionDetails(input)) ?? new List<StockTransactionDetailViewModel>(),
		//			Filter = input.Filter,
		//			StartTime = input.StartTime,
		//			EndTime = input.EndTime,
		//			Status = input.Status
		//		};

		//		return View(model);
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Error("Error in History action", ex);
		//		return View(new StockTransactionHistoryViewModel()); // Trả về model rỗng
		//	}
		//}

		//public async Task<IActionResult> History(GetStockTransactionsInput input)
		//{
		//	try
		//	{
		//		input ??= new GetStockTransactionsInput();

		//		if (input.UserId == 0 && AbpSession.UserId.HasValue)
		//		{
		//			input.UserId = (int)AbpSession.UserId.Value;
		//		}

		//		var transactionDetails = await GetTransactionDetails(input);
		//		var model = new StockTransactionHistoryViewModel
		//		{
		//			Transactions = transactionDetails ?? new List<StockTransactionDetailViewModel>(),
		//			Filter = input.Filter ?? string.Empty,
		//			StartTime = input.StartTime,
		//			EndTime = input.EndTime,
		//			Status = input.Status
		//		};

		//		return View(model);
		//	}
		//	catch (Exception ex)
		//	{
		//		Logger.Error("Error in History action", ex);
		//		return View(new StockTransactionHistoryViewModel());
		//	}
		//}

		public async Task<IActionResult> History(GetStockTransactionsInput input)
		{
			try
			{
				input ??= new GetStockTransactionsInput();

				if (input.UserId == 0 && AbpSession.UserId.HasValue)
				{
					input.UserId = (int)AbpSession.UserId.Value;
				}

				// Lấy dữ liệu phân trang từ service
				var pagedResult = await _stockTransactionAppService.GetStockTransactions(input);

				// Chuyển đổi sang ViewModel
				var transactionDetails = new List<StockTransactionDetailViewModel>();
				if (pagedResult?.Items != null)
				{
					foreach (var transaction in pagedResult.Items.Where(t => t != null))
					{
						var detailed = await _stockTransactionAppService.GetStockTransaction(transaction.Id);
						if (detailed == null) continue;

						transactionDetails.Add(new StockTransactionDetailViewModel
						{
							TransactionId = transaction.Id,
							TransactionCode = transaction.TransactionCode ?? "N/A",
							TransactionDate = transaction.TransactionDate,
							TransactionType = transaction.TransactionType,
							Status = transaction.Status,
							FromWarehouse = transaction.FromWarehouseName ?? "N/A",
							ToWarehouse = transaction.ToWarehouseName ?? "N/A",
							Products = detailed.DetailProduct?
										.Where(p => p != null)
										.Select(p => new TransactionProductViewModel
										{
											ProductId = p.ProductId,
											ProductName = p.ProductName ?? "Không có tên",
											ProductCode = p.ProductCode ?? "N/A",
											Quantity = p.Quantity,
											UnitPrice = p.UnitPrice,
											TotalPrice = p.Quantity * p.UnitPrice,
											StorageLocation = p.StorageLocationCode ?? "N/A"
										}).ToList() ?? new List<TransactionProductViewModel>()
						});
					}
				}

				var model = new StockTransactionHistoryViewModel
				{
					Transactions = transactionDetails,
					Filter = input.Filter ?? string.Empty,
					StartTime = input.StartTime,
					EndTime = input.EndTime,
					Status = input.Status,
					// Thêm thông tin phân trang
					TotalCount = pagedResult?.TotalCount ?? 0,
					CurrentPage = input.SkipCount / input.MaxResultCount + 1,
					PageSize = input.MaxResultCount
				};

				return View(model);
			}
			catch (Exception ex)
			{
				Logger.Error("Error in History action", ex);
				return View(new StockTransactionHistoryViewModel());
			}
		}

		public async Task<List<StockTransactionDetailViewModel>> GetTransactionDetails(GetStockTransactionsInput input)
		{
			var transactions = await _stockTransactionAppService.GetStockTransactions(input);
			if (transactions == null || transactions.Items == null)
			{
				return new List<StockTransactionDetailViewModel>();
			}

			var transactionDetails = new List<StockTransactionDetailViewModel>();

			foreach (var transaction in transactions.Items.Where(t => t != null))
			{
				var detailed = await _stockTransactionAppService.GetStockTransaction(transaction.Id);
				if (detailed == null) continue;

				var viewModel = new StockTransactionDetailViewModel
				{
					TransactionId = transaction.Id,
					TransactionCode = transaction.TransactionCode ?? "N/A",
					TransactionDate = transaction.TransactionDate,
					TransactionType = transaction.TransactionType,
					Status = transaction.Status,
					FromWarehouse = transaction.FromWarehouseName ?? "N/A",
					ToWarehouse = transaction.ToWarehouseName ?? "N/A",
					Products = detailed.DetailProduct?
								.Where(p => p != null)
								.Select(p => new TransactionProductViewModel
								{
									ProductId = p.ProductId,
									ProductName = p.ProductName ?? "Không có tên",
									ProductCode = p.ProductCode ?? "N/A",
									Quantity = p.Quantity,
									UnitPrice = p.UnitPrice,
									TotalPrice = p.Quantity * p.UnitPrice,
									StorageLocation = p.StorageLocationCode ?? "N/A"
								}).ToList() ?? new List<TransactionProductViewModel>()
				};

				transactionDetails.Add(viewModel);
			}

			return transactionDetails;
		}

	}
}
