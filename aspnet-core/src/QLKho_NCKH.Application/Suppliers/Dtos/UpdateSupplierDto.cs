using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Suppliers.Dtos
{
	public class UpdateSupplierDto : FullAuditedEntityDto<int>
	{
		public string Name { get; set; }
		public string Code { get; set; }
		public string Address { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }

		public string TaxCode { get; set; }
		//public string ContactPerson { get; set; }
		//public DateTime CreatedDate { get; set; }
		//public DateTime UpdatedDate { get; set; }
		public bool IsActive { get; set; }
	}
	
}
