using Abp.Application.Services.Dto;
using QLKho_NCKH.StorageLocations.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.StorageLocations
{
	public interface IStorageLocationAppService
	{
		Task<PagedResultDto<StorageLocationListDto>> GetAllAsync(StorageLocationInput input);
		Task<storageLocationDto> GetByIdAsync(int id);
		Task<storageLocationDto> CreateAsync(CreateStorageLocationDto input);

		Task<StorageLocationListDto> UpdateAsync(UpdateStorageLocationDto input);
		Task DeleteAsync(int id);
	}
}
