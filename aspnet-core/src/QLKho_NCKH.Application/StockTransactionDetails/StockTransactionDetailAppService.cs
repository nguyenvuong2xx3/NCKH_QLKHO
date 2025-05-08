using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using AutoMapper.Internal.Mappers;
using Castle.MicroKernel.Registration;
using QLKho_NCKH.Inventory;
using QLKho_NCKH.StockTransactionDetails.Dto;
using QLKho_NCKH.StockTransactions;
using QLKho_NCKH.StockTransactions.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YourProject.Domain.Transactions;

namespace QLKho_NCKH.StockTransactionDetails
{
	public class StockTransactionDetailAppService : IApplicationService, IStockTransactionDetailAppService
	{
		private readonly IRepository<StockTransactionDetail, int> _stockTransactionDetailRepository;
		private readonly IRepository<StockTransaction, int> _stockTransactionRepository;
		private readonly IRepository<InventoryItem, int> _inventoryItemRepository;
		private readonly IUnitOfWorkManager _unitOfWorkManager;
		//private readonly ITempFileCacheManager _tempFileCacheManager;
		//private readonly MediaTypeManager MediaTypeManager;

		public StockTransactionDetailAppService(IRepository<StockTransactionDetail, int> stockTransactionDetailRepository
			, IRepository<InventoryItem, int> inventoryItemRepository
			,
IUnitOfWorkManager unitOfWorkManager
		//, IBlobContainerFactory blobContainerFactory
		//, ITempFileCacheManager tempFileCacheManager
		//, MediaTypeManager mediaTypeManager

		)
		{
			_stockTransactionDetailRepository = stockTransactionDetailRepository;
			_inventoryItemRepository = inventoryItemRepository;
			_unitOfWorkManager = unitOfWorkManager;
			//_tempFileCacheManager = tempFileCacheManager;
			//MediaTypeManager = mediaTypeManager;
		}


		public async Task<StockTransactionDetailEditDto> CreateStockTransactionDetail(StockTransactionDetailCreatingInput input)
		{
			// Map input DTO to entity  
			var stockTransactionDetail = new StockTransactionDetail
			{
				StockTransactionId = input.StockTransactionId,
				ProductId = input.ProductId,
				StorageLocationId = input.StorageLocationId,
				Quantity = input.Quantity,
				UnitPrice = input.UnitPrice,
				Note = input.Note,
				BatchNumber = input.BatchNumber,
				ExpiryDate = input.ExpiryDate,
				//TotalPrice = input.Quantity * input.UnitPrice // Calculate total price  
			};

			// Insert the entity into the repository  
			await _stockTransactionDetailRepository.InsertAsync(stockTransactionDetail);

			// Save changes to the database  
			//await CurrentUnitOfWork.SaveChangesAsync();

			// Map the entity back to the output DTO and return  
			return new StockTransactionDetailEditDto
			{
				Id = stockTransactionDetail.Id,
				StockTransactionId = stockTransactionDetail.StockTransactionId,
				ProductId = stockTransactionDetail.ProductId,
				StorageLocationId = stockTransactionDetail.StorageLocationId,
				Quantity = stockTransactionDetail.Quantity,
				UnitPrice = stockTransactionDetail.UnitPrice,
				Note = stockTransactionDetail.Note,
				BatchNumber = stockTransactionDetail.BatchNumber,
				ExpiryDate = stockTransactionDetail.ExpiryDate
			};
		}
		public async Task<StockTransactionDetailCreatingInput> GetStockTransactionDetail(int id)
		{
			var stockTransactionDetail = await _stockTransactionDetailRepository.FirstOrDefaultAsync(x => x.StockTransactionId == id);

			if (stockTransactionDetail == null)
			{
				throw new EntityNotFoundException($"Không tìm thấy chi tiết giao dịch kho với ID {id}");
			}

			return new StockTransactionDetailCreatingInput
			{
				StockTransactionId = stockTransactionDetail.StockTransactionId,
				ProductId = stockTransactionDetail.ProductId,
				StorageLocationId = stockTransactionDetail.StorageLocationId,
				Quantity = stockTransactionDetail.Quantity,
				UnitPrice = stockTransactionDetail.UnitPrice,
				Note = stockTransactionDetail.Note,
				ExpiryDate = stockTransactionDetail.ExpiryDate,
				BatchNumber = stockTransactionDetail.BatchNumber
			};
		}

		
	}
}

//public async Task<StockTransactionDetailEditDto> GetStockTransactionDetail(int id)
//{
//	var stockTransactionDetail = await _stockTransactionDetailRepository.GetAsync(id);

//	return ObjectMapper.Map<StockTransactionDetailEditDto>(stockTransactionDetail);
//}

//public async Task<StockTransactionDetailEditDto> EditStockTransactionDetail(StockTransactionDetailEditDto input)
//{
//	var stockTransactionDetail = await _stockTransactionDetailRepository.GetAsync(input.Id);
//	stockTransactionDetail.StockTransactionId = input.StockTransactionId;
//	stockTransactionDetail.ProductId = input.ProductId;
//	stockTransactionDetail.StorageLocationId = input.StorageLocationId;
//	stockTransactionDetail.Quantity = input.Quantity;
//	stockTransactionDetail.UnitPrice = input.UnitPrice;
//	stockTransactionDetail.Note = input.Note;
//	stockTransactionDetail.BatchNumber = input.BatchNumber;
//	stockTransactionDetail.ExpiryDate = input.ExpiryDate;



//	await CurrentUnitOfWork.SaveChangesAsync();

//	return ObjectMapper.Map<StockTransactionDetailEditDto>(stockTransactionDetail);
//}

//// Permission??
//public async Task DeleteStockTransactionDetail(int Id)
//{
//	await _stockTransactionDetailRepository.DeleteAsync(Id);
//}

