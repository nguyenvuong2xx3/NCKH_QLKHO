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
	public class StockTransactionDto : FullAuditedEntityDto<int>
	{
		public string TransactionCode { get; set; }
		public DateTime TransactionDate { get; set; }
		public string FromWarehouseName { get; set; }
		public string ToWarehouseName { get; set; }

		public string SupplierName { get; set; }
		public string ReferenceNumber { get; set; }
		public string Note { get; set; }

		public TransactionStatusEnum Status { get; set; }

		public string TransactionType { get; set; }

	}
}
