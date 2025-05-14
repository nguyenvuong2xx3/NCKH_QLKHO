using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using QLKho_NCKH.Authorization;
using QLKho_NCKH.StorageLocations.Dto;
using QLKho_NCKH.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLKho_NCKH.StorageLocations
{
	[AbpMvcAuthorize(PermissionNames.Pages_StorageLocations)]

	public class StorageLocationAppService : IStorageLocationAppService, IApplicationService
	{
		private readonly IRepository<StorageLocation, int> _storageLocationRepository;
		//private readonly IUnitOfWorkManager _unitOfWorkManager;
		public StorageLocationAppService(IRepository<StorageLocation, int> storageLocationRepository) //, IUnitOfWorkManager unitOfWorkManager
		{
			_storageLocationRepository = storageLocationRepository;
			//_unitOfWorkManager = unitOfWorkManager;
		}
		[AbpMvcAuthorize(PermissionNames.Pages_StorageLocations_Create)]

		public async Task<storageLocationDto> CreateAsync(CreateStorageLocationDto input)
		{
			StorageLocation storageLocation = new StorageLocation
			{
				Code = input.Code,
				WarehouseId = input.WarehouseId,
				Capacity = input.Capacity,
				CurrentVolume = input.CurrentVolume,
				IsAvailable = input.IsAvailable
			};
			await _storageLocationRepository.InsertAsync(storageLocation);
			return new storageLocationDto
			{
				Id = storageLocation.Id,
				Code = storageLocation.Code,
				WarehouseId = storageLocation.WarehouseId,
				Capacity = storageLocation.Capacity,
				CurrentVolume = storageLocation.CurrentVolume,
				IsAvailable = storageLocation.IsAvailable
			};
		}

		public async Task DeleteAsync(int id)
		{
			await _storageLocationRepository.DeleteAsync(id);
		}

		public async Task<PagedResultDto<StorageLocationListDto>> GetAllAsync(StorageLocationInput input)
		{
			// Lấy queryable (không await) để còn áp dụng filter
			var query = _storageLocationRepository
					.GetAll()                          // hoặc GetAllIncluding nếu hỗ trợ include trực tiếp
					.Include(x => x.Warehouse)         // join Warehouse để lấy tên

					// Lọc theo mã nếu có filter text
					.WhereIf(!input.Filter.IsNullOrEmpty(),
									 x => x.Code.Contains(input.Filter))

					// Lọc theo WarehouseId nếu client đã truyền
					.WhereIf(input.WarehouseId.HasValue,
									 x => x.WarehouseId == input.WarehouseId.Value);

			// Tính tổng số bản ghi sau khi đã filter
			var totalCount = await query.CountAsync();

			// Áp dụng sắp xếp + phân trang
			var storageLocations = await query
					.OrderBy(x => x.CreationTime)
					.Skip(input.SkipCount)
					.Take(input.MaxResultCount)
					.ToListAsync();

			// Map ra DTO
			var items = storageLocations.Select(x => new StorageLocationListDto
			{
				Id = x.Id,
				Code = x.Code,
				WarehouseName = x.Warehouse.Name,
				Capacity = x.Capacity,
				CurrentVolume = x.CurrentVolume,
				IsAvailable = x.IsAvailable
			}).ToList();

			return new PagedResultDto<StorageLocationListDto>(totalCount, items);
		}

		public async Task<storageLocationDto> GetByIdAsync(int id)
		{
			var query = await _storageLocationRepository
		.GetAll()
		.Include(x => x.Warehouse)
		.FirstOrDefaultAsync(x => x.Id == id);
			return new storageLocationDto
			{
				Id = query.Id,
				Code = query.Code,
				WarehouseId = query.WarehouseId,
				WarehouseName = query.Warehouse.Name,
				Capacity = query.Capacity,
				CurrentVolume = query.CurrentVolume,
				IsAvailable = query.IsAvailable
			};
		}
		[AbpMvcAuthorize(PermissionNames.Pages_StorageLocations_Edit)]

		public async Task<StorageLocationListDto> UpdateAsync(UpdateStorageLocationDto input)
		{
			var storageLocation = await _storageLocationRepository.GetAsync(input.Id);
			if (storageLocation == null)
			{
				throw new Exception("Storage location not found");
			}
			storageLocation.Code = input.Code;
			storageLocation.WarehouseId = input.WarehouseId;
			storageLocation.Capacity = input.Capacity;
			storageLocation.CurrentVolume = input.CurrentVolume;
			storageLocation.IsAvailable = input.IsAvailable;
			await _storageLocationRepository.UpdateAsync(storageLocation);
			return new StorageLocationListDto
			{
				Id = storageLocation.Id,
				Code = storageLocation.Code,
				//WarehouseId = storageLocation.WarehouseId,
				Capacity = storageLocation.Capacity,
				CurrentVolume = storageLocation.CurrentVolume,
				IsAvailable = storageLocation.IsAvailable
			};
		}
	}
}
