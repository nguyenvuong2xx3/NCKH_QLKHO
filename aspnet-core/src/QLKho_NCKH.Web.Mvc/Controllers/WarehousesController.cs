using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.Suppliers;
using QLKho_NCKH.Warehouses;
using QLKho_NCKH.Web.Models.Suppliers;
using QLKho_NCKH.Web.Models.Warehouses;
using System.Threading.Tasks;

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

		public async Task<IActionResult> Edit(int warehouseId)
		{
			var warehouse = await _warehouseAppService.GetByIdAsync(warehouseId);
			if (warehouse == null)
			{
				return NotFound();
			}

			var model = new WarehouseEditViewModel
			{
				Id = warehouse.Id,
				Code = warehouse.Code,
				Name = warehouse.Name,
				Location = warehouse.Location,
				TotalArea = warehouse.TotalArea,
				IsActive = warehouse.IsActive
			};

			return PartialView("_EditWarehouseModal", model);
		}
	}
}
