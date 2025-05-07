using Microsoft.AspNetCore.Mvc;
using QLKho_NCKH.Controllers;
using QLKho_NCKH.StockTransactionDetails;
using QLKho_NCKH.StockTransactions;
using QLKho_NCKH.StockTransactions.Dtos;
using System.Threading.Tasks;

namespace QLKho_NCKH.Web.Controllers
{
	public class StockTransactionsController : QLKho_NCKHControllerBase
	{
		private readonly IStockTransactionAppService _stockTransactionAppService;
		private readonly IStockTransactionDetailAppService _stockTransactionDetailAppService;
		public StockTransactionsController(IStockTransactionAppService stockTransactionAppService, IStockTransactionDetailAppService stockTransactionDetailAppService)
		{
			_stockTransactionAppService = stockTransactionAppService;
			_stockTransactionDetailAppService = stockTransactionDetailAppService;
		}
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult CreateImport()
		{
			return PartialView("_CreateImportModal");
		}

		//public async Task<IActionResult> CreateImportStockTransactions(CreateImportRequestDto input)
		//{
		//	var import = await _stockTransactionAppService.CreateStockTransactionImport();
		//	var importdetail = await _stockTransactionDetailAppService.CreateStockTransactionDetail(input.ImportRequestDetails);
		//}
	}
}
