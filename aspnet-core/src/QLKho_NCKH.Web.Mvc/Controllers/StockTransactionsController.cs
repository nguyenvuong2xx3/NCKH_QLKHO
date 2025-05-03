using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Controllers;

namespace QLKho_NCKH.Web.Controllers
{
	public class StockTransactionsController : QLKho_NCKHControllerBase
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult CreateImport()
		{
			return PartialView("_CreateImportModal");
		}
	}
}
