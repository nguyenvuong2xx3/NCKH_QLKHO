using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;

namespace QLKho_NCKH.Customers.Dto
{
	public class CustomerListDto : FullAuditedEntity<int>
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string TaxCode { get; set; }
		public bool IsActive { get; set; }
	}

	public class CreateCustomerDto : FullAuditedEntity<int>
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string TaxCode { get; set; }
		public bool IsActive { get; set; } = true;
	}

	public class UpdateCustomerDto
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string TaxCode { get; set; }
		public bool IsActive { get; set; }
	}

	public class GetAllCustomersInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }
	}
}
