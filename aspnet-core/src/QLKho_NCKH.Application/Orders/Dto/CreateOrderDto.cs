using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Entities.Auditing;
using QLKho_NCKH.OrderDetails.Dto;

namespace QLKho_NCKH.Orders.Dto
{
	public class CreateOrderDto : FullAuditedEntity<int>
	{
		public long UserId { get; set; }
		public string NameUser { get; set; }
		public decimal TotalAmount { get; set; }
		public decimal DiscountAmount { get; set; }
		public int	PaymentMethod { get; set; }
		public int OrderStatus { get; set; }
	}
}
