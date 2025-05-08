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

namespace QLKho_NCKH.StockTransactions
{
	public class StockTransactionAppService : IStockTransactionAppService, IApplicationService
	{
		private readonly IRepository<StockTransaction, int> _stockTransactionRepository;
		private readonly IRepository<StockTransactionDetail, int> _stockTransactionDetailRepository;
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		public StockTransactionAppService(IRepository<StockTransaction, int> stockTransactionRepository, IRepository<StockTransactionDetail, int> stockTransactionDetailRepository, IUnitOfWorkManager unitOfWorkManager)
		{
			_stockTransactionRepository = stockTransactionRepository;
			_stockTransactionDetailRepository = stockTransactionDetailRepository;
			_unitOfWorkManager = unitOfWorkManager;
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
			var query = _stockTransactionRepository.GetAll()
					.WhereIf(!string.IsNullOrEmpty(input.Filter), u => u.TransactionCode.Contains(input.Filter)
					|| u.ReferenceNumber.Contains(input.Filter) || u.Note.Contains(input.Filter));

			var count = await query.CountAsync();
			var result = await query.OrderByDescending(x => x.CreationTime)
			.PageBy(input)
			.ToListAsync();

			var StockTransactionsDtos = new List<StockTransactionListDto>();

			foreach (var item in result)
			{
				StockTransactionsDtos.Add(new StockTransactionListDto
				{

					Id = item.Id,
					TransactionCode = item.TransactionCode,
					TransactionDate = item.TransactionDate,
					FromWarehouseId = item.FromWarehouseId ?? 0,
					ToWarehouseId = item.ToWarehouseId ?? 0,
					SupplierId = item.SupplierId,
					ReferenceNumber = item.ReferenceNumber,
					Note = item.Note,
					Status = item.Status
				});
			}
			return new PagedResultDto<StockTransactionListDto>()
			{
				TotalCount = count,
				Items = StockTransactionsDtos
			};

		}
		public async Task<StockTransactionListDto> GetStockTransaction(int id)
		{
			var stockTransaction = await _stockTransactionRepository.GetAsync(id);

			return new StockTransactionListDto
			{
				Id = stockTransaction.Id,
				TransactionCode = stockTransaction.TransactionCode,
				TransactionDate = stockTransaction.TransactionDate,
				FromWarehouseId = stockTransaction.FromWarehouseId ?? 0,
				ToWarehouseId = stockTransaction.ToWarehouseId ?? 0,
				SupplierId = stockTransaction.SupplierId,
				ReferenceNumber = stockTransaction.ReferenceNumber,
				Note = stockTransaction.Note,
				Status = stockTransaction.Status, // Không cần cast nếu kiểu đúng
				//TransactionType = stockTransaction.TransactionType.ToString() // Enum to string (nếu là enum)
			};
		}

		public async Task Update(StockTransactionUpdateInput input)
		{
			var query = await _stockTransactionRepository.GetAsync(input.Id);
			if (query != null)
			{
				query.Status = TransactionStatusEnum.Draft;
				await _stockTransactionRepository.UpdateAsync(query);
			}
			else
			{
				throw new Exception("Không tìm thấy giao dịch với ID đã cho.");
			}
		}
	}
}
