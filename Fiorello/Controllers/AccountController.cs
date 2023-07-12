using Fiorello.Enums;
using Fiorello.Models;
using Fiorello.Utilities.EmailService.EmailSender;
using Fiorello.Utilities.EmailService.EmailSender.Abstract;
using Fiorello.ViewModels.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Fiorello.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IEmailSender _emailSender;

		public AccountController(UserManager<User> userManager,
								SignInManager<User> signInManager,
								RoleManager<IdentityRole> roleManager,
								IEmailSender emailSender)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_emailSender = emailSender;
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(AccountRegisterVM model)
		{
			if(!ModelState.IsValid) return View();

			var user = new User
			{
				UserName = model.Username,
				Email = model.Email,
				Country = model.Country,
				FullName = model.Fullname,
				PhoneNumber = model.PhoneNumber,
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if(!result.Succeeded)
			{
				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);

				return View();
			}

			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var confirmationLink = Url.Action(nameof(ConfirmEmail), "account", new { token, email = user.Email }, Request.Scheme);

			var message = new Message(new string[] { user.Email }, "P331 Email Confirmation", confirmationLink);
			_emailSender.SendEmail(message);

			await _userManager.AddToRoleAsync(user, UserRoles.User.ToString());
			return RedirectToAction(nameof(Login));
		}

		[HttpGet]
		public async Task<IActionResult> ConfirmEmail(string token, string email)
		{
			var user = await _userManager.FindByEmailAsync(email);
			if (user == null)
				return View("Error");
			var result = await _userManager.ConfirmEmailAsync(user, token);
			return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
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

			if (!user.EmailConfirmed)
			{
				ModelState.AddModelError(string.Empty, "Email address must be confirmed to Login");
				return View();
			}

			if (user is null)
			{
				ModelState.AddModelError(string.Empty, "Username or password is incorrect");
				return View();
			}

			var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
			if(!result.Succeeded)
			{
				ModelState.AddModelError(string.Empty, "Username or password is incorrect"); 
				return View();
			}

			if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
				return Redirect(model.ReturnUrl);

			return RedirectToAction(nameof(Index),"home");
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}

		[HttpGet]
		public IActionResult ForgotPassword()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> ForgotPassword(AccountForgotPasswordVM model)
		{
			if (!ModelState.IsValid)  
				return View(model);

			var user = await _userManager.FindByEmailAsync(model.Email);
			if (user == null)
                return Content("User Doesn't exist!");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

			var resetLink = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

			var message = new Message(new string[] { user.Email }, "Reset password", resetLink);
			_emailSender.SendEmail(message);

			return Content("Email Sent!");
		}

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
			var model = new AccountResetPasswordVM
			{
				Email = email,
				Token = token
			};
			return View(model);
        }

		[HttpPost]
		public async Task<IActionResult> ResetPassword(AccountResetPasswordVM model)
		{
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return Content("User Doesn't exist!");

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View();
            }

			return Content("Password has successfully been changed");
        }
    }
}
