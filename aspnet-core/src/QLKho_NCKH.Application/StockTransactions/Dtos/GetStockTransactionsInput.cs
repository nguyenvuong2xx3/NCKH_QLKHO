using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using QLKho_NCKH.EnumCustom;
using QLKho_NCKH.InventoryItems.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.StockTransactions.Dtos
{
	public class GetStockTransactionsInput : PagedAndSortedResultRequestDto, IShouldNormalize
	{
		public string Filter { get; set; }
		public TransactionStatusEnum? Status { get; set; } // Thêm trường này

		//public DateTime? StartDate { get; set; }

		//public DateTime? EndDate { get; set; }

		//public int? TransactionType { get; set; }
		//public int? FromWarehouseId { get; set; }
		//public int? ToWarehouseId { get; set; }
		//public int? SupplierId { get; set; }
		//public int? Status { get; set; }



		public void Normalize()
		{
			if (Sorting.IsNullOrWhiteSpace())
			{
				Sorting = "CreationTime DESC";
			}
		}
	}
}
