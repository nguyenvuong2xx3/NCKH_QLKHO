using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.StockTransactions.Dtos
{
	public class ExportInputDto
	{
		public int WarehouseId { get; set; }
		//public int? SupplierId { get; set; }
		public string ReferenceNumber { get; set; }
		public string Note { get; set; }
		public int CustomerId { get; set; }
		public List<ImportRequestDetailDto> ExportRequestDetails { get; set; }
		public string TransactionCode { get; set; }
		public int? FromWarehouseId { get; set; }
		public int? ToWarehouseId { get; set; }
	}
}
