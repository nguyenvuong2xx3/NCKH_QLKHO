using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Products.Dtos
{
	public class ProductDto
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public int? CategoryId { get; set; }
		public string CategoryName { get; set; } // Optional: để hiển thị tên Category

		public string Barcode { get; set; }
		public string Unit { get; set; }
		public decimal Weight { get; set; }
		public decimal Volume { get; set; }
		public bool IsActive { get; set; }

		public int? SupplierId { get; set; }
		public string SupplierName { get; set; } // Optional: để hiển thị tên Supplier
	}
}
