using Abp.Application.Services.Dto;
using QLKho_NCKH.Suppliers.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Suppliers
{
	public interface  ISupplierAppService
	{
		Task<PagedResultDto<SupplierDto>> GetAllAsync(SupplierInput input);
		Task<SupplierDto> GetByIdAsync(int id);
		Task<SupplierDto> CreateAsync(CreateSupplierDto input);
		Task<UpdateSupplierDto> UpdateAsync(UpdateSupplierDto input);
		Task DeleteAsync(int id);
		Task<List<SupplierDto>> SearchAsync(string searchTerm);
		Task<List<SupplierDto>> GetSuppliersByCategoryIdAsync(int categoryId);
	}
}
