using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Controllers;

namespace QLKho_NCKH.Web.Controllers
{
	public class ProductsController : QLKho_NCKHControllerBase
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
