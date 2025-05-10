using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Authorization;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.StorageLocations;
using QLKho_NCKH.Web.Models.storageLocations;

namespace QLKho_NCKH.Web.Controllers
{
	[AbpMvcAuthorize(PermissionNames.Pages_StorageLocations)]

	public class StorageLocationsController : QLKho_NCKHControllerBase
	{
		private readonly IStorageLocationAppService _storageLocationRepository;
		public StorageLocationsController(IStorageLocationAppService storageLocationRepository)
		{
			_storageLocationRepository = storageLocationRepository;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Edit(int storageLocationId)
		{
			var storageLocation =  _storageLocationRepository.GetByIdAsync(storageLocationId);
			var viewmodel = new EditStorageLocationViewModel
			{
				Id = storageLocationId,
				Code = storageLocation.Result.Code,
				WarehouseId = storageLocation.Result.WarehouseId,
				Capacity = storageLocation.Result.Capacity,
				CurrentVolume = storageLocation.Result.CurrentVolume,
				IsAvailable = storageLocation.Result.IsAvailable,
				WarehouseName = storageLocation.Result.WarehouseName,
				//WarehouseCode = storageLocation.Warehouse.Code,
			};
			return PartialView("_EditStorageLocationModal", viewmodel);
		}
	}
}
