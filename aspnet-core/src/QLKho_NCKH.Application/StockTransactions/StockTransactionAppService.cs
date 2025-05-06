using Abp.Application.Services;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.EnumCustom;
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
		private readonly IRepository<StockTransactionDetail, int> _stockTransactionDetailRepository;
		public StockTransactionAppService(IRepository<StockTransaction, int> stockTransactionRepository, IRepository<StockTransactionDetail, int> stockTransactionDetailRepository)
		{
			_stockTransactionRepository = stockTransactionRepository;
			_stockTransactionDetailRepository = stockTransactionDetailRepository;
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
		//[HttpPost]
		public async Task<IActionResult> CreateImportRequest(CreateImportRequestDto input)
		{
			//using (var transaction = await _unitOfWork.BeginTransactionAsync())
			//{
			//	try
			//	{
			// 1. Lưu master (StockTransaction)
			var master = new StockTransaction
			{
				TransactionType = TransactionType.Import,
				//Status = RequestStatus.Draft,
				ToWarehouseId = input.WarehouseId,
				SupplierId = input.SupplierId
			};
			await _stockTransactionRepository.InsertAsync(master);

			// 2. Lưu details (StockTransactionDetail)
			foreach (var detailDto in input.ImportRequestDetails)
			{
				var detail = new StockTransactionDetail
				{
					StockTransactionId = master.Id,
					ProductId = detailDto.ProductId,
					Quantity = detailDto.Quantity,
					StorageLocationId = detailDto.StorageLocationId,
					//BatchNumber = detailDto.BatchNumber
				};
				await _stockTransactionDetailRepository.InsertAsync(detail);
			}

			return new OkObjectResult(new
			{
				TransactionCode = master.TransactionCode,
				TransactionDate = master.TransactionDate,
				FromWarehouseName = master.FromWarehouse?.Name,
				ToWarehouseName = master.ToWarehouse?.Name,
				ReferenceNumber = master.ReferenceNumber,
				Note = master.Note
			});
		}
	}
}
