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
		public async Task<IActionResult> AddWarehoses()
		{
			return PartialView("_AddWarehosesModal");
		}

		public IActionResult Edit(int warehouseId)
		{
			var warehouse =  _warehouseAppService.GetByIdAsync(warehouseId);
			if (warehouse == null)
			{
				return NotFound();
			}

			var viewmodel = new WarehouseEditViewModel
			{
				Id = warehouse.Result.Id,
				Code = warehouse.Result.Code,
				Name = warehouse.Result.Name,
				Location = warehouse.Result.Location,
				TotalArea = warehouse.Result.TotalArea,
				IsActive = warehouse.Result.IsActive
			};

			return PartialView("_EditWarehouseModal", viewmodel);
		}
	}
}
