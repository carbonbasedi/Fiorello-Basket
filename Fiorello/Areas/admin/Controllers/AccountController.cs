using Fiorello.Enums;
using Fiorello.Models;
using Fiorello.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Fiorello.Areas.admin.Controllers
{
	[Area("admin")]
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;

		public AccountController(UserManager<User> userManager,
								SignInManager<User> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(AccountLoginVM model)
		{
			if (!ModelState.IsValid) return View();

			var user = await _userManager.FindByNameAsync(model.Username);

			if (user is null)
			{
				ModelState.AddModelError(string.Empty, "Username or password is incorrect");
				return View();
			}

			var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
			if (!result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "Username or password is incorrect");
				return View();
			}

			if (!await HasAccessToAdminPanelAsync(user))
			{
				ModelState.AddModelError(string.Empty, "You don't have permission to admin panel");
				return View();
			}
			return RedirectToAction(nameof(Index), "dashboard");
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}

		private async Task<bool> HasAccessToAdminPanelAsync(User user)
		{
			if (await _userManager.IsInRoleAsync(user, UserRoles.Admin.ToString()) ||
				await _userManager.IsInRoleAsync(user, UserRoles.SuperAdmin.ToString()) ||
				await _userManager.IsInRoleAsync(user, UserRoles.Manager.ToString()) ||
				await _userManager.IsInRoleAsync(user, UserRoles.HR.ToString()))
			{
				return true;
			}
			return false;
		}
	}
}
