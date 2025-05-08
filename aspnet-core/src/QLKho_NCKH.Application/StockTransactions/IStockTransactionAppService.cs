using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.EnumCustom;
using QLKho_NCKH.StockTransactions.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.StockTransactions
{
	public interface IStockTransactionAppService
	{
		Task<StockTransactionDto> CreateStockTransactionImport(CreateStockTransactionImportDto input);

		Task CreateImportRequest(CreateImportRequestDto input);
		Task CreateExportRequest(ExportInputDto input);
		Task UpdateExportStockTransactions(StockTransactionUpdateInput input);
		Task<PagedResultDto<StockTransactionListDto>> GetStockTransactions(GetStockTransactionsInput input);
		Task<StockTransactionListDto> GetStockTransaction(int id);

		Task Update(StockTransactionUpdateInput input);

		Task DeleteStockTransaction(int Id);
	}
}
