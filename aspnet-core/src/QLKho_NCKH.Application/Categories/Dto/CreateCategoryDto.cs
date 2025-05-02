using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Entities.Auditing;

namespace QLKho_NCKH.Categories.Dto
{
	public class CreateCategoryDto : FullAuditedEntity<int>
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public int? ParentId { get; set; }
		//public List<CreateCategoryDto> SubCategories { get; set; } = new List<CreateCategoryDto>();
	}
}
