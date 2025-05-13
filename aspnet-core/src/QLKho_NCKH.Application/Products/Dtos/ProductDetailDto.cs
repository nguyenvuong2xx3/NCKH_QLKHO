using QLKho_NCKH.EnumCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QLKho_NCKH.Products.Product;

namespace QLKho_NCKH.Products.Dtos
{
	public class ProductDetailDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public decimal Price { get; set; }
		public ProductState State { get; set; }
		public DateTime CreationTime { get; set; }
		public string Image { get; set; }

	}
}
