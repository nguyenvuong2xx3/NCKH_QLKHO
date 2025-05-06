// YourProject.Domain/Suppliers/Supplier.cs
using Abp.Domain.Entities.Auditing;
using QLKho_NCKH.Products;
using QLKho_NCKH.StockTransactions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using YourProject.Domain.Transactions;

namespace QLKho_NCKH.Suppliers
{
	[Table("AppSuppliers")]
	public class Supplier : FullAuditedEntity<int>
	{
		[Required]
		[StringLength(50)]
		public string Code { get; set; }

		[Required]
		[StringLength(100)]
		public string Name { get; set; }

		[StringLength(200)]
		public string Address { get; set; }

		[StringLength(20)]
		public string PhoneNumber { get; set; }

		[EmailAddress]
		[StringLength(100)]
		public string Email { get; set; }

		[StringLength(20)]
		public string TaxCode { get; set; }

		public bool IsActive { get; set; } = true;

		public ICollection<Product> Products { get; set; } = new List<Product>();
		public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
	}
}