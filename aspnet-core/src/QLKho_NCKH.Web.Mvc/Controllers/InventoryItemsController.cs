using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.InventoryItems.Dto;
using QLKho_NCKH.InventoryItems;
using QLKho_NCKH.Web.Models.inventoryItems;
using System.Threading.Tasks;

public class InventoryItemsController : QLKho_NCKHControllerBase
{
	private readonly IInventoryItemAppService _inventoryItemAppService;

	public InventoryItemsController(IInventoryItemAppService inventoryItemAppService)
	{
		_inventoryItemAppService = inventoryItemAppService;
	}

	public IActionResult Index()
	{
		return View();
	}

	public async Task<IActionResult> AddInventory()
	{
		return PartialView("_AddInventoryItemModal");
	}

	// GET: Modal for editing inventory item
	public async Task<PartialViewResult> EditInventoryItemModal(int productId, int storageLocationId)
	{
		// Get the inventory item details
		var inventoryItem = await _inventoryItemAppService.GetInventoryItemForEdit(
				new GetInventoryItemForEditInput
				{
					ProductId = productId,
					StorageLocationId = storageLocationId
				});

		var viewModel = new EditInventoryItemViewModel
		{
			InventoryItem = inventoryItem
		};

		return PartialView("_EditModal", viewModel);
	}
}