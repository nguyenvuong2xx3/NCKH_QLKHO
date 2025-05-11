using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Http;

namespace QLKho_NCKH.Sliders.Dto
{
	public class UpdateSliderDto: EntityDto
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public string Image { get; set; }
		public bool IsActive { get; set; }
		public IFormFile ImageFile { get; set; }


	}
}
