using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using QLKho_NCKH.Carts.Dto;

namespace QLKho_NCKH.Carts
{
	public interface ICartAppService : IApplicationService
	{
		Task<List<CartsDto>> GetAllCart();
		Task AddToCart(int productId, int quantity, bool check);

		Task DeleteCart(int productId);

		Task UpdateCart(int productId, int quantity);
		Task ClearCart(long userId);
	}
}
