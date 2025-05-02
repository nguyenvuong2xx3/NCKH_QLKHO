using Abp.Application.Services.Dto;
using QLKho_NCKH.Warehouses.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Warehouses
{
	public interface IWarehouseAppService
	{
		Task<WarehouseDto> CreateAsync(CreateWarehouseDto input);
		Task DeleteAsync(int id);
		Task<PagedResultDto<WarehouseDto>> GetAllAsync(WarehouseInput input);
		Task<WarehouseDto> GetByIdAsync(int id);
		Task<WarehouseDto> UpdateAsync(UpdateWarehouseDto input);
	}
}
