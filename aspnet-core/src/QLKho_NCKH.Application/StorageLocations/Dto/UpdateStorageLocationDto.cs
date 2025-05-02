using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.StorageLocations.Dto
{
	public class UpdateStorageLocationDto : FullAuditedEntity<int>
	{
		public string Code { get; set; }
		public int WarehouseId { get; set; }
		public decimal Capacity { get; set; }
		public decimal CurrentVolume { get; set; }
		public bool IsAvailable { get; set; }
	}
}
