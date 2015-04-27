using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AE.Net.Mail;
using Newtonsoft.Json;

namespace ImapSlack
{
	class Program
	{
		static void Main(string[] args)
		{
			string host = "imap.gmail.com";
			string login = args[0];
			string password = args[1];
			string url = args[2];

			using (var imap = new ImapClient(host, login, password, AuthMethods.Login, port: 993, secure: true))
			{
				imap.NewMessage += (sender, eventArgs) =>
				{
					var mailMessage = imap.GetMessages(0, 0, false).Single();
					SendMessage(url, mailMessage.Body).Wait();
				};

				Console.ReadLine();
			}
		}

		private static async Task SendMessage(string url, string body)
		{
			var webRequest = WebRequest.Create(url);
			webRequest.Method = "POST";
			webRequest.ContentType = "application/json; charset=utf-8";

			var stream = await webRequest.GetRequestStreamAsync();
			using (var writer = new StreamWriter(stream))
			{
				string message = JsonConvert.SerializeObject(new { text = body });
				await writer.WriteAsync(message);
			}

			var response = await webRequest.GetResponseAsync();
		}
	}
}
