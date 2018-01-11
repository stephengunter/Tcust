using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;

namespace IdentityApp.Services
{
	
	public interface IEmailSender
	{
		Task SendEmailAsync(string email, string subject, string message);
	}

	// This class is used by the application to send email for account confirmation and password reset.
	// For more details see https://go.microsoft.com/fwlink/?LinkID=532713
	public class EmailSender : IEmailSender
    {
		private readonly IOptions<AppSettings> settings;

		public EmailSender(IOptions<AppSettings> settings)
		{
			this.settings = settings;
		}
		

		public Task SendEmailAsync(string email, string subject, string message)
		{
			return Execute( subject, message, email);
		}

		public Task Execute(string subject, string message, string email)
		{
			string apiKey = settings.Value.EmailApiKey;
			var client = new SendGridClient(apiKey);

			string siteEmail = settings.Value.Email;
			string siteTitle = settings.Value.Title;

			var msg = new SendGridMessage()
			{
				From = new EmailAddress(siteEmail, siteTitle),
				Subject = subject,
				PlainTextContent = message,
				
				HtmlContent = message
			};
			msg.AddTo(new EmailAddress(email));
			return client.SendEmailAsync(msg);
		}
	}
}
