using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using QLKho_NCKH.EnumCustom;
using QLKho_NCKH.StockTransactions.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using QLKho_NCKH.Warehouses;
using System.Linq.Dynamic.Core;
using QLKho_NCKH.Warehouses.Dto;
using QLKho_NCKH.Inventory;
using Abp.UI;

namespace QLKho_NCKH.StockTransactions
{
	public class StockTransactionAppService : IStockTransactionAppService, IApplicationService
	{
		private readonly IRepository<StockTransaction, int> _stockTransactionRepository;
		private readonly IRepository<StockTransactionDetail, int> _stockTransactionDetailRepository;
		private readonly IRepository<Warehouse, int> _warehouseRepository;
		private readonly IRepository<InventoryItem, int> _inventoryItemRepository;


		private readonly IUnitOfWorkManager _unitOfWorkManager;
		public StockTransactionAppService(IRepository<StockTransaction, int> stockTransactionRepository,
			IRepository<StockTransactionDetail, int> stockTransactionDetailRepository,
			IUnitOfWorkManager unitOfWorkManager,
			IRepository<Warehouse, int> warehouseRepository,
			IRepository<InventoryItem, int> inventoryItemRepository)
		{
			_stockTransactionRepository = stockTransactionRepository;
			_stockTransactionDetailRepository = stockTransactionDetailRepository;
			_unitOfWorkManager = unitOfWorkManager;
			_warehouseRepository = warehouseRepository;
			_inventoryItemRepository = inventoryItemRepository;
		}
		public async Task<StockTransactionDto> CreateStockTransactionImport(CreateStockTransactionImportDto input)
		{
			var stockTransaction = new StockTransaction
			{
				TransactionCode = input.TransactionCode,
				TransactionDate = input.TransactionDate,
				ToWarehouseId = input.ToWarehouseId,
				SupplierId = input.SupplierId,
				ReferenceNumber = string.IsNullOrEmpty(input.ReferenceNumber)
						? $"NK-{DateTime.Now:yyyyMMddHHmmss}"
						: input.ReferenceNumber,
				Note = input.Note
			};
			await _stockTransactionRepository.InsertAsync(stockTransaction);
			return new StockTransactionDto
			{
				TransactionCode = stockTransaction.TransactionCode,
				TransactionDate = stockTransaction.TransactionDate,
				FromWarehouseName = stockTransaction.FromWarehouse?.Name,
				ToWarehouseName = stockTransaction.ToWarehouse?.Name,
				ReferenceNumber = stockTransaction.ReferenceNumber,
				Note = stockTransaction.Note
			};
		}
		public async Task CreateImportRequest(CreateImportRequestDto input)
		{

			var master = new StockTransaction
			{
				TransactionCode = input.TransactionCode,
				TransactionType = TransactionType.Import,
				//Status = RequestStatus.Draft,
				ToWarehouseId = input.ToWarehouseId,
				SupplierId = input.SupplierId,
				ReferenceNumber = input.ReferenceNumber,
				Note = input.Note,
			};
			await _stockTransactionRepository.InsertAsync(master);
			await _unitOfWorkManager.Current.SaveChangesAsync();

			// 2. Lưu details (StockTransactionDetail)
			foreach (var detailDto in input.ImportRequestDetails)
			{
				var detail = new StockTransactionDetail
				{
					StockTransactionId = master.Id,
					ProductId = detailDto.ProductId,
					Quantity = detailDto.Quantity,
					StorageLocationId = detailDto.StorageLocationId,
					UnitPrice = detailDto.UnitPrice

				};
				await _stockTransactionDetailRepository.InsertAsync(detail);
			}
		}
		public async Task CreateExportRequest(ExportInputDto input)
		{
			// Kiểm tra đầu vào
			if (input.ExportRequestDetails == null || !input.ExportRequestDetails.Any())
			{
				throw new UserFriendlyException("Danh sách sản phẩm không được để trống.");
			}

			foreach (var product in input.ExportRequestDetails)
			{
				if (product.Quantity <= 0)
				{
					throw new UserFriendlyException("Số lượng sản phẩm phải lớn hơn 0.");
				}
			}

			// Lấy tất cả thông tin InventoryItems liên quan đến các ProductIds
			var productIds = input.ExportRequestDetails.Select(d => d.ProductId).ToList();
			var inventoryItems = await _inventoryItemRepository.GetAll()
					.Include(x => x.StorageLocation)
					.Where(x => productIds.Contains(x.ProductId))
					.ToListAsync();

			if (!inventoryItems.Any())
			{
				throw new UserFriendlyException("Không tìm thấy thông tin sản phẩm trong hệ thống.");
			}

			// Phân nhóm sản phẩm theo WarehouseId
			var groupedByWarehouse = input.ExportRequestDetails
					.GroupBy(detail =>
					{
						// Lấy InventoryItem phù hợp và chọn một vị trí lưu trữ
						var inventoryItemsForProduct = inventoryItems
							.Where(x => x.ProductId == detail.ProductId)
							.OrderByDescending(x => x.Quantity) // Ưu tiên vị trí có số lượng lớn nhất
							.ToList();

						var selectedInventoryItem = inventoryItemsForProduct.FirstOrDefault();
						return selectedInventoryItem?.StorageLocation.WarehouseId;
					})
					.Where(group => group.Key.HasValue) // Loại bỏ các nhóm không có WarehouseId
					.ToList();

			// Tạo đơn hàng tương ứng cho từng kho
			foreach (var group in groupedByWarehouse)
			{
				var warehouseId = group.Key.Value; // WarehouseId không null vì đã lọc ở trên
				var groupedDetails = group.ToList();

				// Tạo đơn hàng (StockTransaction) mới
				var stockTransaction = new StockTransaction
				{
					TransactionCode = input.TransactionCode, // Có thể tạo TransactionCode riêng cho từng kho nếu cần
					TransactionType = TransactionType.Export,
					FromWarehouseId = warehouseId,
					UserId = input.CustomerId,
					ReferenceNumber = input.ReferenceNumber,
					Note = input.Note,
				};

				await _stockTransactionRepository.InsertAsync(stockTransaction);
				await _unitOfWorkManager.Current.SaveChangesAsync();

				// Thêm chi tiết sản phẩm cho đơn hàng (StockTransactionDetail)
				foreach (var detail in groupedDetails)
				{
					// Lấy InventoryItem phù hợp với sản phẩm
					var inventoryItemsForProduct = inventoryItems
							.Where(x => x.ProductId == detail.ProductId)
							.OrderByDescending(x => x.Quantity) // Ưu tiên vị trí có số lượng lớn nhất
							.ToList();

					var selectedInventoryItem = inventoryItemsForProduct.FirstOrDefault();

					if (selectedInventoryItem == null)
					{
						throw new UserFriendlyException($"Không tìm thấy vị trí lưu trữ phù hợp cho sản phẩm {detail.ProductId}.");
					}

					var transactionDetail = new StockTransactionDetail
					{
						StockTransactionId = stockTransaction.Id,
						ProductId = detail.ProductId,
						Quantity = detail.Quantity,
						UnitPrice = detail.UnitPrice,
						StorageLocationId = selectedInventoryItem.StorageLocationId, // Chọn vị trí lưu trữ
					};

					await _stockTransactionDetailRepository.InsertAsync(transactionDetail);
				}

				await _unitOfWorkManager.Current.SaveChangesAsync();
			}
		}
		//public async Task CreateExportRequest(ExportInputDto input)
		//{
		//	if (input.ExportRequestDetails == null || !input.ExportRequestDetails.Any())
		//	{
		//		throw new UserFriendlyException("Danh sách sản phẩm không được để trống.");
		//	}
		//	foreach (var product in input.ExportRequestDetails)
		//	{
		//		if (product.Quantity <= 0)
		//		{
		//			throw new UserFriendlyException("Số lượng sản phẩm phải lớn hơn 0.");
		//		}
		//	}// Lấy tất cả thông tin InventoryItems liên quan đến các sản phẩm trong ExportRequestDetails
		//	var productIds = input.ExportRequestDetails.Select(d => d.ProductId).ToList();

