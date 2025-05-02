using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Warehouses.Dto
{
	public class WarehouseDto : FullAuditedEntityDto<int>
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public decimal TotalArea { get; set; }
		public bool IsActive { get; set; }
	}
	
}
