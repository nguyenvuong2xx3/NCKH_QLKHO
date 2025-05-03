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
	}
}
