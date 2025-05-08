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
		public int TransactionType { get; set; }
		public DateTime TransactionDate { get; set; }
		public int? FromWarehouseId { get; set; }	
		public int? ToWarehouseId { get; set; }
		public int? SupplierId { get; set; }
		public string ReferenceNumber { get; set; }
		public string Note { get; set; }
		public TransactionStatusEnum Status { get; set; }
		public StockTransactionListDto()
		{
		}
	}
}
