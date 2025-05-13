using System.Collections.Generic;
using QLKho_NCKH.Sliders.Dto;

namespace QLKho_NCKH.Web.Models.Sliders
{
	public class SliderViewModel
	{
		public IReadOnlyList<SliderListDto> Sliders;
		public SliderViewModel(IReadOnlyList<SliderListDto> sliders) 
		{
			Sliders = sliders;
		}
	}
}
