namespace QLKho_NCKH.Web.Models.storageLocations
{
	public class EditStorageLocationViewModel
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public int WarehouseId { get; set; }
		public decimal Capacity { get; set; }
		public decimal CurrentVolume { get; set; }
		public bool IsAvailable { get; set; }
		public string WarehouseName { get; set; }
		public string WarehouseCode { get; set; }
	}
}
