using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Warehouses.Dto
{
	public class WarehouseFormTo
	{
		public WarehouseDetailDto FromWarehouse { get; set; }
		public WarehouseDetailDto ToWarehouse { get; set; }
	}

	public class WarehouseDetailDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Location { get; set; }
	}
}
