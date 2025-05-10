using System;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Runtime.Validation;
using System.IO;

namespace QLKho_NCKH.InventoryItems.Dto
{

	public class InventoryItemCreatingInput
	{
		public int ProductId { get; set; }
		public int StorageLocationId { get; set; }
		public int Quantity { get; set; }
		public int ReservedQuantity { get; set; }
		public decimal UnitPrice { get; set; }

	}

	public class InventoryItemEditDto : EntityDto<int>
	{
		public int ProductId { get; set; }
		public int StorageLocationId { get; set; }
		public int Quantity { get; set; }
		public int ReservedQuantity { get; set; }
		public decimal UnitPrice { get; set; }

	}

	//public class GetInventoryItemsInput : PagedAndSortedInputDto, IShouldNormalize
	//{
	//	public string Filter { get; set; }

	//	public DateTime? StartDate { get; set; }

	//	public DateTime? EndDate { get; set; }

	//	public int? ProductId {get;set;}
	//	public int? StorageLocationId {get;set;}
	//	public int? Quantity {get;set;}
	//	public int? ReservedQuantity {get;set;}
	//	public decimal? UnitPrice {get;set;}
	//	//public void Normalize()
	//	//{
	//	//	if (Sorting.IsNullOrWhiteSpace())
	//	//	{
	//	//		Sorting = "CreationTime DESC";
	//	//	}
	//	//}
	//}
	public class InventoryItemListDto : AuditedEntityDto<int>
	{
		public string ProductName { get; set; }
		public string ProductBarcode { get; set; }
		public int ProductId { get; set; }
		public string Description { get; set; }
		public int StorageLocationId { get; set; }
		public string ProductCode { get; set; }
		public string StorageLocationCode { get; set; }
		public int Quantity { get; set; }
		public int ReservedQuantity { get; set; }
		public decimal UnitPrice { get; set; }

		public InventoryItemListDto()
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