		//	var getAllInventory = await _inventoryItemRepository.GetAll()
		//			.Include(x => x.StorageLocation)
		//			.Where(x => productIds.Contains(x.ProductId))
		//			.ToListAsync();
		//	// Phân nhóm sản phẩm theo WarehouseId
		//	var groupedByWarehouse = input.ExportRequestDetails
		//			.GroupBy(detail =>
		//			{
		//				var inventoryItem = getAllInventory.FirstOrDefault(x => x.ProductId == detail.ProductId);

		//				return inventoryItem?.StorageLocation.WarehouseId;
		//			})
		//			.ToList();
		//	// Tạo đơn hàng tương ứng cho từng kho
		//	foreach (var group in groupedByWarehouse)
		//	{
		//		var warehouseId = group.Key;
		//		var groupedDetails = group.ToList();

		//		// Tạo đơn hàng mới
		//		var stockTransaction = new StockTransaction
		//		{
		//			TransactionCode = input.TransactionCode, // Bạn có thể tạo mã giao dịch riêng cho từng kho nếu cần
		//			TransactionType = TransactionType.Export,
		//			FromWarehouseId = warehouseId,
		//			UserId = input.CustomerId,
		//			ReferenceNumber = input.ReferenceNumber,
		//			Note = input.Note,
		//		};

