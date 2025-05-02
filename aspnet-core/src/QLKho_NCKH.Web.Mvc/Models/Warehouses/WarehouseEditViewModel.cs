namespace QLKho_NCKH.Web.Models.Warehouses
{
	public class WarehouseEditViewModel
	{
		public int Id { get; set; }
		public string Code { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
		public decimal TotalArea { get; set; }
		public bool IsActive { get; set; }
	}
}
