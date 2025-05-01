// YourProject.Domain/Transactions/StockTransaction.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Domain.Entities.Auditing;
using QLKho_NCKH.Suppliers;
using QLKho_NCKH.Warehouses;
using System.ComponentModel.DataAnnotations.Schema;
using QLKho_NCKH.StockTransactions;
using QLKho_NCKH.EnumCustom;

namespace YourProject.Domain.Transactions
{
	public class StockTransaction : FullAuditedEntity<int>
	{
		[Required]
		[StringLength(20)]
		public string TransactionCode { get; set; }

		public TransactionType TransactionType { get; set; }
		public DateTime TransactionDate { get; set; } = DateTime.Now;

		[ForeignKey(nameof(FromWarehouseId))]
		public Warehouse FromWarehouse { get; set; }
		public int? FromWarehouseId { get; set; }

		[ForeignKey(nameof(ToWarehouseId))]
		public Warehouse ToWarehouse { get; set; }
		public int? ToWarehouseId { get; set; }

		[ForeignKey(nameof(SupplierId))]
		public Supplier Supplier { get; set; }
		public int? SupplierId { get; set; }

		public string ReferenceNumber { get; set; }
		public string Note { get; set; }
		public TransactionStatus Status { get; set; } = TransactionStatus.Draft;

		public ICollection<StockTransactionDetail> Details { get; set; } = new List<StockTransactionDetail>();
	}
}