using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using QLKho_NCKH.StorageLocations.Dto;
using QLKho_NCKH.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QLKho_NCKH.StorageLocations
{
	public class StorageLocationAppService : IStorageLocationAppService, IApplicationService
	{
		private readonly IRepository<StorageLocation, int> _storageLocationRepository;
		//private readonly IUnitOfWorkManager _unitOfWorkManager;
		public StorageLocationAppService(IRepository<StorageLocation, int> storageLocationRepository) //, IUnitOfWorkManager unitOfWorkManager
		{
			_storageLocationRepository = storageLocationRepository;
			//_unitOfWorkManager = unitOfWorkManager;
		}

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
			var query = await _storageLocationRepository.GetAllAsync();
			var storageLocations = query
				.WhereIf(!input.Filter.IsNullOrEmpty(), x => x.Code.Contains(input.Filter))
				.OrderBy(x => x.CreationTime)
				.Skip(input.SkipCount)
				.Take(input.MaxResultCount);
			var totalCount = await query.CountAsync();
			var storageLocationListDtos = new List<StorageLocationListDto>();
			foreach (var storageLocation in storageLocations)
			{
				storageLocationListDtos.Add(new StorageLocationListDto
				{
					Id = storageLocation.Id,
					Code = storageLocation.Code,
					WarehouseId = storageLocation.WarehouseId,
					Capacity = storageLocation.Capacity,
					CurrentVolume = storageLocation.CurrentVolume,
					IsAvailable = storageLocation.IsAvailable
				});
			}
			return new PagedResultDto<StorageLocationListDto>
			{
				TotalCount = totalCount,
				Items = storageLocationListDtos
			};
		}

		public async Task<StorageLocationListDto> GetByIdAsync(int id)
		{
			var query = await _storageLocationRepository.GetAsync(id);
			return new StorageLocationListDto
			{
				Id = query.Id,
				Code = query.Code,
				WarehouseId = query.WarehouseId,
				Capacity = query.Capacity,
				CurrentVolume = query.CurrentVolume,
				IsAvailable = query.IsAvailable
			};
		}

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
				WarehouseId = storageLocation.WarehouseId,
				Capacity = storageLocation.Capacity,
				CurrentVolume = storageLocation.CurrentVolume,
				IsAvailable = storageLocation.IsAvailable
			};
		}
	}
}
