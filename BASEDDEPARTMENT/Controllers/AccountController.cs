using BASEDDEPARTMENT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BASEDDEPARTMENT.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly MyDBContext _context;

		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager, MyDBContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_context = context;
		}

		[HttpGet]
		[Authorize]
		public IActionResult AssignRole(string searchString = "", int currentPage = 1)
		{
			if (ModelState.IsValid)
			{
				int pageSize = 15;

				var users = _userManager.Users.Where(x => x.UserName.Contains(searchString)).OrderBy(x => x.Id).Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
				var roles = _roleManager.Roles.ToList();

				var userRoles = _context.UserRoles.ToList();

				var viewModel = new AssignRoleViewModel
				{
					Users = users,
					UserRoles = userRoles,
				};

				ViewData["i"] = (currentPage - 1) * pageSize;
				ViewData["CurrentPage"] = currentPage;
				ViewData["TotalPages"] = (int)Math.Ceiling(_userManager.Users.Count() / (double)pageSize);
				ViewData["SearchString"] = searchString;

				ViewBag.Roles = roles;

				return View(viewModel);
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AssignRole(AssignRoleViewModel viewModel)
		{
			if (ModelState.IsValid)
			{
				var user = _userManager.Users.FirstOrDefault(x => x.Id == viewModel.UserId);
				var roleId = _context.UserRoles.FirstOrDefault(x => x.UserId == viewModel.UserId).RoleId;

				if (roleId != default)
				{
					var roleName = _roleManager.Roles.FirstOrDefault(x => x.Id == roleId).Name;
					await _userManager.RemoveFromRoleAsync(user, roleName);
				}

				await _userManager.AddToRoleAsync(user, viewModel.Role);
			}

			return RedirectToAction("AssignRole", "Account");
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public IActionResult CreateRole()
		{
			var roleNames = _context.Roles.Select(x => x.Name).ToList();
			var model = new CreateRoleViewModel();
			model.Roles = roleNames;
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> CreateRole(CreateRoleViewModel role)
		{
			if (ModelState.IsValid)
			{
				await _roleManager.CreateAsync(new IdentityRole { Name = role.RoleName });
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = new IdentityUser { UserName = model.UserName };
				if (await _userManager.FindByNameAsync(model.UserName) != null)
				{
					ModelState.AddModelError(string.Empty, "Username is already taken");

					return View();
				}
				var result = await _userManager.CreateAsync(user, model.Password);

				if (result.Succeeded)
				{
					await _userManager.AddToRoleAsync(user, "Xixo");
					await _signInManager.SignInAsync(user, isPersistent: false);

					return RedirectToAction("Index", "Home");
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, false);

				if (result.Succeeded)
				{
					return RedirectToAction("Index", "Home");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Something went wrong.");
				}
			}

			return View(model);
		}

		[HttpGet]
		public async Task<IActionResult> Login()
		{
			return View();
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Logout()
		{
			_signInManager.SignOutAsync();

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> EditProfile(string actionName = "username")
		{
			if (ModelState.IsValid)
			{
				var id = User.FindFirst(ClaimTypes.NameIdentifier).Value;
				var user = await _userManager.FindByIdAsync(id);
				var userVM = new UserProfileViewModel { UserName = user.UserName!, Id = user.Id, ActionName = actionName, Email = user.Email! };

				return View(userVM);
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[Authorize]
		public IActionResult ChangeUserName(string userName, string id)
		{
			var model = new ChangeUserNameViewModel
			{
				UserName = userName,
				Id = id,
			};
			return View(model);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> ChangeUserName(ChangeUserNameViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(model.Id);
				var result = await _userManager.CheckPasswordAsync(user, model.Password);
				if (!result)
				{
					ModelState.AddModelError(string.Empty, "Incorrect password.");

					return View(model);
				}
				if (await _userManager.FindByNameAsync(model.NewUserName) == null)
				{
					user.UserName = model.NewUserName;
					await _userManager.UpdateAsync(user);

					return RedirectToAction("EditProfile", "Account");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Username is already taken.");
				}
			}
			return View(model);
		}
		[HttpGet]
		[Authorize]
		public IActionResult ChangeUserPassword(string userName, string id)
		{
			var model = new ChangeUserPassViewModel
			{
				UserName = userName,
				Id = id,
			};
			return View(model);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> ChangeUserPassword(ChangeUserPassViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(model.Id!);
				var result = await _userManager.CheckPasswordAsync(user!, model.CurrentPassword!);
				if (!result)
				{
					ModelState.AddModelError(string.Empty, "Current password is incorrect.");

					return View(model);
				}
				if (model.NewPassword == model.ConfirmNewPassword)
				{
					await _userManager.ChangePasswordAsync(user!, model.CurrentPassword!, model.NewPassword!);

					return RedirectToAction("EditProfile", "Account");
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Passwords do not match.");
				}
			}
			return View(model);
		}

		[HttpGet]
		[Authorize]
		public IActionResult AttachUserEmail(string userName, string id)
		{
			var model = new AttachUserEmailViewModel
			{
				UserName = userName,
				Id = id,
			};
			return View(model);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AttachUserEmail(AttachUserEmailViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(model.Id!);
				var passResult = await _userManager.CheckPasswordAsync(user!, model.Password!);
				if (!passResult)
				{
					ModelState.AddModelError(string.Empty, "Incorrect password.");
					return View(model);
				}
				var result = await _userManager.FindByEmailAsync(model.Email!);
				if (result == null)
				{
					await _userManager.SetEmailAsync(user!, model.Email);

					return RedirectToAction("EditProfile", "Account");
				}

				ModelState.AddModelError(string.Empty, "Email is already attached to a profile.");
			}
			return View(model);
		}

		[HttpGet]
		[Authorize]
		public IActionResult ChangeUserEmail(string userName, string id, string email)
		{
			var model = new ChangeUserEmailViewModel
			{
				Email = email,
				UserName = userName,
				Id = id,
			};
			return View(model);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> ChangeUserEmail(ChangeUserEmailViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByIdAsync(model.Id!);
				var passResult = await _userManager.CheckPasswordAsync(user!, model.Password!);
				if (!passResult)
				{
					ModelState.AddModelError(string.Empty, "Incorrect password.");
					return View(model);
				}
				var result = await _userManager.FindByEmailAsync(model.NewEmail!);
				if (result == null)
				{
					var _token = await _userManager.GenerateChangeEmailTokenAsync(user!, model.NewEmail!);

					return RedirectToAction("CheckEmailChangeToken", "Account", new { id = model.Id, newEmail = model.NewEmail, token = _token });
				}

				ModelState.AddModelError(string.Empty, "Email is already attached to a profile.");
			}

			return View(model);
		}

		[HttpGet]
		[Authorize]
		public IActionResult CheckEmailChangeToken(string id, string newEmail, string token)
		{
			var model = new EmailChangeTokenValidationViewModel
			{
				Id = id,
				Email = newEmail,
				TempToken = token,
			};

			return View(model);
		}


		[HttpPost]
		[Authorize]
		public async Task<IActionResult> CheckEmailChangeToken(EmailChangeTokenValidationViewModel model)
		{
			if (ModelState.IsValid)
			{

				var user = await _userManager.FindByIdAsync(model.Id!);
				var result = await _userManager.ChangeEmailAsync(user!, model.Email!, model.Token!);

				if (result.Succeeded)
				{
					return RedirectToAction("EditProfile", "Account", new { actionName = "email" });
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			return View(model);
		}

	}
}
