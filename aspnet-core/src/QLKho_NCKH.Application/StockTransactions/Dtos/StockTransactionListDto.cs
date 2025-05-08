using Abp.Application.Services.Dto;
using QLKho_NCKH.EnumCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace QLKho_NCKH.StockTransactions.Dtos
{
	public class StockTransactionListDto : AuditedEntityDto<int>
	{
		public string TransactionCode { get; set; }
		public TransactionType TransactionType { get; set; }
		public DateTime TransactionDate { get; set; }
		public int? FromWarehouseId { get; set; }	
		public int? ToWarehouseId { get; set; }
		public int? CustomerId { get; set; }
		public string? SupplierName { get; set; }
		public string? FromWarehouseName { get; set; }
		public string? ToWarehouseName { get; set; }
		public string ReferenceNumber { get; set; }
		public string Note { get; set; }
		public DateTime CreationTime { get; set; }
		public TransactionStatusEnum Status { get; set; }

		public string? CustomerName { get; set; }

		public int? SupplierId { get; set; }

		public StockTransactionListDto()
		{
		}
	}
}
