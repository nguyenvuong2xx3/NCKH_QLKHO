using System;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Microsoft.AspNetCore.Http;

namespace QLKho_NCKH.Sliders.Dto
{
	public class CreateSliderDto: EntityDto, IHasCreationTime
	{
		public string Title { get; set; }
		public string Description { get; set; }
		public bool IsActive { get; set; }
		public DateTime CreationTime { get; set; }
		public string Image { get; set; }
		public IFormFile ImageFile { get; set; }
	}
}
