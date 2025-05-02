using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QLKho_NCKH.Categories.Dto
{
	public class UpdateCategoryDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int? ParentId { get; set; }
		//public List<CreateCategoryDto> SubCategories { get; set; } = new List<CreateCategoryDto>();
		//public bool IsActive { get; set; } = true;
		//public List<CategoryListDto> SubCategories { get; set; } = new List<CategoryListDto>();
	}
}