		//		await _stockTransactionRepository.InsertAsync(stockTransaction);
		//		await _unitOfWorkManager.Current.SaveChangesAsync();

		//		// Thêm chi tiết sản phẩm cho đơn hàng này
		//		foreach (var detail in groupedDetails)
		//		{
		//			var transactionDetail = new StockTransactionDetail
		//			{
		//				StockTransactionId = stockTransaction.Id,
		//				ProductId = detail.ProductId,
		//				Quantity = detail.Quantity,
		//				// Không cần StorageLocationId nếu không truyền vào
		//			};

		//			await _stockTransactionDetailRepository.InsertAsync(transactionDetail);
		//		}

		//		await _unitOfWorkManager.Current.SaveChangesAsync();
		//	}
		//}
		////tao 1 yc xuat kho
		//var master = new StockTransaction
		//{
		//	TransactionCode = input.TransactionCode,
		//	TransactionType = TransactionType.Export,
		//	//Status = RequestStatus.Draft,
		//	FromWarehouseId = input.FromWarehouseId,
		//	//SupplierId = input.SupplierId,
		//	UserId = input.CustomerId,
		//	ReferenceNumber = input.ReferenceNumber,
		//	Note = input.Note,
		//};
		//await _stockTransactionRepository.InsertAsync(master);
		//await _unitOfWorkManager.Current.SaveChangesAsync();

		////luu san pham xuat kho
		//// 2. Lưu details (StockTransactionDetail)
		//foreach (var detailDto in input.ExportRequestDetails)
		//{
		//	var inventory = await _inventoryItemRepository.GetAll()
		//			.Where(x => x.ProductId == detailDto.ProductId && x.StorageLocationId == detailDto.StorageLocationId)
		//			.FirstOrDefaultAsync();

		//	if (inventory == null)
		//	{
		//		throw new Exception($"Không tìm thấy sản phẩm với ID {detailDto.ProductId} trong kho.");
		//	}

		//	if (inventory.Quantity < detailDto.Quantity)
		//	{
		//		throw new Exception($"Số lượng sản phẩm với ID {detailDto.ProductId} trong kho không đủ để xuất.");
		//	}

		//	//// Trừ số lượng trong kho  
		//	//inventory.Quantity -= detailDto.Quantity;
		//	//await _inventoryItemRepository.UpdateAsync(inventory);

		//	// Lưu chi tiết giao dịch xuất kho  
		//	var detail = new StockTransactionDetail
		//	{
		//		StockTransactionId = master.Id,
		//		ProductId = detailDto.ProductId,
		//		Quantity = detailDto.Quantity,
		//		StorageLocationId = detailDto.StorageLocationId,
		//		UnitPrice = detailDto.UnitPrice
		//	};
		//	await _stockTransactionDetailRepository.InsertAsync(detail);
		//}


		public async Task<PagedResultDto<StockTransactionListDto>> GetStockTransactions(GetStockTransactionsInput input)
		{
			var getAll = await _warehouseRepository.GetAllAsync();
			var query = _stockTransactionRepository.GetAll()
					.WhereIf(!string.IsNullOrEmpty(input.Filter), u => u.TransactionCode.Contains(input.Filter)
					|| u.ReferenceNumber.Contains(input.Filter) || u.Note.Contains(input.Filter))
					.WhereIf(input.Status.HasValue,
						u => u.Status == input.Status.Value) // Thêm điều kiện lọc theo status
					.WhereIf(input.StartTime.HasValue && input.EndTime.HasValue, u => u.CreationTime >= input.StartTime.Value && u.CreationTime <= input.EndTime.Value);


			var count = await query.CountAsync();
			var result = await query.OrderByDescending(x => x.CreationTime)
					.PageBy(input)
					.ToListAsync();

			var stockTransactionsDtos = new List<StockTransactionListDto>();

			foreach (var item in result)
			{
				var warehouses = await GetWarehousesByTransaction(item.Id);

				stockTransactionsDtos.Add(new StockTransactionListDto
				{
					Id = item.Id,
					TransactionCode = item.TransactionCode,
					TransactionType = item.TransactionType,
					TransactionDate = item.TransactionDate,
					FromWarehouseId = item.FromWarehouseId ?? 0,
					ToWarehouseId = item.ToWarehouseId ?? 0,
					SupplierId = item.SupplierId,
					ReferenceNumber = item.ReferenceNumber,
					Note = item.Note,
					Status = item.Status,
					FromWarehouseName = warehouses.FromWarehouse?.Name,
					ToWarehouseName = warehouses.ToWarehouse?.Name,
					CreationTime = item.CreationTime
				});
			}

			return new PagedResultDto<StockTransactionListDto>()
			{
				TotalCount = count,
				Items = stockTransactionsDtos
			};
		}
		public async Task<StockTransactionListDto> GetStockTransaction(int id)
		{
			var stockTransaction = await _stockTransactionRepository
					.GetAll()
					.Include(x => x.Supplier)
					.Include(x => x.User)
					.FirstOrDefaultAsync(x => x.Id == id);

			var warehouses = await GetWarehousesByTransaction(id);

			return new StockTransactionListDto
			{
				Id = stockTransaction.Id,
				TransactionCode = stockTransaction.TransactionCode,
				TransactionType = stockTransaction.TransactionType,
				TransactionDate = stockTransaction.TransactionDate,
				FromWarehouseId = stockTransaction.FromWarehouseId ?? 0,
				ToWarehouseId = stockTransaction.ToWarehouseId ?? 0,
				//SupplierId = stockTransaction.SupplierId,
				CustomerName = stockTransaction.User?.Name,
				SupplierName = stockTransaction.Supplier?.Name,
				ReferenceNumber = stockTransaction.ReferenceNumber,
				Note = stockTransaction.Note,
				Status = stockTransaction.Status,
				FromWarehouseName = warehouses.FromWarehouse?.Name,
				ToWarehouseName = warehouses.ToWarehouse?.Name
			};
		}

