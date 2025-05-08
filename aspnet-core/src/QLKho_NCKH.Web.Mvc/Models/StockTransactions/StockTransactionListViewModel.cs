using Abp.Application.Services.Dto;
using QLKho_NCKH.EnumCustom;
using System;

namespace QLKho_NCKH.Web.Models.StockTransactions
{
	public class StockTransactionListViewModel : FullAuditedEntityDto<int>
	{
		public string TransactionCode { get; set; }
		public DateTime TransactionDate { get; set; }
		public int? FromWarehouseId { get; set; }
		public int? ToWarehouseId { get; set; }
		public int SupplierId { get; set; }
		public string ReferenceNumber { get; set; }
		public string Note { get; set; }
		public TransactionStatusEnum Status { get; set; }
		public string TransactionType { get; set; }
	}
}
