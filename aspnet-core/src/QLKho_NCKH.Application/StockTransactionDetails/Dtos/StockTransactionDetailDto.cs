using System;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System.IO;
using System.Collections.Generic;

namespace QLKho_NCKH.StockTransactionDetails.Dto {

public class StockTransactionDetailCreatingInput {
    public int StockTransactionId { get; set; }
    public int ProductId { get; set; }
    public int StorageLocationId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string Note { get; set; }
    public string BatchNumber { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public List<StockTransactionDetailEditDto> StockTransactionDetails { get; set; } = new List<StockTransactionDetailEditDto>();
}

public class StockTransactionDetailEditDto: EntityDto<int> {
   public int StockTransactionId {get;set;}
   public int ProductId {get;set;}
   public int StorageLocationId {get;set;}
   public int Quantity {get;set;}
   public decimal UnitPrice {get;set;}
   public string Note {get;set;}
   public string BatchNumber {get;set;}
   public DateTime? ExpiryDate {get;set;}

}

public class GetStockTransactionDetailsInput 
{
	public string Filter { get; set; }

	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public int? StockTransactionId {get;set;}
	public int? ProductId {get;set;}
	public int? StorageLocationId {get;set;}
	public int? Quantity {get;set;}
	public decimal? UnitPrice {get;set;}
	



	//public void Normalize()
	//{
	//	if (Sorting.IsNullOrWhiteSpace())
	//	{
	//		Sorting = "CreationTime DESC";
	//	}
	//}
}
public class StockTransactionDetailListDto : AuditedEntityDto<int> {
  public int StockTransactionId {get;set;}
  public int ProductId {get;set;}
  public int StorageLocationId {get;set;}
  public int Quantity {get;set;}
  public decimal UnitPrice {get;set;}
  public string Note {get;set;}
  public string BatchNumber {get;set;}
  public DateTime? ExpiryDate {get;set;}

  public StockTransactionDetailListDto()
  {
  }
}
  public class FileOutputDto
  {
    public string FileName { get; set; }
    public string FileUrl { get; set; }
    public string ContentType { get; set; }

    public FileOutputDto(string fileName, string fileUrl, string contentType)
    {
      FileName = fileName;
      FileUrl = fileUrl;
      ContentType = contentType;
    }

    public FileOutputDto(string fileName)
    {
      FileName = fileName;
    }

    public FileOutputDto(string fileName, string contentType)
    {
      FileName = Path.GetFileName(fileName);
      ContentType = contentType;
      //FileUrl = fileUrl;
    }
  }

  public class FileDownloadDto(string fileName, string contentType, byte[] bytes) : FileOutputDto(fileName, contentType)
  {
    public byte[] Bytes { get; set; } = bytes;
  }
}
