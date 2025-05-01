using Microsoft.AspNetCore.Mvc;

namespace QLKho_NCKH.Web.Controllers
{
	public class ProductsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
