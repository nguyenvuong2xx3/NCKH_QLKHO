using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.Customers;
using QLKho_NCKH.Web.Models.Categories;
using QLKho_NCKH.Web.Models.Customers;

namespace QLKho_NCKH.Web.Controllers
{
	public class CustomersController : QLKho_NCKHControllerBase
	{
		private readonly ICustomerAppService _customerAppService;
		public CustomersController(ICustomerAppService customerAppService)
		{
			_customerAppService = customerAppService;
		}
		public async Task<IActionResult> Index()
		{
			//var customers = await _customerAppService.GetAllAsync();
			return View();
		}

		public async Task<ActionResult> EditModal(int customerId)
		{
			var customer = await _customerAppService.GetCustomerById(customerId);

			var model = new EditCustomerViewModel
			{
				Customer = customer
			};

			return PartialView("_EditModal", model);
		}

		public async Task<ActionResult> AddCustomer()
		{
			return PartialView("_AddCustomersModal");
		}
	}
}
