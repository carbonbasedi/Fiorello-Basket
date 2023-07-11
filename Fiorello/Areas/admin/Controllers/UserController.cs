using Fiorello.Areas.admin.ViewModels.User;
using Fiorello.Enums;
using Fiorello.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Fiorello.Areas.admin.Controllers
{
	[Area("admin")]
	[Authorize(Roles = "SuperAdmin, Admin")]
	public class UserController : Controller
	{
		private readonly UserManager<User> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public UserController(UserManager<User> userManager,
							RoleManager<IdentityRole> roleManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var model = new UserIndexVM();

			var users = await _userManager.Users.ToListAsync();
			foreach (var user in users)
			{
				var roles = await _userManager.GetRolesAsync(user);
				if (!await _userManager.IsInRoleAsync(user, UserRoles.SuperAdmin.ToString()))
				{
					var userWithRoles = new UserVM
					{
						Id = user.Id,
						Fullname = user.FullName,
						Email = user.Email,
						Username = user.UserName,
						Roles = roles.ToList(),
					};
					model.Users.Add(userWithRoles);
				}
			}

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Create()
		{
			var model = new UserCreateVM
			{
				Roles = await _roleManager.Roles.Where(r => r.Name != UserRoles.User.ToString() &&
															r.Name != UserRoles.SuperAdmin.ToString())
															.Select(r => new SelectListItem
															{
																Text = r.Name,
																Value = r.Id
															})
				.ToListAsync()
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Create(UserCreateVM model)
		{
			model.Roles = await _roleManager.Roles.Where(r => r.Name != UserRoles.User.ToString() &&
														r.Name != UserRoles.SuperAdmin.ToString())
														.Select(r => new SelectListItem
														{
															Text = r.Name,
															Value = r.Id
														})
														.ToListAsync();

			if (!ModelState.IsValid) return View();

			var user = new User
			{
				UserName = model.Username,
				FullName = model.Fullname,
				Email = model.Email,
				PhoneNumber = model.PhoneNumber,
				Country = model.Country,
			};

			var result = await _userManager.CreateAsync(user, model.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
					ModelState.AddModelError(string.Empty, error.Description);

				return View();
			}

			foreach (var roleId in model.RolesIds)
			{
				var role = await _roleManager.FindByIdAsync(roleId);
				if (role is null)
				{
					ModelState.AddModelError("RolesIDs", "Role under this name doesn't exist");
					return View();
				}

				result = await _userManager.AddToRoleAsync(user, role.Name);
				if (!result.Succeeded)
				{
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
						return View();
					}
				}
			}
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Update(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();

			var roles = await _userManager.GetRolesAsync(user);
			var rolesIds = new List<string>();
			foreach (var roleName in roles)
			{
				var role = await _roleManager.FindByNameAsync(roleName);
				if (role is null)
				{
					throw new Exception("This role doesn't exist");
				}

				rolesIds.Add(role.Id);
			}

			var userWithRoles = new UserUpdateVM
			{
				Fullname = user.FullName,
				Country = user.Country,
				Email = user.Email,
				Username = user.UserName,
				PhoneNumber = user.PhoneNumber,
				Roles = await _roleManager.Roles.Where(r => r.Name != UserRoles.User.ToString() &&
														r.Name != UserRoles.SuperAdmin.ToString())
														.Select(r => new SelectListItem
														{
															Text = r.Name,
															Value = r.Id
														})
														.ToListAsync(),
				RolesIds = rolesIds
			};
			return View(userWithRoles);
		}

		[HttpPost]
		public async Task<IActionResult> Update(string id, UserUpdateVM model)
		{
			model.Roles = await _roleManager.Roles.Where(r => r.Name != UserRoles.User.ToString() &&
														r.Name != UserRoles.SuperAdmin.ToString())
														.Select(r => new SelectListItem
														{
															Text = r.Name,
															Value = r.Id
														})
														.ToListAsync();

			if (!ModelState.IsValid) return View();

			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();

			user.FullName = model.Fullname;
			user.Email = model.Email;
			user.PhoneNumber = model.PhoneNumber;
			user.UserName = model.Username;
			user.Country = model.Country;

			if (model.Password is not null)
			{
				user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.Password);
			}

			var result = await _userManager.UpdateAsync(user);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
					return View();
				}
			}

			var userRoles = await _userManager.GetRolesAsync(user);

			foreach (var roleName in userRoles)
			{
				if (!model.RolesIds.Contains(roleName))
				{
					await _userManager.RemoveFromRoleAsync(user, roleName);
				}
			}

			foreach (var roleId in model.RolesIds)
			{
				var role = await _roleManager.FindByIdAsync(roleId);
				if (role is null)
				{
					ModelState.AddModelError("RoleIds", "Role doesn't exist");
					return View();
				}

				if (!await _userManager.IsInRoleAsync(user, role.Name))
				{
					await _userManager.AddToRoleAsync(user, role.Name);
				}
			}
			return RedirectToAction(nameof(Index));
		}

		[HttpGet]
		public async Task<IActionResult> Details(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();

			var roles = await _userManager.GetRolesAsync(user);

			var userWithRoles = new UserDetailsVM
			{
				Username = user.UserName,
				Fullname = user.FullName,
				Country = user.Country,
				Email = user.Email,
				PhoneNumber = user.PhoneNumber,
				Roles = roles.ToList()
			};

			return View(userWithRoles);
		}

		[HttpGet]
		public async Task<IActionResult> Delete(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();

			var result = await _userManager.DeleteAsync(user);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					throw new Exception(error.Description);
				}
			}

			return RedirectToAction(nameof(Index));
		}
	}
}