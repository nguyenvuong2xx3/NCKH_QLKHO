using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Controllers;
using System.Threading.Tasks;

namespace QLKho_NCKH.Web.Controllers
{
	public class InventoryItemsController : QLKho_NCKHControllerBase
	{
		public IActionResult Index()
		{
			return View();
		}
		public async Task<IActionResult> AddInventory()
		{
			return PartialView("_AddInventoryItemModal");
		}
	}
}
