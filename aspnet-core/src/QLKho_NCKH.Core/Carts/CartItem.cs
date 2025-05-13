using Abp.Domain.Entities.Auditing;
using QLKho_NCKH.Authorization.Users;
using QLKho_NCKH.Inventory;
using QLKho_NCKH.Products;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Carts
{
	[Table("AppCarts")]
	public class CartItem : FullAuditedEntity<int>
	{
			public int ProductId { get; set; }
			[ForeignKey("ProductId")]
			public Product Product { get; set; }
			public long UserId { get; set; }
			[ForeignKey("UserId")]
			public User User { get; set; }
			public int Quantity { get; set; }
	}
}
