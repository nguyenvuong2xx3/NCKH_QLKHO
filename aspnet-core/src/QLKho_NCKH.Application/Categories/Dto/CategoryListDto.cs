using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace QLKho_NCKH.Categories.Dto
{
	public class CategoryListDto : FullAuditedEntity<int>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int? ParentId { get; set; } 
		public DateTime CreateTime { get; set; } 
		//public bool IsActive { get; set; } = true;
		//public List<CategoryListDto> SubCategories { get; set; } = new List<CategoryListDto>();
	}
}
