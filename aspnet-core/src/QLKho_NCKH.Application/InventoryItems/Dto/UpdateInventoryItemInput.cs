using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.InventoryItems.Dto
{
	public class UpdateInventoryItemInput
	{
		public int ProductId { get; set; }
		public int StorageLocationId { get; set; }
		public int Quantity { get; set; }
		//public int ReservedQuantity { get; set; }
		public decimal? UnitPrice { get; set; }
		// Add any additional fields you want to update
	}
}
