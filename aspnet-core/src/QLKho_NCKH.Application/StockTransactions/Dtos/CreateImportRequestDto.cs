using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.StockTransactions.Dtos
{
	public class CreateImportRequestDto
	{
		public int WarehouseId { get; set; }
		public int SupplierId { get; set; }
		//public string Note { get; set; }
		//public int FromWarehouseId { get; set; }
		public List<ImportRequestDetailDto> ImportRequestDetails { get; set; }
	}
}
	