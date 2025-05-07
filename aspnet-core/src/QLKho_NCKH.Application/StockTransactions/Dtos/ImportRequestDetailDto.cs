using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.StockTransactions.Dtos
{
	public class ImportRequestDetailDto
	{
		public int ProductId { get; set; }
		public int Quantity { get; set; }
		public int StorageLocationId { get; set; }

		public int UnitPrice { get; set; }
		//public string BatchNumber { get; set; }
	}
}
