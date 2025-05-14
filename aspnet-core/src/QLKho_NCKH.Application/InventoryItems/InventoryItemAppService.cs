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
using Abp.Timing;

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
		if (input.CategoryId != 0)
		{
			var getallcategory = await _categoryRepository.GetAllAsync();
			var categoryList = getallcategory.ToList();
		}

		var query = _inventoryItemRepository.GetAll()
				.Include(x => x.Product)
				.Include(x => x.StorageLocation)
				.WhereIf(input.WarehouseId != 0, x => x.StorageLocation.WarehouseId == input.WarehouseId)
				.WhereIf(input.Filter != null, x => x.Product.Name.Contains(input.Filter));

		if (input.CategoryId != 0)
		{
			query = query.WhereIf(input.CategoryId != null, x => x.Product.CategoryId == input.CategoryId);
		}

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
		//var query = _inventoryItemRepository.GetAll()
		//		.Include(x => x.Product)
		//		.Include(x => x.StorageLocation)
		//		.ThenInclude(x => x.Warehouse)
		//		.WhereIf(input.WarehouseId != 0, x => x.StorageLocation.WarehouseId == input.WarehouseId)
		//		.WhereIf(!string.IsNullOrEmpty(input.Filter), x =>
		//				x.Product.Name.Contains(input.Filter) ||
		//				x.Product.Code.Contains(input.Filter) ||
		//				x.Product.Barcode.Contains(input.Filter))
		//		.WhereIf(input.CategoryId != 0, x => x.Product.CategoryId == input.CategoryId);

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
	public async Task DeleteInventoryItem(DeleteInventoryItemInput input)
	{
		// Validate input
		if (input.ProductId <= 0 || input.StorageLocationId <= 0)
		{
			throw new UserFriendlyException("Thông tin sản phẩm hoặc vị trí kho không hợp lệ!");
		}

		// Check if the item exists
		var inventoryItem = await _inventoryItemRepository.FirstOrDefaultAsync(
				x => x.ProductId == input.ProductId &&
						 x.StorageLocationId == input.StorageLocationId);

		if (inventoryItem == null)
		{
			throw new UserFriendlyException("Không tìm thấy tồn kho cần xóa!");
		}

		// Check if there's any reserved quantity (prevent deletion if items are reserved)
		if (inventoryItem.ReservedQuantity > 0)
		{
			throw new UserFriendlyException(
					"Không thể xóa tồn kho khi có số lượng đã được đặt trước. Vui lòng giải phóng số lượng đặt trước trước khi xóa.");
		}

		// Additional business validation can be added here

		await _inventoryItemRepository.DeleteAsync(inventoryItem);
	}
	public async Task<InventoryItemListDto> UpdateInventoryItem(UpdateInventoryItemInput input)
	{
		// Validate input
		if (input.ProductId <= 0 || input.StorageLocationId <= 0)
		{
			throw new UserFriendlyException("Thông tin sản phẩm hoặc vị trí kho không hợp lệ!");
		}

		// Check if the item exists
		var inventoryItem = await _inventoryItemRepository.FirstOrDefaultAsync(
				x => x.ProductId == input.ProductId &&
						 x.StorageLocationId == input.StorageLocationId);

		if (inventoryItem == null)
		{
			throw new UserFriendlyException("Không tìm thấy tồn kho cần cập nhật!");
		}

		// Validate quantity changes
		if (input.Quantity < 0)
		{
			throw new UserFriendlyException("Số lượng tồn kho không thể âm!");
		}

		//if (input.ReservedQuantity < 0)
		//{
		//	throw new UserFriendlyException("Số lượng đặt trước không thể âm!");
		//}

		//if (input.ReservedQuantity > input.Quantity)
		//{
		//	throw new UserFriendlyException("Số lượng đặt trước không thể lớn hơn số lượng tồn kho!");
		//}

		// Check if the new quantity is less than current reserved quantity
		if (input.Quantity < inventoryItem.ReservedQuantity)
		{
			throw new UserFriendlyException(
					$"Số lượng tồn kho mới ({input.Quantity}) không thể nhỏ hơn số lượng đã đặt trước ({inventoryItem.ReservedQuantity})");
		}

		if (input.UnitPrice != null)
		{
			inventoryItem.UnitPrice = input.UnitPrice.Value;
		}
		// Update fields
		inventoryItem.Quantity = input.Quantity;
		//inventoryItem.ReservedQuantity = input.ReservedQuantity;
		inventoryItem.LastModificationTime = Clock.Now;
		inventoryItem.LastModifierUserId = AbpSession.UserId;

		// Additional fields can be updated here

		await _inventoryItemRepository.UpdateAsync(inventoryItem);

		// Return updated DTO
		return new InventoryItemListDto
		{
			ProductId = inventoryItem.ProductId,
			StorageLocationId = inventoryItem.StorageLocationId,
			Quantity = inventoryItem.Quantity,
			ReservedQuantity = inventoryItem.ReservedQuantity,
			UnitPrice = inventoryItem.UnitPrice,
			// Include any other fields you need
		};
	}
	public async Task<GetInventoryItemForEditInput> GetInventoryItemForEdit(GetInventoryItemForEditInput input)
	{
		// Get the inventory item with all related data
		var inventoryItem = await _inventoryItemRepository.GetAll()
				.Include(x => x.Product)
				.Include(x => x.StorageLocation)
				.ThenInclude(x => x.Warehouse)
				.Where(x => x.ProductId == input.ProductId && x.StorageLocationId == input.StorageLocationId)
				.FirstOrDefaultAsync();

		if (inventoryItem == null)
		{
			throw new UserFriendlyException("Không tìm thấy tồn kho!");
		}

		// Map to DTO
		return new GetInventoryItemForEditInput
		{
			ProductId = inventoryItem.ProductId,
			ProductCode = inventoryItem.Product.Code,
			ProductName = inventoryItem.Product.Name,
			ProductBarcode = inventoryItem.Product.Barcode,
			StorageLocationId = inventoryItem.StorageLocationId,
			StorageLocationCode = inventoryItem.StorageLocation.Code,
			WarehouseName = inventoryItem.StorageLocation.Warehouse.Name,
			Quantity = inventoryItem.Quantity,
			ReservedQuantity = inventoryItem.ReservedQuantity,
			UnitPrice = inventoryItem.UnitPrice
		};
	}
}
////InventoryItems AutoMapper
//configuration.CreateMap<InventoryItem, InventoryItemEditDto>();
//configuration.CreateMap<InventoryItem, InventoryItemListDto>();
//configuration.CreateMap<InventoryItemCreatingInput, InventoryItem>();
