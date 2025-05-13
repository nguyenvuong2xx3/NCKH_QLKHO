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

namespace QLKho_NCKH.TestSendMail
{
	public interface ITestSendMailAppService
	{
	}
	
	public class TestSendMailAppService : QLKho_NCKHAppServiceBase, ITestSendMailAppService
	{
		private readonly IEmailSender _emailSender;

		public TestSendMailAppService(IEmailSender emailSender)
		{
			_emailSender = emailSender;
		}

		public async Task SendMail()
		{
			try
			{
				var smtpClient = new SmtpClient("smtp.gmail.com", 587)
				{
					Credentials = new NetworkCredential("mailtestforworkofstudy@gmail.com", "gifp dnrg ovni jhrw"),
					EnableSsl = true
				};

				var mailMessage = new MailMessage
				{
					From = new MailAddress("callmesu.pls@gmail.com", "Call Me Su"),
					Subject = "Test Send Mail",
					Body = "This is a test email from QLKho_NCKH application. Please ignore this email.",
					IsBodyHtml = true,
				};

				mailMessage.To.Add("callmesu.pls@gmail.com");

				await smtpClient.SendMailAsync(mailMessage);
			}
			catch (Exception ex)
			{
				throw new UserFriendlyException("Email bị gián đoạn!", ex);
			}
		}

	}
}
