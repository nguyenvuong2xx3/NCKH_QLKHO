using Abp.Application.Services.Dto;
using Abp.Application.Services;
using QLKho_NCKH.StockTransactionDetails.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.StockTransactionDetails
{
	public interface IStockTransactionDetailAppService : IApplicationService
	{
		Task<StockTransactionDetailEditDto> CreateStockTransactionDetail(StockTransactionDetailCreatingInput input);

		//Task<StockTransactionDetailEditDto> GetStockTransactionDetail(int id);

		//Task<StockTransactionDetailEditDto> EditStockTransactionDetail(StockTransactionDetailEditDto input);

		//Task<PagedResultDto<StockTransactionDetailListDto>> GetStockTransactionDetails(GetStockTransactionDetailsInput input);

		//Task DeleteStockTransactionDetail(int Id);
	}
}
