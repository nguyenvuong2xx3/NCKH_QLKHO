// YourProject.Domain/Warehouses/Warehouse.cs
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;

namespace QLKho_NCKH.Warehouses
{
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