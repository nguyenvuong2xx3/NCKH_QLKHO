using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.EnumCustom
{
	public enum ProductType
	{
		FinishedGoods, // Thành phẩm
		RawMaterial,  // Nguyên vật liệu
		SemiFinished, // Bán thành phẩm
		Consumable    // Vật tư tiêu hao
	}

	public enum TransactionType
	{
		Import,     // Nhập kho
		Export,     // Xuất kho
		Transfer,   // Điều chuyển kho
		Adjustment  // Điều chỉnh tồn kho	
	}

	public enum TransactionStatus
	{
		Draft,      // Nháp
		Pending,    // Chờ duyệt
		Approved,   // Đã duyệt
		Completed,  // Hoàn thành
		Cancelled   // Đã hủy
	}

	public enum InventoryActionType
	{
		Import,
		Export,
		TransferIn,
		TransferOut,
		StockTake,
		Adjustment
	}
}
