using QLKho_NCKH.Customers.Dto;
using System.Collections.Generic;

namespace QLKho_NCKH.Web.Models.Customers
{
	public class CustomerViewModel
	{
		public IReadOnlyList<CustomerListDto> Customers { get; }

		public CustomerViewModel(IReadOnlyList<CustomerListDto> customers)
		{
			this.Customers = customers;
		}
	}
}