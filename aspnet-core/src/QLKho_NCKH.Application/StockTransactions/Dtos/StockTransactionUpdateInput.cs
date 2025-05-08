using QLKho_NCKH.EnumCustom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.StockTransactions.Dtos
{
	public class StockTransactionUpdateInput
	{
		public int Id { get; set; }

		public int? ProductId { get; set; }

		public int? StorageLocationId { get; set; }

		public TransactionStatusEnum Status { get; set; }
	}
}
