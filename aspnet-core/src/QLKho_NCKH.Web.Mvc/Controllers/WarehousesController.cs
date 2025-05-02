using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.Warehouses;

namespace QLKho_NCKH.Web.Controllers
{
	public class WarehousesController : QLKho_NCKHControllerBase
	{
		private readonly IWarehouseAppService _warehouseAppService;
		public WarehousesController(IWarehouseAppService warehouseAppService)
		{
			_warehouseAppService = warehouseAppService;
		}

		public IActionResult Index()
		{
			return View();
		}

		public IActionResult Edit( int warehouseId)
		{
			var viewModel = _warehouseAppService.GetByIdAsync(warehouseId);
			return PartialView("_EditModal", warehouseId);
		}
	}
}
