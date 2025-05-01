using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace QLKho_NCKH.Products.Dtos
{
	public class CreateProductDto
	{
		[Required]
		[StringLength(50)]
		public string Code { get; set; }

		[Required]
		[StringLength(256)]
		public string Name { get; set; }

		[StringLength(500)]
		public string Description { get; set; }

		public int? CategoryId { get; set; }

		[StringLength(100)]
		public string Barcode { get; set; }

		[StringLength(20)]
		public string Unit { get; set; }

		public decimal Weight { get; set; }
		public decimal Volume { get; set; }
		public bool IsActive { get; set; } = true;

		public int? SupplierId { get; set; }
	}
}
