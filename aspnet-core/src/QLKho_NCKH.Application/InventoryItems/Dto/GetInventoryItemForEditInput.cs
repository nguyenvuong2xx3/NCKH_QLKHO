using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.InventoryItems.Dto
{
	public class GetInventoryItemForEditInput
	{
		public string ProductName { get; set; }
		public string ProductBarcode { get; set; }
		public string WarehouseName { get; set; }
		public int ProductId { get; set; }
		public string ProductImage { get; set; }
		public string Description { get; set; }
		public int StorageLocationId { get; set; }
		public string ProductCode { get; set; }
		public string StorageLocationCode { get; set; }
		public int Quantity { get; set; }
		public int ReservedQuantity { get; set; }
		public decimal UnitPrice { get; set; }
	}
}
