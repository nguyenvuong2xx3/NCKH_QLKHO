using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QLKho_NCKH.StockTransactions.Dtos;
using QLKho_NCKH.Users.Dto;
using System.Threading.Tasks;

namespace QLKho_NCKH.StockTransactions
{
	public interface IStockTransactionAppService : IApplicationService
	{
		Task<StockTransactionDto> CreateStockTransactionImport(CreateStockTransactionImportDto input);

		Task CreateImportRequest(CreateImportRequestDto input);
		Task CreateExportRequest(ExportInputDto input);
		Task UpdateExportStockTransactions(StockTransactionUpdateInput input);
		Task<PagedResultDto<StockTransactionListDto>> GetStockTransactions(GetStockTransactionsInput input);
		Task<StockTransactionListDto> GetStockTransaction(int id);

		Task Update(StockTransactionUpdateInput input);

		Task DeleteStockTransaction(int Id);
		Task<UserDto> GetCustomerByIdStockTransaction(int stockTransactionId);
	}
}
