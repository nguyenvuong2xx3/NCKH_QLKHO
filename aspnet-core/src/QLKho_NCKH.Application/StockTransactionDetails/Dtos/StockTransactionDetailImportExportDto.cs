using System;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System.IO;
//using QLKho_NCKH.QLKho_NCKH.Dto;

namespace QLKho_NCKH.StockTransactionDetails.Dto {

public class StockTransactionDetailImportDto {
   public string StockTransactionId {get;set;}
   public string ProductId {get;set;}
   public string StorageLocationId {get;set;}
   public string Quantity {get;set;}
   public string UnitPrice {get;set;}
   public string Note {get;set;}
   public string BatchNumber {get;set;}
   public string ExpiryDate {get;set;}
   public string Id {get;set;}

}

public class GetStockTransactionDetailsExportOptionsInput : GetStockTransactionDetailsInput
{
	public int ExportFileType { get; set; } // 1 excel, 2, csv, 3, json
	public bool IsDisplayStockTransactionId { get; set; }
	public bool IsDisplayProductId { get; set; }
	public bool IsDisplayStorageLocationId { get; set; }
	public bool IsDisplayQuantity { get; set; }
	public bool IsDisplayUnitPrice { get; set; }
	public bool IsDisplayNote { get; set; }
	public bool IsDisplayBatchNumber { get; set; }
	public bool IsDisplayExpiryDate { get; set; }

}
}
