using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using QLKho_NCKH.Carts.Dto;
using QLKho_NCKH.InventoryItems;
using QLKho_NCKH.Products;
namespace QLKho_NCKH.Carts
{
	public class CartAppService : QLKho_NCKHAppServiceBase, ICartAppService
	{
		//private readonly IRepository<Cart> _cartRepository;
		private readonly IRepository<CartItem> _cartItemRepository;
		private readonly IRepository<Product> _productRepository;
		private readonly IInventoryItemAppService _inventoryItemAppService;

		public CartAppService(
			//IRepository<Cart> cartRepository, 
			IRepository<Product> productRepository,
			IRepository<CartItem> cartItemRepository,
			IInventoryItemAppService inventoryItemAppService)
		{
			//	_cartRepository = cartRepository;
			_productRepository = productRepository;
			_cartItemRepository = cartItemRepository;
			_inventoryItemAppService = inventoryItemAppService;
		}
		public async Task<List<CartsDto>> GetAllCart()
		{
			if (AbpSession.UserId != null)
			{
				var productList = await _productRepository.GetAllListAsync();

				var cart = await _cartItemRepository.GetAllListAsync(c => c.UserId == AbpSession.UserId);
				if (cart != null)
				{
					return cart.Select(c => new CartsDto
					{
						Id = c.Id,
						ProductId = c.ProductId,
						Quantity = c.Quantity,
						UserId = c.UserId,
						Name = productList.FirstOrDefault(p => p.Id == c.ProductId)?.Name
					}).ToList();
				}
				else
				{
					return new List<CartsDto>();
				}
			}
			else
			{
				return null;
			}
		}

		public async Task AddToCart(int productId, int quantity, bool check)
		{

			if (AbpSession.UserId != null)
			{
				var cartItem = await _cartItemRepository.FirstOrDefaultAsync(c => c.UserId == AbpSession.UserId && c.ProductId == productId);
				if (cartItem != null)
				{
					if (check == true)
					{
						cartItem.Quantity += quantity;
					}
					else
					{
						cartItem.Quantity -= quantity;
					}
				}
				else
				{
					cartItem = new CartItem
					{
						UserId = AbpSession.UserId.Value,
						ProductId = productId,
						Quantity = quantity
					};
				}
				await _cartItemRepository.InsertOrUpdateAsync(cartItem);
			}
		}

		public async Task DeleteCart(int productId)
		{
			if (AbpSession.UserId != null)
			{
				var cartItem = await _cartItemRepository.FirstOrDefaultAsync(c => c.UserId == AbpSession.UserId && c.ProductId == productId);
				if (cartItem != null)
				{
					await _cartItemRepository.DeleteAsync(cartItem);
				}
			}
		}

		public async Task ClearProduct(int productId)
		{
			if (AbpSession.UserId != null)
			{
				var cartItem = await _cartItemRepository.GetAllListAsync(c => c.UserId == AbpSession.UserId && c.ProductId == productId);
				if (cartItem != null)
				{
					foreach (var item in cartItem)
					{
						await _cartItemRepository.DeleteAsync(item);
					}
				}
			}
		}

		public async Task UpdateCart(int productId, int quantity)
		{
			if (AbpSession.UserId != null)
			{
				var cartItem = await _cartItemRepository.FirstOrDefaultAsync(c => c.UserId == AbpSession.UserId && c.ProductId == productId);
				if (cartItem != null)
				{
					cartItem.Quantity = quantity;
					await _cartItemRepository.UpdateAsync(cartItem);
				}
			}
		}

		public async Task ClearCart(long userId)
		{
			var userCartItems = await _cartItemRepository.GetAllListAsync(c => c.UserId == userId);

			foreach (var cartItem in userCartItems)
			{
				await _cartItemRepository.DeleteAsync(cartItem);
			}
		}
	}
}

