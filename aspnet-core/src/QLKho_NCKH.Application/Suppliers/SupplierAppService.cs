using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using QLKho_NCKH.Suppliers.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Suppliers
{
	public class SupplierAppService : ISupplierAppService, IApplicationService
	{
		private readonly IRepository<Supplier, int> _supplierRepository;

		public SupplierAppService(IRepository<Supplier, int> supplierRepository)
		{
			_supplierRepository = supplierRepository;
		}

		public async Task<SupplierDto> CreateAsync(CreateSupplierDto input)
		{
			Supplier supplier = new Supplier
			{
				Name = input.Name,
				Code = input.Code,
				Address = input.Address,
				PhoneNumber = input.PhoneNumber,
				Email = input.Email,
				TaxCode = input.TaxCode,
				IsActive = input.IsActive,
				//CreationTime = DateTime.Now,
				//LastModificationTime = DateTime.Now
			};
			await _supplierRepository.InsertAsync(supplier);
			return new SupplierDto
			{
				Id = supplier.Id,
				Code = supplier.Code,
				Name = supplier.Name,
				Address = supplier.Address,
				PhoneNumber = supplier.PhoneNumber,
				Email = supplier.Email,
				TaxCode = supplier.TaxCode,
				IsActive = supplier.IsActive
			};
		}

		public async Task DeleteAsync(int id)
		{
			await _supplierRepository.DeleteAsync(id);
		}

		public async Task<PagedResultDto<SupplierDto>> GetAllAsync(SupplierInput input)
		{
			var suppliers = _supplierRepository.GetAll();
			suppliers = suppliers.WhereIf(!input.Filter.IsNullOrEmpty(), s => s.Name.Contains(input.Filter));

			suppliers = suppliers.OrderByDescending(x => x.CreationTime)
													//.ThenBy(x => EF.Property<object>(x, input.Sorting))
													.ThenBy(x => x.Id);
			var items = await suppliers.PageBy(input).ToListAsync();
			var totalCount = await suppliers.CountAsync();
			var suppliersDtos = new List<SupplierDto>();
			foreach (var supplier in items)
			{
				suppliersDtos.Add(new SupplierDto
				{
					Id = supplier.Id,
					Code = supplier.Code,
					Name = supplier.Name,
					Address = supplier.Address,
					PhoneNumber = supplier.PhoneNumber,
					Email = supplier.Email,
					TaxCode = supplier.TaxCode,
					IsActive = supplier.IsActive,

					//CreationTime = supplier.CreationTime
				});
			}
			return new PagedResultDto<SupplierDto>()
			{
				TotalCount = totalCount,
				Items = suppliersDtos
			};
		}

		/// <summary>
		/// Retrieves a supplier by its ID.
		/// </summary>
		/// <param name="id">The ID of the supplier to retrieve.</param>
		/// <returns>A <see cref="SupplierDto"/> containing the supplier details.</returns>
		public async Task<SupplierDto> GetByIdAsync(int id)
		{
			var supplier = await _supplierRepository.GetAsync(id);

			return new SupplierDto
			{
				Code = supplier.Code,
				Name = supplier.Name,
				Address = supplier.Address,
				PhoneNumber = supplier.PhoneNumber,
				Email = supplier.Email,
				TaxCode = supplier.TaxCode,
				IsActive = supplier.IsActive
			};
		}

		public Task<List<SupplierDto>> GetSuppliersByCategoryIdAsync(int categoryId)
		{
			throw new NotImplementedException();
		}

		public Task<List<SupplierDto>> SearchAsync(string searchTerm)
		{
			throw new NotImplementedException();
		}

		public async Task<UpdateSupplierDto> UpdateAsync(UpdateSupplierDto input)
		{
			var supplier = await _supplierRepository.GetAsync(input.Id);
			if (supplier == null)
			{
				throw new Exception("Supplier not found.");
			}

			supplier.Name = input.Name;
			supplier.Code = input.Code;
			supplier.Address = input.Address;
			supplier.PhoneNumber = input.PhoneNumber;
			supplier.Email = input.Email;
			supplier.TaxCode = input.TaxCode;
			supplier.IsActive = input.IsActive;

			await _supplierRepository.UpdateAsync(supplier);

			return new UpdateSupplierDto
			{
				Id = supplier.Id,
				Code = supplier.Code,
				Name = supplier.Name,
				Address = supplier.Address,
				PhoneNumber = supplier.PhoneNumber,
				Email = supplier.Email,
				TaxCode = supplier.TaxCode,
				IsActive = supplier.IsActive
			};
		}

		public async Task<List<SupplierDto>> GetAllSupplier()
		{
			var suppliers = await _supplierRepository.GetAllListAsync();
			var supplierDtos = suppliers.Select(s => new SupplierDto
			{
				Id = s.Id,
				Code = s.Code,
				Name = s.Name,
				Address = s.Address,
				PhoneNumber = s.PhoneNumber,
				Email = s.Email,
				TaxCode = s.TaxCode,
				IsActive = s.IsActive
			}).ToList();
			return supplierDtos;
		}
	}
}
