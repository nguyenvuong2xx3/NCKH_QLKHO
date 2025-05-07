using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QLKho_NCKH.EnumCustom;
using QLKho_NCKH.StockTransactions.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
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
	}
}
