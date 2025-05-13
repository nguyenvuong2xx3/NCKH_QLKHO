using Abp.Net.Mail;
using Abp.UI;
using MyProject.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using QLKho_NCKH.StockTransactions;
using Microsoft.EntityFrameworkCore;
using QLKho_NCKH.StockTransactions.Dtos;
using Microsoft.AspNetCore.Identity;
using QLKho_NCKH.Authorization.Users;

namespace QLKho_NCKH.TestSendMail
{
	public interface ITestSendMailAppService
	{
		Task SendMailOrder();
	}

	public class TestSendMailAppService : QLKho_NCKHAppServiceBase, ITestSendMailAppService
	{
		private readonly IRepository<StockTransaction, int> _stockTransactionRepository;
		private readonly IRepository<StockTransactionDetail, int> _stockTransactionDetailRepository;
		private readonly UserManager _userManager;
		private readonly IEmailSender _emailSender;

		public TestSendMailAppService(IEmailSender emailSender,
			UserManager userManager,
			IRepository<StockTransaction, int> stockTransactionRepository,
			IRepository<StockTransactionDetail, int> stockTransactionDetailRepository
			)
		{
			_userManager = userManager;
			_stockTransactionRepository = stockTransactionRepository;
			_stockTransactionDetailRepository = stockTransactionDetailRepository;
			_emailSender = emailSender;
		}

		public async Task SendMailOrder()
		{
			{
				try
				{

					//var getStockByUserId = await _stockTransactionRepository.GetAll()
					//	.Where(x => x.UserId == AbpSession.UserId)
					//	.ToListAsync();
					//var stockTransactionDetails = await _stockTransactionDetailRepository
					//	.GetAll()
					//	.Include(x => x.Product) // Bao gồm thông tin sản phẩm
					//	.Include(x => x.StorageLocation) // Bao gồm thông tin vị trí lưu trữ
					//	.Where(x => x.StockTransactionId == getStockByUserId.Id) // Lọc theo StockTransactionId
					//	.ToListAsync(); // Lấy tất cả các bản ghi dưới dạng danh sách
					var getuserId = await _userManager.GetUserByIdAsync(AbpSession.UserId.Value);
					DateTime currentTime = DateTime.Now;
					string formattedTime = currentTime.ToString("dd/MM/yyyy HH:mm:ss"); // Định dạng thời gian

					var smtpClient = new SmtpClient("smtp.gmail.com", 587)
					{
						Credentials = new NetworkCredential("mailtestforworkofstudy@gmail.com", "gifp dnrg ovni jhrw"),
						EnableSsl = true
					};

					var mailMessage = new MailMessage
					{
						From = new MailAddress("vuongmot2k3@gmail.com", "Quản trị viên kho"),
						Subject = "Hệ thống bán hàng HUMG",
						Body = $"Bạn vừa đặt đơn hàng của chúng tôi vào {formattedTime} cảm ơn bạn đã đặt hàng của chúng tôi, chi tiết đơn hàng xem tại theo dõi đơn hàng",
						IsBodyHtml = true,
					};

					mailMessage.To.Add(getuserId.EmailAddress);

					await smtpClient.SendMailAsync(mailMessage);
				}
				catch (Exception ex)
				{
					throw new UserFriendlyException("Email bị gián đoạn!", ex);
				}
			}

		}
	}
}
