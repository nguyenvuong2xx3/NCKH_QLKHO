// YourProject.Domain/Inventory/InventoryItem.cs
using System.ComponentModel.DataAnnotations.Schema;
using Abp.Domain.Entities.Auditing;
using QLKho_NCKH.Products;
using QLKho_NCKH.Warehouses;

namespace QLKho_NCKH.Inventory
{
	[Table("AppInventoryItems")]

	public class InventoryItem : FullAuditedEntity<int>
	{
		[ForeignKey(nameof(ProductId))]
		public Product Product { get; set; }
		public int ProductId { get; set; }

		[ForeignKey(nameof(StorageLocationId))]
		public StorageLocation StorageLocation { get; set; }
		public int StorageLocationId { get; set; }

		public int Quantity { get; set; }
		public int ReservedQuantity { get; set; }
		public decimal UnitPrice { get; set; }
	}
}