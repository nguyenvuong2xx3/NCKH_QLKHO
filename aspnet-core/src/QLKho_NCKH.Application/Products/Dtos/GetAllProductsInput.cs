using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Products.Dtos
{
	public class GetAllProductsInput : PagedAndSortedResultRequestDto
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Barcode { get; set; }
		public int? CategoryId { get; set; }
		public int? SupplierId { get; set; }
		public bool? IsActive { get; set; }
	}
}
