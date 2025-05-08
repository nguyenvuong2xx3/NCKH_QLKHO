using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Abp.Linq.Extensions;
using Abp.UI;
using Abp.Authorization;
using Abp.Auditing;
using Abp.Extensions;
using Abp.Domain.Uow;
using QLKho_NCKH.InventoryItems.Dto;
using QLKho_NCKH.Inventory;
using QLKho_NCKH.Products;

namespace QLKho_NCKH.InventoryItems;

public class InventoryItemAppService : ApplicationService, IInventoryItemAppService
{
 private readonly IRepository<InventoryItem, int> _inventoryItemRepository;
 //private readonly ITempFileCacheManager _tempFileCacheManager;
 //private readonly MediaTypeManager MediaTypeManager;

 public InventoryItemAppService(IRepository<InventoryItem, int> inventoryItemRepository
   //, IBlobContainerFactory blobContainerFactory
   //, ITempFileCacheManager tempFileCacheManager
   //, MediaTypeManager mediaTypeManager

 )
 {
   _inventoryItemRepository = inventoryItemRepository;
   //_tempFileCacheManager = tempFileCacheManager;
   //MediaTypeManager = mediaTypeManager;
  }


	public async Task CreateInventoryItem(InventoryItemCreatingInput input)
	{
		var getall = await _inventoryItemRepository.GetAllAsync();
		if (getall.Any(x => x.ProductId == input.ProductId && x.StorageLocationId == input.StorageLocationId))
		{
			var existingItem = getall.First(x => x.ProductId == input.ProductId && x.StorageLocationId == input.StorageLocationId);
			existingItem.Quantity += input.Quantity;
			await _inventoryItemRepository.UpdateAsync(existingItem);
		}
		else
		{
			var inventoryItem = new InventoryItem
			{
				ProductId = input.ProductId,
				StorageLocationId = input.StorageLocationId,
				Quantity = input.Quantity,
				ReservedQuantity = input.ReservedQuantity,
				UnitPrice = input.UnitPrice,
			};
			await _inventoryItemRepository.InsertAsync(inventoryItem);
		}
		
		
		//await CurrentUnitOfWork.SaveChangesAsync();

		//return new InventoryItemEditDto
		//{
		//	Id = inventoryItem.Id,
		//	ProductId = inventoryItem.ProductId,
		//	StorageLocationId = inventoryItem.StorageLocationId,
		//	Quantity = inventoryItem.Quantity,
		//	ReservedQuantity = inventoryItem.ReservedQuantity,
		//	UnitPrice = inventoryItem.UnitPrice,
		//};
	}

	//public async Task<PagedResultDto<InventoryItemListDto>> GetInventoryItems(GetInventoryItemsInput input)
	//{
	//	var query = _inventoryItemRepository.GetAll()
	//			.WhereIf(!string.IsNullOrEmpty(input.Filter), u => (true))


	//		  ;

	//	var count = await query.CountAsync();
	//	var result = await query.OrderBy(input.Sorting)
	//			.PageBy(input)
	//			.ToListAsync();
	//	var listDto = ObjectMapper.Map<List<InventoryItemListDto>>(result);
	//	return new PagedResultDto<InventoryItemListDto>(
	//			count,
	//			listDto
	//			);
	//}

	//public async Task<InventoryItemEditDto> GetInventoryItem(int id)
	//{
	//	var inventoryItem = await _inventoryItemRepository.GetAsync(id);

	//	return ObjectMapper.Map<InventoryItemEditDto>(inventoryItem);
	//}

	//public async Task<InventoryItemEditDto> EditInventoryItem(InventoryItemEditDto input)
	//{
	//	var inventoryItem = await _inventoryItemRepository.GetAsync(input.Id);
	//	inventoryItem.ProductId = input.ProductId;
	//	inventoryItem.StorageLocationId = input.StorageLocationId;
	//	inventoryItem.Quantity = input.Quantity;
	//	inventoryItem.ReservedQuantity = input.ReservedQuantity;
	//	inventoryItem.UnitPrice = input.UnitPrice;



	//	await CurrentUnitOfWork.SaveChangesAsync();

	//	return ObjectMapper.Map<InventoryItemEditDto>(inventoryItem);
	//}

	// Permission??
	//public async Task DeleteInventoryItem(int Id)
	//{
	//	await  _inventoryItemRepository.DeleteAsync(Id);
	//}


	}
	////InventoryItems AutoMapper
	//configuration.CreateMap<InventoryItem, InventoryItemEditDto>();
	//configuration.CreateMap<InventoryItem, InventoryItemListDto>();
	//configuration.CreateMap<InventoryItemCreatingInput, InventoryItem>();
