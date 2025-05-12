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
using QLKho_NCKH.Products.Dtos;
using QLKho_NCKH.StockTransactions;
using QLKho_NCKH.Categories;
using Abp.Collections.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace QLKho_NCKH.InventoryItems;

public class InventoryItemAppService : ApplicationService, IInventoryItemAppService
{
	private readonly IRepository<InventoryItem, int> _inventoryItemRepository;
	private readonly IRepository<Category> _categoryRepository;

	//private readonly ITempFileCacheManager _tempFileCacheManager;
	//private readonly MediaTypeManager MediaTypeManager;

	public InventoryItemAppService(IRepository<InventoryItem, int> inventoryItemRepository
	, IRepository<Category> categoryRepository
	//, IBlobContainerFactory blobContainerFactory
	//, ITempFileCacheManager tempFileCacheManager
	//, MediaTypeManager mediaTypeManager

	)
	{
		_inventoryItemRepository = inventoryItemRepository;
		_categoryRepository = categoryRepository;
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


	//public async Task<PagedResultDto<InventoryItemListDto>> GetInventoryItemsProduct(GetInventoryItemsInput input)
	//{
	//	var query = _inventoryItemRepository.GetAll()
	//			 .Include(x => x.Product);

	//	var count = await query.CountAsync();
	//	var items = await query.OrderBy(input.Sorting)
	//			 .PageBy(input)
	//			 .ToListAsync();
	//	var result = items.Select(item => new InventoryItemListDto
	//	{
	//		ProductId = item.ProductId,
	//		ProductCode = item.Product.Code,
	//		StorageLocationId = item.StorageLocationId,
	//		StorageLocationCode = item.StorageLocation.Code,
	//		Quantity = item.Quantity,
	//		ReservedQuantity = item.ReservedQuantity,
	//		UnitPrice = item.UnitPrice,
	//		ProductName = item.Product.Name,
	//		ProductBarcode = item.Product.Barcode,
	//	}).ToList();
	//	return new PagedResultDto<InventoryItemListDto> { Items = result, TotalCount = count };

	//public async Task<PagedResultDto<InventoryItemListDto>> GetInventoryItems(GetInventoryItemsInput input)
	//{
	//	if (input.CategoryId != 0)
	//	{
	//		var getallcategory = await _categoryRepository.GetAllAsync();
	//		var categoryList = getallcategory.ToList();
	//	}

	//	//var query = _inventoryItemRepository.GetAll()
	//	//		.Include(x => x.Product)
	//	//		.Include(x => x.StorageLocation)
	//	//		.WhereIf(input.WarehouseId != 0, x => x.StorageLocation.WarehouseId == input.WarehouseId)
	//	//		.WhereIf(input.Filter != null, x => x.Product.Name.Contains(input.Filter))
	//	//    .OrderBy(x => x.ProductId) // đảm bảo trước khi group

	//	//// group để chỉ lấy một item theo mỗi ProductId
	//	//.GroupBy(x => x.ProductId)
	//	//.Select(g => g.FirstOrDefault());
	//	var query = _inventoryItemRepository.GetAll()
	//	.Include(x => x.Product)
	//	.Include(x => x.StorageLocation)
	//	.WhereIf(input.WarehouseId != 0, x => x.StorageLocation.WarehouseId == input.WarehouseId)
	//	.WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.Product.Name.Contains(input.Filter))
	//	.WhereIf(input.CategoryId != 0, x => x.Product.CategoryId == input.CategoryId);

	//	// Thực hiện truy vấn và đưa về memory
	//	var allItems = await query.ToListAsync();

	//	// Group theo ProductId và chỉ lấy 1 item cho mỗi ProductId
	//	var groupedItems = allItems
	//			.GroupBy(x => x.ProductId)
	//			.Select(g => g.First())
	//			.ToList();

	//	//if (input.CategoryId != 0)
	//	//{
	//	//	query = query.WhereIf(input.CategoryId != null, x => x.Product.CategoryId == input.CategoryId);
	//	//}
	//	var count = await groupedItems.CountAsync();
	//	var items = await query.OrderBy(input.Sorting)
	//			 .PageBy(input)
	//			 .ToListAsync();

	//	var result = items.Select(item => new InventoryItemListDto
	//	{
	//		ProductId = item.ProductId,
	//		ProductImage = item.Product.Image,
	//		ProductCode = item.Product.Code,
	//		Description = item.Product.Description,
	//		StorageLocationId = item.StorageLocationId,
	//		StorageLocationCode = item.StorageLocation.Code,
	//		Quantity = item.Quantity,
	//		ReservedQuantity = item.ReservedQuantity,
	//		UnitPrice = item.UnitPrice,
	//		ProductName = item.Product.Name,
	//		ProductBarcode = item.Product.Barcode,
	//	}).ToList();

	//	return new PagedResultDto<InventoryItemListDto> { Items = result, TotalCount = count };
	//}
	public async Task<PagedResultDto<InventoryItemListDto>> GetInventoryItems(GetInventoryItemsInput input)
	{
		// Bước 1: truy vấn dữ liệu đã lọc
		var query = _inventoryItemRepository.GetAll()
			.Include(x => x.Product)
			.Include(x => x.StorageLocation)
			.WhereIf(input.WarehouseId != 0, x => x.StorageLocation.WarehouseId == input.WarehouseId)
			.WhereIf(!string.IsNullOrEmpty(input.Filter), x => x.Product.Name.Contains(input.Filter))
			.WhereIf(input.CategoryId != 0, x => x.Product.CategoryId == input.CategoryId);

		// Bước 2: đưa vào bộ nhớ (ToListAsync)
		var allItems = await query.ToListAsync();

		// Bước 3: group theo ProductId để loại trùng, mỗi sản phẩm chỉ lấy 1 item
		var groupedItems = allItems
			.GroupBy(x => x.ProductId)
			.Select(g => g.First())
			.ToList();

		// Bước 4: đếm số lượng sau khi đã loại trùng
		var totalCount = groupedItems.Count;

		// Bước 5: phân trang thủ công sau khi đã group
		var pagedItems = groupedItems
			.AsQueryable()
			.OrderBy(input.Sorting ?? "ProductId") // tránh lỗi nếu input.Sorting null
			.PageBy(input)
			.ToList();

		// Bước 6: map sang DTO
		var result = pagedItems.Select(item => new InventoryItemListDto
		{
			ProductId = item.ProductId,
			ProductImage = item.Product.Image,
			ProductCode = item.Product.Code,
			Description = item.Product.Description,
			StorageLocationId = item.StorageLocationId,
			StorageLocationCode = item.StorageLocation.Code,
			Quantity = item.Quantity,
			ReservedQuantity = item.ReservedQuantity,
			UnitPrice = item.UnitPrice,
			ProductName = item.Product.Name,
			ProductBarcode = item.Product.Barcode,
		}).ToList();

		// Trả về kết quả
		return new PagedResultDto<InventoryItemListDto>
		{
			Items = result,
			TotalCount = totalCount
		};
	}


	public async Task<InventoryItemEditDto> GetInventoryItem(int productId)
	{
		var productDetails = _inventoryItemRepository.GetAll().Include(x => x.Product)
		.WhereIf(productId != 0, x => x.ProductId == productId)
		.ToList();
		return new InventoryItemEditDto
		{

			ProductId = productDetails.FirstOrDefault().ProductId,
			Volume = productDetails.FirstOrDefault().Product.Volume,
			Weight = productDetails.FirstOrDefault().Product.Weight,
			//StorageLocationId = productDetails.FirstOrDefault().StorageLocationId,
			Quantity = productDetails.FirstOrDefault().Quantity,
			//ReservedQuantity = productDetails.FirstOrDefault().ReservedQuantity,
			Unit = productDetails.FirstOrDefault().Product.Unit,
			UnitPrice = productDetails.FirstOrDefault().UnitPrice,
			ProductImage = productDetails.FirstOrDefault().Product.Image,
			ProductName = productDetails.FirstOrDefault().Product.Name,
			ProductBarcode = productDetails.FirstOrDefault().Product.Barcode,
			Description = productDetails.FirstOrDefault().Product.Description
		};
	}
	public async Task<PagedResultDto<InventoryItemListDto>> GetAllInventoryItems(GetInventoryItemsInput input)
	{
		// Query all inventory items with related data
		var query = _inventoryItemRepository.GetAll()
				.Include(x => x.Product)
				.Include(x => x.StorageLocation)
				.ThenInclude(x => x.Warehouse)
				.WhereIf(input.WarehouseId != 0, x => x.StorageLocation.WarehouseId == input.WarehouseId)
				.WhereIf(!string.IsNullOrEmpty(input.Filter), x =>
						x.Product.Name.Contains(input.Filter) ||
						x.Product.Code.Contains(input.Filter) ||
						x.Product.Barcode.Contains(input.Filter))
				.WhereIf(input.CategoryId != 0, x => x.Product.CategoryId == input.CategoryId);

		// Get total count before pagination
		var totalCount = await query.CountAsync();

		// Apply sorting and pagination
		var items = await query
				.OrderBy(input.Sorting ?? "Product.Name")
				.PageBy(input)
				.ToListAsync();

		// Map to DTO
		var result = items.Select(item => new InventoryItemListDto
		{
			ProductId = item.ProductId,
			ProductImage = item.Product.Image,
			ProductCode = item.Product.Code,
			Description = item.Product.Description,
			StorageLocationId = item.StorageLocationId,
			StorageLocationCode = item.StorageLocation.Code,
			WarehouseName = item.StorageLocation.Warehouse.Name,
			Quantity = item.Quantity,
			ReservedQuantity = item.ReservedQuantity,
			UnitPrice = item.UnitPrice,
			ProductName = item.Product.Name,
			ProductBarcode = item.Product.Barcode,
			// Add any additional fields you need
		}).ToList();

		return new PagedResultDto<InventoryItemListDto>
		{
			Items = result,
			TotalCount = totalCount
		};
	}
}
////InventoryItems AutoMapper
//configuration.CreateMap<InventoryItem, InventoryItemEditDto>();
//configuration.CreateMap<InventoryItem, InventoryItemListDto>();
//configuration.CreateMap<InventoryItemCreatingInput, InventoryItem>();
