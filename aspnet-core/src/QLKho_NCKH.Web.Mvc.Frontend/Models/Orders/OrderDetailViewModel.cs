using QLKho_NCKH.EnumCustom;
using QLKho_NCKH.StockTransactionDetails.Dto;
using QLKho_NCKH.StockTransactions.Dtos;
using QLKho_NCKH.Users.Dto;
using System;
using System.Collections.Generic;

namespace QLKho_NCKH.Web.Models
{
	public class OrderDetailViewModel
	{
		public StockTransactionListDto Order { get; set; }
		public List<StockTransactionDetailEditDto> OrderDetails { get; set; }
		public UserDto Customer { get; set; }
	}

	// View Models to add to your project
	public class StockTransactionHistoryViewModel
	{
		//public List<StockTransactionDetailViewModel> Transactions { get; set; }
		public List<StockTransactionDetailViewModel> Transactions { get; set; } = new List<StockTransactionDetailViewModel>();
		public string Filter { get; set; }
		public DateTime? StartTime { get; set; }
		public DateTime? EndTime { get; set; }
		public TransactionStatusEnum? Status { get; set; }

		// Thêm các thuộc tính phân trang
		public int TotalCount { get; set; }
		public int CurrentPage { get; set; }
		public int PageSize { get; set; }
		public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
	}

	public class StockTransactionDetailViewModel
	{
		public int TransactionId { get; set; }
		public string TransactionCode { get; set; }
		public DateTime TransactionDate { get; set; }
		public TransactionType TransactionType { get; set; }
		public TransactionStatusEnum Status { get; set; }
		public string FromWarehouse { get; set; }
		public string ToWarehouse { get; set; }
		public List<TransactionProductViewModel> Products { get; set; }
	}

	public class TransactionProductViewModel
	{
		public int ProductId { get; set; }
		public string ProductCode { get; set; }
		public string ProductName { get; set; }
		public int Quantity { get; set; }
		public decimal UnitPrice { get; set; }
		public decimal TotalPrice { get; set; }
		public string StorageLocation { get; set; }
		//public string BatchNumber { get; set; }
		//public DateTime? ExpiryDate { get; set; }
	}
}
