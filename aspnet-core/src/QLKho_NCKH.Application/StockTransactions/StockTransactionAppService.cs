using Abp.Application.Services;
using Abp.Domain.Repositories;
using Castle.MicroKernel.Registration;
using QLKho_NCKH.StockTransactions.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourProject.Domain.Transactions;

namespace QLKho_NCKH.StockTransactions
{
	public class StockTransactionAppService : IStockTransactionAppService, IApplicationService
	{
		private readonly IRepository<StockTransaction, int> _stockTransactionRepository;
		public StockTransactionAppService(IRepository<StockTransaction, int> stockTransactionRepository)
		{
			_stockTransactionRepository = stockTransactionRepository;
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
	}
}
