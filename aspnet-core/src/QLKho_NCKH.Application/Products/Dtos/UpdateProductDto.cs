using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Products.Dtos
{
    public class UpdateProductDto 
    {
		public int Id { get; set; }
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
		public decimal Volume { get; set; } = 0;
		public bool IsActive { get; set; } = true;

		public int? SupplierId { get; set; }
		public string Image { get; set; }
		public IFormFile ImageFile { get; set; }
	}
}
