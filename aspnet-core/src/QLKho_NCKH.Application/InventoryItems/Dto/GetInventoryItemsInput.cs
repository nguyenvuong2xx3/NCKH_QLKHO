using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.InventoryItems.Dto
{
	public class GetInventoryItemsInput : PagedAndSortedResultRequestDto, IShouldNormalize
	{
		public string Filter { get; set; }

		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }

		public int CategoryId { get; set; }
		public int? ProductId { get; set; }
		public int? StorageLocationId { get; set; }
		public int? Quantity { get; set; }
		public int? ReservedQuantity { get; set; }
		public decimal? UnitPrice { get; set; }

		public int WarehouseId { get; set; }
		public void Normalize()
		{
			if (Sorting.IsNullOrWhiteSpace())
			{
				Sorting = "CreationTime DESC";
			}
		}
	}
}
