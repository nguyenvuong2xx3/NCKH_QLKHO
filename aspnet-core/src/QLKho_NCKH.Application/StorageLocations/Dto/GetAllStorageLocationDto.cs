using Abp.Application.Services.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.StorageLocations.Dto
{
	public class GetAllStorageLocationDto : FullAuditedEntityDto<int>
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public int WarehouseId { get; set; }
		public decimal Capacity { get; set; }
		public decimal CurrentVolume { get; set; }
		public bool IsAvailable { get; set; }
	}
}
