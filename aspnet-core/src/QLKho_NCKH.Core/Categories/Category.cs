using Abp.Domain.Entities.Auditing;
using QLKho_NCKH.Products;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKho_NCKH.Categories
{
	public class Category : FullAuditedEntity<int>
	{
		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[StringLength(500)]
		public string Description { get; set; }

		public int? ParentId { get; set; }

		[ForeignKey(nameof(ParentId))]
		public Category Parent { get; set; }

		public ICollection<Product> Products { get; set; } = new List<Product>();
	}
}