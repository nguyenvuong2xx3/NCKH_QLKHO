using System;
using Abp.Application.Services.Dto;

namespace QLKho_NCKH.Sliders.Dto
{
	public class GetAllSlidersInput: PagedAndSortedResultRequestDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public bool? IsActive { get; set; }
		public DateTime CreationTime { get; set; }

		public string Keyword { get; set; }
		
	}
}
