using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace ProjectPRN222.Services
{
	public class SmsService
	{
		private readonly string _twilioPhoneNumber;

		public SmsService(IConfiguration configuration)
		{
			_twilioPhoneNumber = configuration["Twilio:PhoneNumber"];
		}

		public async Task SendSmsAsync(string toPhoneNumber, string message)
		{
			string normalizedNumber = NormalizePhoneNumber(toPhoneNumber);

			var messageResource = await MessageResource.CreateAsync(
				to: new PhoneNumber(normalizedNumber),
				from: new PhoneNumber(_twilioPhoneNumber),
				body: message
			);

			Console.WriteLine($"Message sent to {normalizedNumber}: {messageResource.Sid}");
		}


		string NormalizePhoneNumber(string phoneNumber)
		{
			if (phoneNumber.StartsWith("0"))
			{
				return "+84" + phoneNumber.Substring(1);
			}
			return phoneNumber;
		}

	}

}
