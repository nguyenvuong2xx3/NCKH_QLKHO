using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QLKho_NCKH.StockTransactions;

namespace QLKho_NCKH.Customers
{
	[Table("AppCustomers")]
	public class Customer : FullAuditedEntity<int>
	{
		[Required]
		[StringLength(50)]
		public string Code { get; set; } // Mã khách hàng (ví dụ: KH-001)

		[Required]
		[StringLength(100)]
		public string Name { get; set; } // Tên khách hàng

		[StringLength(200)]
		public string Address { get; set; }

		[StringLength(20)]
		public string PhoneNumber { get; set; }

		[EmailAddress]
		[StringLength(100)]
		public string Email { get; set; }

		[StringLength(20)]
		public string TaxCode { get; set; } // Mã số thuế

		public bool IsActive { get; set; } = true;

		// Quan hệ 1-N với StockTransaction (1 khách hàng có nhiều lần xuất kho)
		public ICollection<StockTransaction> StockTransactions { get; set; } = new List<StockTransaction>();
	}
}
