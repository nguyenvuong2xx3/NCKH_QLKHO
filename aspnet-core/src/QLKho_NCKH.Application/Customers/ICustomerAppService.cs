using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QLKho_NCKH.Customers.Dto;

namespace QLKho_NCKH.Customers
{
	public interface ICustomerAppService : IApplicationService
	{
		Task<CustomerListDto> Create(CreateCustomerDto input);
		Task<PagedResultDto<CustomerListDto>> GetAllCustomers(GetAllCustomersInput input);
		Task<CustomerListDto> GetCustomerById(int id);
		Task<CustomerListDto> Update(UpdateCustomerDto input);
		Task Delete(int id);
		//Task<List<CustomerListDto>> GetAllCustomers();
	}
}