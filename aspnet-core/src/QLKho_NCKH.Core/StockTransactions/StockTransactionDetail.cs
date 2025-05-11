using Abp.Domain.Entities.Auditing;
using QLKho_NCKH.Products;
using QLKho_NCKH.Warehouses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLKho_NCKH.StockTransactions;

namespace QLKho_NCKH.StockTransactions
{
	[Table("AppStockTransactionDetails")]
	public class StockTransactionDetail : FullAuditedEntity<int>
	{
		[Required]
		[ForeignKey(nameof(StockTransactionId))]
		public StockTransaction StockTransaction { get; set; }
		public int StockTransactionId { get; set; }

		[Required]
		[ForeignKey(nameof(ProductId))]
		public Product Product { get; set; }
		public int ProductId { get; set; }

		[Required]
		[ForeignKey(nameof(StorageLocationId))]
		public StorageLocation StorageLocation { get; set; }
		public int StorageLocationId { get; set; }

		[Required]
		[Range(1, int.MaxValue)]
		public int Quantity { get; set; }

		[Required]
		[Range(0, double.MaxValue)]
		public decimal UnitPrice { get; set; }

		[Column(TypeName = "decimal(18,2)")]
		public decimal TotalPrice => Quantity * UnitPrice;

		[StringLength(500)]
		public string Note { get; set; }

		// Dùng cho hàng có lô/serial
		[StringLength(50)]
		public string BatchNumber { get; set; }
		public DateTime? ExpiryDate { get; set; }

	}
}
