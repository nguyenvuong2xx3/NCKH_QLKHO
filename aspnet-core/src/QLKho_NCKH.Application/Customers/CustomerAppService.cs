using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using QLKho_NCKH.Authorization;
using QLKho_NCKH.Customers.Dto;

namespace QLKho_NCKH.Customers
{
	//[AbpAuthorize(PermissionNames.Pages_Customers)]
	public class CustomerAppService : QLKho_NCKHAppServiceBase, ICustomerAppService
	{
		private readonly IRepository<Customer> _customerRepository;

		public CustomerAppService(IRepository<Customer> customerRepository)
		{
			_customerRepository = customerRepository;
		}

		public async Task<CustomerListDto> Create(CreateCustomerDto input)
		{
			var customer = new Customer
			{
				Name = input.Name,
				Code = input.Code,
				Address = input.Address,
				PhoneNumber = input.PhoneNumber,
				Email = input.Email,
				TaxCode = input.TaxCode,
				IsActive = input.IsActive
			};
			await _customerRepository.InsertAsync(customer);
			return new CustomerListDto
			{
				Id = customer.Id,
				Code = customer.Code,
				Name = customer.Name,
				Address = customer.Address,
				PhoneNumber = customer.PhoneNumber,
				Email = customer.Email,
				TaxCode = customer.TaxCode,
				IsActive = customer.IsActive
			};
		}

		public async Task<PagedResultDto<CustomerListDto>> GetAllCustomers(GetAllCustomersInput input)
		{
			var query = _customerRepository.GetAll()
					.WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
							c => c.Name.Contains(input.Filter) ||
									 c.Code.Contains(input.Filter) ||
									 c.PhoneNumber.Contains(input.Filter));

			var totalCount = await query.CountAsync();
			var customers = await query.PageBy(input).Select(c => new CustomerListDto
			{
				Id = c.Id,
				Code = c.Code,
				Name = c.Name,
				Address = c.Address,
				PhoneNumber = c.PhoneNumber,
				Email = c.Email,
				TaxCode = c.TaxCode,
				IsActive = c.IsActive
			}).ToListAsync();

			return new PagedResultDto<CustomerListDto>(totalCount, customers);
		}

		public async Task<CustomerListDto> GetCustomerById(int id)
		{
			var customer = await _customerRepository.GetAsync(id);
			return new CustomerListDto
			{
				Id = customer.Id,
				Code = customer.Code,
				Name = customer.Name,
				Address = customer.Address,
				PhoneNumber = customer.PhoneNumber,
				Email = customer.Email,
				TaxCode = customer.TaxCode,
				IsActive = customer.IsActive
			};
		}

		public async Task<CustomerListDto> Update(UpdateCustomerDto input)
		{
			var customer = await _customerRepository.GetAsync(input.Id);
			customer.Code = input.Code;
			customer.Name = input.Name;
			customer.Address = input.Address;
			customer.PhoneNumber = input.PhoneNumber;
			customer.Email = input.Email;
			customer.TaxCode = input.TaxCode;
			customer.IsActive = input.IsActive;
			// Update the customer entity
			await _customerRepository.UpdateAsync(customer);
			return new CustomerListDto
			{
				Id = customer.Id,
				Code = customer.Code,
				Name = customer.Name,
				Address = customer.Address,
				PhoneNumber = customer.PhoneNumber,
				Email = customer.Email,
				TaxCode = customer.TaxCode,
				IsActive = customer.IsActive
			};
		}

		public async Task Delete(int id)
		{
			await _customerRepository.DeleteAsync(id);
		}

		//public async Task<List<CustomerListDto>> GetAllCustomers()
		//{
		//	var customers = await _customerRepository.GetAllListAsync();
		//	return ObjectMapper.Map<List<CustomerListDto>>(customers);
		//}
	}
}