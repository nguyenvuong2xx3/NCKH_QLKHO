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
using Microsoft.EntityFrameworkCore;
using QLKho_NCKH.Carts.Dto;
using QLKho_NCKH.Inventory;
using QLKho_NCKH.InventoryItems;
using QLKho_NCKH.Products;
namespace QLKho_NCKH.Carts
{
	public class CartAppService : QLKho_NCKHAppServiceBase, ICartAppService
	{
		//private readonly IRepository<Cart> _cartRepository;
		private readonly IRepository<CartItem> _cartItemRepository;
		private readonly IRepository<Product> _productRepository;
		private readonly IRepository<InventoryItem> _inventoryItemRepository;

		public CartAppService(
			//IRepository<Cart> cartRepository, 
			IRepository<Product> productRepository,
			IRepository<CartItem> cartItemRepository,
			IRepository<InventoryItem> inventoryItemRepository)
		{
			//	_cartRepository = cartRepository;
			_productRepository = productRepository;
			_cartItemRepository = cartItemRepository;
			_inventoryItemRepository = inventoryItemRepository;
		}
		public async Task<List<CartsDto>> GetAllCart()
		{
			if (AbpSession.UserId == null)
			{
				return null;
			}

			// Lấy danh sách cart của user
			var cartItems = await _cartItemRepository.GetAllListAsync(c => c.UserId == AbpSession.UserId);

			if (cartItems == null || !cartItems.Any())
			{
				return new List<CartsDto>();
			}

			// Lấy danh sách ProductId có trong giỏ hàng
			var productIds = cartItems.Select(c => c.ProductId).Distinct().ToList();

			// Lấy thông tin InventoryItem và Product tương ứng
			var inventoryItems = await _inventoryItemRepository.GetAll()
					.Include(i => i.Product).ToListAsync();
			inventoryItems.Where(i => productIds.Contains(i.ProductId));

			// Gộp dữ liệu lại
			var result = cartItems.Select(cart =>
			{
				var inventory = inventoryItems.FirstOrDefault(i => i.ProductId == cart.ProductId);
				var product = inventory?.Product;

				return new CartsDto
				{
					Id = cart.Id,
					ProductId = cart.ProductId,
					Quantity = cart.Quantity,
					UserId = cart.UserId,
					Name = product?.Name,
					UnitPrice = inventory?.UnitPrice ?? 0,
					//TotalPrice = (inventory?.Price ?? 0) * cart.Quantity,
					//Location = inventory?.Location
				};
			}).ToList();

			return result;
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

