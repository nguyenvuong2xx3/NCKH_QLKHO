using Abp.Application.Services.Dto;
using QLKho_NCKH.InventoryItems.Dto;
using QLKho_NCKH.StockTransactions.Dtos;
using System.Collections.Generic;

namespace QLKho_NCKH.Web.Models.Home
{
	public class DashboardViewModel
	{
		public int TotalItems { get; set; }
		public int TotalProducts { get; set; }
		public int PendingTransactions { get; set; }
		public int CompletedTransactions { get; set; }
		public List<StockTransactionListDto> RecentTransactions { get; set; }
		public PagedResultDto<InventoryItemListDto> InventoryStats { get; set; }
	}
}
