using Abp.Application.Services.Dto;
using QLKho_NCKH.EnumCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.StockTransactions.Dtos
{
	public class CreateStockTransactionImportDto : FullAuditedEntityDto<int>
	{
		public string TransactionCode { get; set; }
		public DateTime TransactionDate { get; set; } = DateTime.Now;
		public TransactionType TransactionType { get; set; } = TransactionType.Import;
		public int? FromWarehouseId { get; set; }
		public int? ToWarehouseId { get; set; }
		public int? SupplierId { get; set; }
		public string ReferenceNumber { get; set; }
		public string Note { get; set; }
	}
}
