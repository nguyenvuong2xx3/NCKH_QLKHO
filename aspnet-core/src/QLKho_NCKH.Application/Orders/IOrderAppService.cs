using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using QLKho_NCKH.Orders.Dto;
using static QLKho_NCKH.Orders.OrderAppService;

namespace QLKho_NCKH.Orders
{
	public interface IOrderAppService : IApplicationService
	{
		Task<PagedResultDto<OrderListDto>> GetAllOrder(GetAllOrdersInput input);
		Task<int> CreateOrder(CreateOrderDto input);
		Task<List<OrderOutput>> GetStatusOrder(int? orderStatus = 5);
		//Task<List<int>> GetStatusOrder(int? orderStatus = 5);
		Task UpdateOrder(UpdateOrderDto input);
		Task<OrderListDto> GetOrderById(int orderId);
	}
}
