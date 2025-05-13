using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QLKho_NCKH.Sliders.Dto;

namespace QLKho_NCKH.Sliders
{
	public interface ISliderAppService : IApplicationService
	{
		Task<PagedResultDto<SliderListDto>> GetAllSlider(GetAllSlidersInput input);
		Task CreateSlider(CreateSliderDto input);
		Task<SliderListDto> GetSlider(EntityDto<int> input);
		Task UpdateSlider(UpdateSliderDto input);
		Task UpdateActive(EntityDto<int> input);
		Task DeleteSlider(EntityDto<int> input);
		Task<List<SliderListDto>> GetSliderByActive();
	}
}
