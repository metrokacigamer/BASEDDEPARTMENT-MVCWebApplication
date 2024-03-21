using BASEDDEPARTMENT.Entities;
using BASEDDEPARTMENT.Models;
using BASEDDEPARTMENT.Services.AccountService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BASEDDEPARTMENT.Controllers
{
	[Authorize]
    public class AccountController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly MyDBContext _context;
		private readonly IAccountService _accountService;

		public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, MyDBContext context, IAccountService accountService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_context = context;
			_accountService = accountService;
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public IActionResult AssignRole(string searchString = "", int currentPage = 1)
		{
			if (ModelState.IsValid)
			{
				int pageSize = 15;

				var viewModel = _accountService.GetAssignRoleViewModel(searchString, currentPage, pageSize);

				var roles = _accountService.GetRoles();

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
				await _accountService.AssignUserRole(viewModel);
			}

			return RedirectToAction("AssignRole", "Account");
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public IActionResult CreateRole()
		{
			var model = _accountService.GenerateCreateRoleViewModel();
			return View(model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> CreateRole(CreateRoleViewModel role)
		{
			if (ModelState.IsValid)
			{
				await _accountService.CreateRoleAsync(role.RoleName);
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			if (ModelState.IsValid)
			{
				var userWithSuchNameExists = await _accountService.UserWithSuchNameExists(model.UserName);
				if (userWithSuchNameExists)
				{
					ModelState.AddModelError(string.Empty, "Username is already taken");

					return View();
				}

				var user = new AppUser { UserName = model.UserName };
				var result = await _accountService.CreateUserAsync(user, model.Password);

				if (result.Succeeded)
				{
					await _accountService.AssignRoleAndSignIn(user, "Xixo", false); // false = isPersistent :

					return RedirectToAction("Index", "Home");
				}

				GetErrors(result);
			}

			return View(model);
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> Login(LoginViewModel model)
		{
			if (ModelState.IsValid)
			{
				var lockoutOnFailure = false;
				var isPersistent = false;
				var result = await _accountService.PasswordSignInAsync(model, isPersistent, lockoutOnFailure);
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
		[AllowAnonymous]
		public IActionResult Login()
		{
			return View();
		}

		[HttpGet]
		public async Task<IActionResult> Logout()
		{
			await _accountService.UserSignOutAsync();
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public async Task<IActionResult> EditProfile(string actionName = "username")
		{
			if (ModelState.IsValid)
			{
				var id = _accountService.GetIdOfAuthorizedUser(User);
				var userVM = await _accountService.GenerateEditProfileViewModel(actionName, id);

				return View(userVM);
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult ChangeUserName(string userName, string id)
		{
			if (!_accountService.RequestIsAuthorized(User, id))
			{
				return RedirectToAction("Index", "Home");
			}

			var model = new ChangeUserNameViewModel
			{
				UserName = userName,
				Id = id,
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ChangeUserName(ChangeUserNameViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _accountService.GetUserAsync(model.Id!);
				var result = await _accountService.CheckPasswordAsync(user!, model.Password!);

				if (!result)
				{
					ModelState.AddModelError(string.Empty, "Incorrect password.");

					return View(model);
				}
				else if (await _accountService.GetUserByNameAsync(model.NewUserName) == null)
				{
					user!.UserName = model.NewUserName;
					var res = await _accountService.UpdateUserAsync(user);

					if (res.Succeeded)
					{
						return RedirectToAction("EditProfile", "Account");
					}

					GetErrors(res);
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Username is already taken.");
				}
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult ChangeUserPassword(string userName, string id)
		{
			if (!_accountService.RequestIsAuthorized(User, id))
			{
				return RedirectToAction("Index", "Home");
			}
			var model = new ChangeUserPassViewModel
			{
				UserName = userName,
				Id = id,
			};
			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ChangeUserPassword(ChangeUserPassViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _accountService.GetUserAsync(model.Id!);
				var result = await _accountService.CheckPasswordAsync(user!, model.CurrentPassword!);

				if (!result)
				{
					ModelState.AddModelError(string.Empty, "Current password is incorrect.");

					return View(model);
				}

				var res = await _accountService.ChangePasswordAsync(user!, model.CurrentPassword!, model.NewPassword!);

				if (res.Succeeded)
				{
					return RedirectToAction("EditProfile", "Account");
				}
				GetErrors(res);
			}
			return View(model);
		}

		[NonAction]
		private void GetErrors(IdentityResult res)
		{
			foreach (var error in res.Errors)
			{
				ModelState.AddModelError(string.Empty, error.Description);
			}
		}

		[HttpGet]
		public IActionResult AttachUserEmail(string userName, string id)
		{
			if (!_accountService.RequestIsAuthorized(User, id))
			{
				return RedirectToAction("Index", "Home");
			}
			var model = new AttachUserEmailViewModel
			{
				UserName = userName,
				Id = id,
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> AttachUserEmail(AttachUserEmailViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _accountService.GetUserAsync(model.Id!);
				var passResult = await _accountService.CheckPasswordAsync(user!, model.Password!);

				if (!passResult)
				{
					ModelState.AddModelError(string.Empty, "Incorrect password.");

					return View(model);
				}

				var result = await _accountService.GetUserByEmailAsync(model.Email!);

				if (result == null)
				{
					var res = await _accountService.SetEmailAsync(user, model.Email);
					if (res.Succeeded)
					{
						return RedirectToAction("EditProfile", "Account");
					}
					GetErrors(res);
				}
				else
				{
					ModelState.AddModelError(string.Empty, "Email is already attached to a profile.");
				}
			}
			return View(model);
		}

		[HttpGet]
		public IActionResult ChangeUserEmail(string userName, string id, string email)
		{
			if (!_accountService.RequestIsAuthorized(User, id))
			{
				return RedirectToAction("Index", "Home");
			}

			var model = new ChangeUserEmailViewModel
			{
				Email = email,
				UserName = userName,
				Id = id,
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> ChangeUserEmail(ChangeUserEmailViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _accountService.GetUserAsync(model.Id!);
				var passResult = await _accountService.CheckPasswordAsync(user!, model.Password!);
				if (!passResult)
				{
					ModelState.AddModelError(string.Empty, "Incorrect password.");
					return View(model);
				}
				var result = await _accountService.GetUserByEmailAsync(model.NewEmail!);
				if (result == null)
				{
					var _token = await _accountService.GenerateChangeEmailTokenAsync(user!, model.NewEmail!);

					return RedirectToAction("CheckEmailChangeToken", "Account", new { id = model.Id, newEmail = model.NewEmail, token = _token });
				}

				ModelState.AddModelError(string.Empty, "Email is already attached to a profile.");
			}

			return View(model);
		}

		[HttpGet]
		public IActionResult CheckEmailChangeToken(string id, string newEmail, string token)
		{
			if (!_accountService.RequestIsAuthorized(User, id))
			{
				return RedirectToAction("Index", "Home");
			}

			var model = new EmailChangeTokenValidationViewModel
			{
				Id = id,
				Email = newEmail,
				TempToken = token,
			};

			return View(model);
		}

		[HttpPost]
		public async Task<IActionResult> CheckEmailChangeToken(EmailChangeTokenValidationViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _accountService.GetUserAsync(model.Id!);
				var result = await _accountService.ChangeUserEmailAsync(user!, model.Email!, model.Token!);

				if (result.Succeeded)
				{
					return RedirectToAction("EditProfile", "Account", new { actionName = "email" });
				}
				GetErrors(result);
			}

			return View(model);
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<IActionResult> Profile(string userId = "")
		{
			if (ModelState.IsValid)
			{
				if(userId.IsNullOrEmpty())
				{
					userId = _accountService.GetIdOfAuthorizedUser(User);
				}

				var userVM = await _accountService.CreateUserProfileViewModel(userId);
				return View(userVM);
			}
			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult AddProfilePicture()
		{
			return View();
		}

		[HttpPost]
        public async Task<IActionResult> AddProfilePicture(AddImgViewModel model)
        {
			if (ModelState.IsValid)
			{
				try
				{
					await _accountService.UpdateUserProfileImage(_accountService.GetIdOfAuthorizedUser(User), model);
				}
				catch (Exception ex)
				{
					ModelState.AddModelError(string.Empty, ex.Message);
					return View();
				}
            }

            return RedirectToAction("Profile", "Account");
        }
    }
}
