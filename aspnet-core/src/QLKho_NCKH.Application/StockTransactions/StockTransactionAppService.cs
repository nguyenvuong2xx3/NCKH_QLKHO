using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using QLKho_NCKH.EnumCustom;
using QLKho_NCKH.StockTransactions.Dtos;
using QLKho_NCKH.Suppliers.Dtos;
using QLKho_NCKH.Suppliers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YourProject.Domain.Transactions;
using QLKho_NCKH.Warehouses;
using System.Linq.Dynamic.Core;
using QLKho_NCKH.Warehouses.Dto;

namespace QLKho_NCKH.StockTransactions
{
	public class StockTransactionAppService : IStockTransactionAppService, IApplicationService
	{
		private readonly IRepository<StockTransaction, int> _stockTransactionRepository;
		private readonly IRepository<StockTransactionDetail, int> _stockTransactionDetailRepository;
		private readonly IRepository<Warehouse, int> _warehouseRepository;

		private readonly IUnitOfWorkManager _unitOfWorkManager;
		public StockTransactionAppService(IRepository<StockTransaction, int> stockTransactionRepository,
			IRepository<StockTransactionDetail, int> stockTransactionDetailRepository,
			IUnitOfWorkManager unitOfWorkManager,
			IRepository<Warehouse, int> warehouseRepository)
		{
			_stockTransactionRepository = stockTransactionRepository;
			_stockTransactionDetailRepository = stockTransactionDetailRepository;
			_unitOfWorkManager = unitOfWorkManager;
			_warehouseRepository = warehouseRepository;
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
				SupplierId = stockTransaction.SupplierId,
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
	}
}
