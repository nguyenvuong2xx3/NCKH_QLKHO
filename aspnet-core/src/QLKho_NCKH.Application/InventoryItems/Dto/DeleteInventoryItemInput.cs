using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.InventoryItems.Dto
{
	public class DeleteInventoryItemInput
	{
		public int ProductId { get; set; }
		public int StorageLocationId { get; set; }
	}
}