		public async Task Update(StockTransactionUpdateInput input)
		{
			var query = await _stockTransactionRepository.GetAsync(input.Id);
			if (query != null)
			{
				query.Status = TransactionStatusEnum.Approved;
				await _stockTransactionRepository.UpdateAsync(query);
			}
			else
			{
				throw new Exception("Không tìm thấy giao dịch với ID đã cho.");
			}
		}

		public async Task UpdateExportStockTransactions(StockTransactionUpdateInput input)
		{
			using (var unitOfWork = _unitOfWorkManager.Begin())
			{

				// 1. Cập nhật trạng thái giao dịch
				var transaction = await _stockTransactionRepository.GetAsync(input.Id);
				// Cập nhật các trường cần thiết
				transaction.Status = TransactionStatusEnum.Approved;
				await _stockTransactionRepository.UpdateAsync(transaction);

				// 2. Lấy danh sách chi tiết giao dịch
				var transactionDetails = await _stockTransactionDetailRepository.GetAllListAsync(x => x.StockTransactionId == input.Id);

				// 3. Cập nhật số lượng tồn kho
				foreach (var detail in transactionDetails)
				{
					var inventoryItem = await _inventoryItemRepository.FirstOrDefaultAsync(x =>
							x.ProductId == detail.ProductId &&
							x.StorageLocationId == detail.StorageLocationId);

					if (inventoryItem == null)
					{
						throw new UserFriendlyException("Sản phẩm không có trong kho");
					}

					if (inventoryItem.Quantity < detail.Quantity)
					{
						throw new UserFriendlyException("Số lượng sản phẩm không đủ để xuất");
					}

					inventoryItem.Quantity -= detail.Quantity;
					await _inventoryItemRepository.UpdateAsync(inventoryItem);
				}

				await unitOfWork.CompleteAsync();
			}
		}

		public async Task<WarehouseFormTo> GetWarehousesByTransaction(int stockTransactionId)
		{
			var stockTransaction = await _stockTransactionRepository.GetAsync(stockTransactionId);
			if (stockTransaction == null)
			{
				throw new Exception("Stock transaction not found.");
			}

			var fromWarehouse = stockTransaction.FromWarehouseId.HasValue
					? await _warehouseRepository.GetAsync(stockTransaction.FromWarehouseId.Value)
					: null;

			var toWarehouse = stockTransaction.ToWarehouseId.HasValue
					? await _warehouseRepository.GetAsync(stockTransaction.ToWarehouseId.Value)
					: null;

			return new WarehouseFormTo
			{
				FromWarehouse = fromWarehouse != null ? new WarehouseDetailDto
				{
					Id = fromWarehouse.Id,
					Name = fromWarehouse.Name,
					Location = fromWarehouse.Location
				} : null,
				ToWarehouse = toWarehouse != null ? new WarehouseDetailDto
				{
					Id = toWarehouse.Id,
					Name = toWarehouse.Name,
					Location = toWarehouse.Location
				} : null
			};
		}
		public async Task DeleteStockTransaction(int Id)
		{
			await _stockTransactionRepository.DeleteAsync(Id);
			var query = await _stockTransactionDetailRepository.FirstOrDefaultAsync(x => x.StockTransactionId == Id);
			await _stockTransactionDetailRepository.DeleteAsync(query);
		}
	}
}
