using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Authorization;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.Suppliers;
using QLKho_NCKH.Web.Models.Suppliers;

namespace QLKho_NCKH.Web.Controllers
{
	[AbpMvcAuthorize(PermissionNames.Pages_Suppliers)]

	public class SuppliersController : QLKho_NCKHControllerBase
	{
		private readonly ISupplierAppService _supplierRepository;

		public SuppliersController(ISupplierAppService supplierRepository)
		{
			_supplierRepository = supplierRepository;
		}

		public IActionResult Index()
		{
			return View();
		}
		public IActionResult Edit(int supplierId)
		{
			var supplier = _supplierRepository.GetByIdAsync(supplierId);

			var viewmodel = new EditSuppliersModalViewModel
			{
				Id = supplierId,
				Code = supplier.Result.Code,
				Name = supplier.Result.Name,
				Address = supplier.Result.Address,
				PhoneNumber = supplier.Result.PhoneNumber,
				Email = supplier.Result.Email,
				TaxCode = supplier.Result.TaxCode,
				IsActive = supplier.Result.IsActive
			};
			return PartialView("_EditSupplierModal", viewmodel);
		}
	}
}
