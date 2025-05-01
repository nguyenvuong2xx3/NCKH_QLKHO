using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace QLKho_NCKH.Categories.Dto
{
	public class GetAllCategoriesInput : PagedAndSortedResultRequestDto
	{
		public string Filter { get; set; }
		//public string Sorting { get; set; }
		//public int MaxResultCount { get; set; } = 10;
		//public int SkipCount { get; set; } = 0;
		//public bool IsAscending { get; set; } = true;
		//public string SearchText { get; set; }
		//public string SortingField { get; set; }
		//public string CategoryName { get; set; }
		//public int? ParentId { get; set; }
		//public bool IsActive { get; set; } = true;
		//public GetAllCategoriesInput()
		//{
		//}
	}
}

