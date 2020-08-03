using EmailTrigger;
using System.Threading.Tasks;

namespace EmailService
{
	public interface IEmailSender
	{
		void SendEmail(EmailMessage message);
		Task SendEmailAsync(EmailMessage message);
	}
}
