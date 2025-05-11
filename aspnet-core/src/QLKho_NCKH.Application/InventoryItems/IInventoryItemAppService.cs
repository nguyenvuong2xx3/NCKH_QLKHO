using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Dependency;
using System;
using QLKho_NCKH.InventoryItems.Dto;
using System.Threading.Tasks;
using System.IO;

namespace QLKho_NCKH.InventoryItems
{

	public interface IInventoryItemAppService : IApplicationService
	{
		Task CreateInventoryItem(InventoryItemCreatingInput input);

		Task<PagedResultDto<InventoryItemListDto>> GetInventoryItems(GetInventoryItemsInput input);
		Task<InventoryItemEditDto> GetInventoryItem(int product);
		//Task<InventoryItemEditDto> GetInventoryItem(int id);

		//Task<InventoryItemEditDto> EditInventoryItem(InventoryItemEditDto input);

		//Task<PagedResultDto<InventoryItemListDto>> GetInventoryItems(GetInventoryItemsInput input);

		//Task DeleteInventoryItem(int Id);



	}
}
