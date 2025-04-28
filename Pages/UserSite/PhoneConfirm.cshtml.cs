using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ProjectPRN222.Models;
using ProjectPRN222.Services;

namespace ProjectPRN222.Pages.UserSite
{
	public class PhoneConfirmModel : PageModel
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly SmsService _smsService;

		public PhoneConfirmModel(UserManager<User> userManager, SignInManager<User> signInManager, SmsService smsService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_smsService = smsService;
		}

		[BindProperty]
		public string PhoneNumber { get; set; }

		[BindProperty]
		public string VerificationCode { get; set; }

		public bool CodeSent { get; set; } = false;

		public async Task<IActionResult> OnGetAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToPage("/Account/Login");
			return Page();
		}

		public async Task<IActionResult> OnPostSendCodeAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToPage("/Account/Login");

			user.PhoneNumber = PhoneNumber;
			await _userManager.UpdateAsync(user);

			var token = await _userManager.GenerateChangePhoneNumberTokenAsync(user, PhoneNumber);
			await _smsService.SendSmsAsync(PhoneNumber, $"Mã xác minh của bạn là: {token}");
			TempData["Message"] = $"Mã xác minh đã được gửi đến số {PhoneNumber} : {token}";
			CodeSent = true;
			return Page();
		}

		public async Task<IActionResult> OnPostConfirmCodeAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null) return RedirectToPage("/Account/Login");

			var result = await _userManager.ChangePhoneNumberAsync(user, PhoneNumber, VerificationCode);
			if (result.Succeeded)
			{
				TempData["Message"] = "Số điện thoại đã được xác minh.";
				await _signInManager.RefreshSignInAsync(user);
				return RedirectToPage("/UserSite/Profile");
			}

			TempData["Message"] = "Xác minh thất bại. Vui lòng kiểm tra lại mã.";
			CodeSent = true;
			return Page();
		}
	}

}
