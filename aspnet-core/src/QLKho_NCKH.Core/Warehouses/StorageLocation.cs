using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKho_NCKH.Warehouses
{
	[Table("AppStorageLocations")]
	public class StorageLocation : FullAuditedEntity<int>
	{
		public string Code { get; set; }

		[ForeignKey(nameof(WarehouseId))]
		public Warehouse Warehouse { get; set; }
		public int WarehouseId { get; set; }

		public decimal Capacity { get; set; }
		public decimal CurrentVolume { get; set; }
		public bool IsAvailable { get; set; }
	}
}