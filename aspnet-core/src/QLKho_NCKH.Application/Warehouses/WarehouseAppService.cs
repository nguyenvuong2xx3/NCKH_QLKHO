using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;

//using Abp.Collections.Extensions;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using QLKho_NCKH.Authorization;
using QLKho_NCKH.Warehouses.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Warehouses
{
	[AbpMvcAuthorize(PermissionNames.Pages_Warehouses)]

	public class WarehouseAppService : IWarehouseAppService, IApplicationService
	{
		private readonly IRepository<Warehouse, int> _warehouseRepository;

		public WarehouseAppService(IRepository<Warehouse, int> warehouseRepository)
		{
			_warehouseRepository = warehouseRepository;
		}

		public async Task<WarehouseDto> CreateAsync(CreateWarehouseDto input)
		{
			var warehouse = new Warehouse
			{
				Code = input.Code,
				Name = input.Name,
				Location = input.Location,
				TotalArea = input.TotalArea,
				IsActive = input.IsActive
			};

			await _warehouseRepository.InsertAsync(warehouse);
			return new WarehouseDto
			{
				Id = warehouse.Id,
				Code = warehouse.Code,
				Name = warehouse.Name,
				Location = warehouse.Location,
				TotalArea = warehouse.TotalArea,
				IsActive = warehouse.IsActive
			};
		}

		public async Task DeleteAsync(int id)
		{
			await _warehouseRepository.DeleteAsync(id);
		}

		public async Task<PagedResultDto<WarehouseDto>> GetAllAsync(WarehouseInput input)
		{
			var query = _warehouseRepository.GetAll();
			query = query.WhereIf(!input.Filter.IsNullOrEmpty(), w => w.Name.Contains(input.Filter) || w.Code.Contains(input.Filter));
			query = query.OrderByDescending(w => w.CreationTime).ThenBy(x => x.Id);

			var items = await query.PageBy(input).ToListAsync();
			var totalCount = await query.CountAsync();
			var result = new List<WarehouseDto>();
			foreach (var item in items)
			{
				result.Add(new WarehouseDto
				{
					Id = item.Id,
					Code = item.Code,
					Name = item.Name,
					Location = item.Location,
					TotalArea = item.TotalArea,
					IsActive = item.IsActive
				});
			}
			return new PagedResultDto<WarehouseDto>
			{
				TotalCount = totalCount,
				Items = result
			};

		}

		public async Task<WarehouseDto> GetByIdAsync(int id)
		{
			var warehouse = await _warehouseRepository.GetAsync(id);
			return new WarehouseDto
			{
				Id = warehouse.Id,
				Code = warehouse.Code,
				Name = warehouse.Name,
				Location = warehouse.Location,
				TotalArea = warehouse.TotalArea,
				IsActive = warehouse.IsActive
			};
		}

		public async Task<WarehouseDto> UpdateAsync(UpdateWarehouseDto input)
		{
			var warehouse = await _warehouseRepository.GetAsync(input.Id);
			if (warehouse == null)
			{
				throw new Exception("Warehouse not found");
			}
			warehouse.Code = input.Code;
			warehouse.Name = input.Name;
			warehouse.Location = input.Location;
			warehouse.TotalArea = input.TotalArea;
			warehouse.IsActive = input.IsActive;
			await _warehouseRepository.UpdateAsync(warehouse);

			return new WarehouseDto
			{
				Id = warehouse.Id,
				Code = warehouse.Code,
				Name = warehouse.Name,
				Location = warehouse.Location,
				TotalArea = warehouse.TotalArea,
				IsActive = warehouse.IsActive
			};

		}
	}
}
