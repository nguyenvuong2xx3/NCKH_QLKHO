// YourProject.Domain/Products/Product.cs
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using QLKho_NCKH.Categories;
using QLKho_NCKH.Suppliers;

namespace QLKho_NCKH.Products
{
	public class Product : FullAuditedEntity<int>
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		[ForeignKey(nameof(CategoryId))]
		public Category Category { get; set; }
		public int? CategoryId { get; set; }
		public string Barcode { get; set; }
		public string Unit { get; set; }
		public decimal Weight { get; set; }
		public decimal Volume { get; set; }
		public bool IsActive { get; set; }
		[ForeignKey(nameof(SupplierId))]
		public Supplier Supplier { get; set; }
		public int? SupplierId { get; set; }
		public string Image { get; set; }
	}
}