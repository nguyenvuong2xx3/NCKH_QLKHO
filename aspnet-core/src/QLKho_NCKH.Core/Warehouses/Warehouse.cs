// YourProject.Domain/Warehouses/Warehouse.cs
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace QLKho_NCKH.Warehouses
{
	[Table("AppWarehouses")]
	public class Warehouse : FullAuditedEntity<int>
	{
		public string Code { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public decimal TotalArea { get; set; }
		public bool IsActive { get; set; }

		public ICollection<StorageLocation> StorageLocations { get; set; } = new List<StorageLocation>();
	}
}