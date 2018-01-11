using IdentityModel.Client;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Client
{
    class Program
    {
		//public static void Main(string[] args) => MainAsync().GetAwaiter().GetResult();

		public static void Main(string[] args) => SendMailAsync().Wait();

		private static async Task SendMailAsync()
		{
			var apiKey = "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";
			var client = new SendGridClient(apiKey);
			var from = new EmailAddress("test@example.com", "Example User");
			var subject = "Sending with SendGrid is Fun";
			var to = new EmailAddress("traders.com.tw@gmail.com", "Example User");
			var plainTextContent = "and easy to do anywhere, even with C#";
			var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
			var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
			var response = await client.SendEmailAsync(msg);
		}

		private static async Task MainAsync()
		{
			// discover endpoints from metadata
			var disco = await DiscoveryClient.GetAsync("http://localhost:50000");
			if (disco.IsError)
			{
				Console.WriteLine(disco.Error);
				return;
			}

			// request token
			var tokenClient = new TokenClient(disco.TokenEndpoint, "clientApp", "secret");
			var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("alice", "password", "apiApp");

			if (tokenResponse.IsError)
			{
				Console.WriteLine(tokenResponse.Error);
				return;
			}
			

			Console.WriteLine(tokenResponse.Json);
			Console.WriteLine("\n\n");



			// call api
			var client = new HttpClient();
			client.SetBearerToken(tokenResponse.AccessToken);

			var response = await client.GetAsync("http://localhost:50001/api/identity");
			if (!response.IsSuccessStatusCode)
			{
				Console.WriteLine(response.StatusCode);
			}
			else
			{
				var content = await response.Content.ReadAsStringAsync();
				Console.WriteLine(JArray.Parse(content));
			}

			Console.ReadLine();

		}

	}
}
