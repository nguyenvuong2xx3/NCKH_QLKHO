using Abp.Domain.Entities.Auditing;
using QLKho_NCKH.Authorization.Users;
using QLKho_NCKH.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLKho_NCKH.Inventory;

namespace QLKho_NCKH.Orders
{
	[Table("AppOrders")]
	public class Order : FullAuditedEntity<int>
	{
		[Required]
		public long UserId { get; set; }
		[Required]
		public string NameUser { get; set; }
		public decimal totalAmount { get; set; } // tổng số tiền
		public decimal DiscountAmount { get; set; }// tổng tiền giảm giá 
		[Required]
		public int PaymentMethod { get; set; }
		[Required]
		[StringLength(50)]
		public int OrderStatus { get; set; }// trang thái đơn hàng mặc định

		//quan hệ với bảng user
		[ForeignKey("UserId")]
		public User User { get; set; }
	}

	[Table("AppOrderDetails")]
	public class OrderDetail : FullAuditedEntity<int>
	{
		[Required]
		public int OrderId { get; set; }
		[ForeignKey("OrderId")]
		public Order Order { get; set; }
		[Required]
		public int Quantity { get; set; }
		[Required]
		public decimal UnitPrice { get; set; } // giá của sản phẩm tại thời điểm đặt hàng
		public decimal DiscountPrice { get; set; } = 0; // giảm giá cho sản phẩm nếu có 
		public decimal TotalPrice => (UnitPrice * Quantity) - DiscountPrice; // tổng giá tiền cho sản phẩm 

		//[Required]
		//public int InventoryItemId { get; set; }
		//[ForeignKey("InventoryItemId")]
		//public InventoryItem InventoryItem { get; set; }
	}
}
