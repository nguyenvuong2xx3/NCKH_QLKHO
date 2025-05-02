using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using System;

namespace QLKho_NCKH.Products.Dtos
{
	public class ProductListDto : FullAuditedEntityDto<int>
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public int? CategoryId { get; set; }
		public string CategoryName { get; set; }

		public string Barcode { get; set; }
		public bool IsActive { get; set; }
		public string Image { get; set; }
		public string Unit { get; set; }
		public decimal Weight { get; set; }
		public decimal Volume { get; set; }
		public int? SupplierId { get; set; }
		public string SupplierName { get; set; }
	}
}
