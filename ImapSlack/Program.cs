using AE.Net.Mail;

namespace ImapSlack
{
	class Program
	{
		static void Main(string[] args)
		{
			string host = "imap.gmail.com";
			string login = args[0];
			string password = args[1];

			using (var imap = new ImapClient(host, login, password, AuthMethods.Login, port: 993, secure: true))
			{
				var messages = imap.GetMessages(0, 5);
			}
		}
	}
}
