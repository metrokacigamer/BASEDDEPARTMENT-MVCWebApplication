using BASEDDEPARTMENT.Entities;
using BASEDDEPARTMENT.Models;
using BASEDDEPARTMENT.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Data;
using System.Security.Claims;

namespace BASEDDEPARTMENT.Services.AccountService
{
	public class AccountService : IAccountService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly MyDBContext _context;

		public AccountService(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, MyDBContext context, SignInManager<AppUser> signInManager)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_context = context;
			_signInManager = signInManager;
		}

		public IEnumerable<IdentityRole> GetRoles()
		{
			return _roleManager.Roles.ToList();
		}

		public AssignRoleViewModel GetAssignRoleViewModel(string searchString, int currentPage, int pageSize)
		{
			var users = _userManager.Users.Where(x => x.UserName!.Contains(searchString))
							.OrderBy(x => x.Id)
							.Skip((currentPage - 1) * pageSize)
							.Take(pageSize).ToList();

			var userRoles = _context.UserRoles.ToList();
			var viewModel = new AssignRoleViewModel
			{
				Users = users,
				UserRoles = userRoles,
			};

			return viewModel;
		}

		public async Task<AppUser> GetUserAsync(string userId)
		{
			return await Task.FromResult(await _userManager.FindByIdAsync(userId));
		}

		public IdentityUserRole<string> GetUserRole(string userId)
		{
			return _context.UserRoles.FirstOrDefault(x => x.UserId.ToString() == userId);
		}

		public IdentityRole GetRole(string roleId)
		{
			return _roleManager.Roles.FirstOrDefault(x => x.Id == roleId);
		}

		public async Task RemoveFromRoleAsync(AppUser appUser, string roleName)
		{
			await _userManager.RemoveFromRoleAsync(appUser!, roleName!);
		}

		public async Task AddToRoleAsync(AppUser appUser, string roleName)
		{
			await _userManager.AddToRoleAsync(appUser!, roleName!);
		}

		public async Task AssignUserRole(AssignRoleViewModel viewModel)
		{
			var user = await GetUserAsync(viewModel.UserId);
			var roleId = GetUserRole(viewModel.UserId)!.RoleId;

			if (roleId != default)
			{
				var roleName = GetRole(roleId)!.Name;

				await RemoveFromRoleAsync(user!, roleName!);
			}

			await AddToRoleAsync(user!, viewModel.Role!);
		}

		public CreateRoleViewModel GenerateCreateRoleViewModel()
		{
			var roleNames = _context.Roles.Select(x => x.Name).ToList();
			var model = new CreateRoleViewModel();
			model.Roles = roleNames!;
			return model;
		}

		public async Task CreateRoleAsync(string roleName)
		{
			await _roleManager.CreateAsync(new IdentityRole { Name = roleName });
		}

		public async Task<bool> UserWithSuchNameExists(string userName)
		{
			return await Task.FromResult(await _userManager.FindByNameAsync(userName) != null);
		}

		public async Task<IdentityResult> CreateUserAsync(AppUser user, string password)
		{
			return await _userManager.CreateAsync(user, password);
		}

		public async Task AssignRoleAndSignIn(AppUser user, string roleName, bool rememberMe)
		{
			await _userManager.AddToRoleAsync(user, roleName);
			await _signInManager.SignInAsync(user, rememberMe);
		}

		public async Task<SignInResult> PasswordSignInAsync(LoginViewModel model, bool isPersistent, bool lockoutOnFailure)
		{
			return await Task.FromResult(await _signInManager.PasswordSignInAsync(model.UserName!, model.Password!, isPersistent, lockoutOnFailure));
		}

		public async Task UserSignOutAsync()
		{
			await _signInManager.SignOutAsync();
		}

		public async Task<EditProfileViewModel> GenerateEditProfileViewModel(string actionName, string id)
		{
			var user = await GetUserAsync(id);
			return new EditProfileViewModel { UserName = user!.UserName!, Id = user.Id, ActionName = actionName, Email = user.Email! };
		}

		public async Task<bool> CheckPasswordAsync(AppUser user, string password)
		{
			return await Task.FromResult(await _userManager.CheckPasswordAsync(user!, password));
		}

		public async Task<AppUser> GetUserByNameAsync(string userName)
		{
			return await Task.FromResult(await _userManager.FindByNameAsync(userName!));
		}

		public async Task<IdentityResult> UpdateUserAsync(AppUser user)
		{
			return await Task.FromResult(await _userManager.UpdateAsync(user));
		}

		public async Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword)
		{
			return await Task.FromResult(await _userManager.ChangePasswordAsync(user, currentPassword, newPassword));
		}

		public async Task<IdentityResult> SetEmailAsync(AppUser user, string email)
		{
			return await Task.FromResult(await _userManager.SetEmailAsync(user, email));
		}

		public async Task<AppUser> GetUserByEmailAsync(string email)
		{
			return await Task.FromResult(await _userManager.FindByEmailAsync(email));
		}

		public async Task<string> GenerateChangeEmailTokenAsync(AppUser user, string newEmail)
		{
			return await Task.FromResult(await _userManager.GenerateChangeEmailTokenAsync(user, newEmail));
		}

		public async Task<IdentityResult> ChangeUserEmailAsync(AppUser user, string newEmail, string token)
		{
			return await Task.FromResult(await _userManager.ChangeEmailAsync(user!, newEmail!, token!));
		}

		public async Task<IEnumerable<Post>> GetUserPostsAsync(string userId)
		{
			return await Task.FromResult(_context.Posts.Where(x => x.UserId == userId).ToList());
		}

		public async Task<string> GetUserProfilePicture(string userId)
		{
			var user = await GetUserAsync(userId);
			return await Task.FromResult(user.Images.FirstOrDefault(x => x.ImageType == Enums.ImageType.ProfileImage)?.ImgUrl);
		}

		private IEnumerable<ImageViewModel> GetImageVMs(IEnumerable<Image> images)
		{
			var _images = new List<ImageViewModel>(images.Select(async image => new ImageViewModel
			{
				ImageId = image.Id,
				ImagePath = image.ImgUrl,
				UserId = image.UserId,
				UserName = image.User.UserName,
				ProfileImagePath = await GetUserProfilePicture(image.UserId),
				UploadedDate = image.UploadDate,
			}).Select(x => x.Result).OrderByDescending(x => x.UploadedDate));


			return _images;
		}

		public async Task<UserProfileViewModel> CreateUserProfileViewModel(string userId)
		{
			var user = await GetUserAsync(userId);
			var posts = await GetUserPostsAsync(userId);
			var vm = new UserProfileViewModel
			{
				Id = user!.Id,
				UserName = user!.UserName,
				ImgUrl = await GetUserProfilePicture(user.Id),
				Posts = posts.Select(async post => new PostViewModel
				{
					UserId = userId,
					Content = post.Content,
					CreatedDate = post.CreatedDate,
					UpdatedDate = post.UpdatedDate,
					PostId = post.Id,
					UserImgUrl = await GetUserProfilePicture(post.UserId),
					Images = GetImageVMs(post.Images),
					Comments = post.Comments.Where(comment => comment.ParentComment == null)
										 .Select(async comment => new CommentViewModel //need to add Take(N) and Replies
										 {
											 UserName = (await GetUserAsync(comment.UserId)).UserName,
											 UserId = comment.UserId,
											 Id = comment.Id,
											 Content = comment.Content,
											 AuthorProfileImage = await GetUserProfilePicture(comment.UserId),
											 CreatedDate = comment.CreatedDate,
											 UpdatedDate = comment.UpdatedDate,
											 Replies = comment.Comments.Select(async reply => new CommentViewModel
											 {
												 UserName = (await GetUserAsync(reply.UserId)).UserName,
												 UserId = reply.UserId,
												 Id = reply.Id,
												 Content = reply.Content,
												 AuthorProfileImage = await GetUserProfilePicture(reply.UserId),
												 CreatedDate = reply.CreatedDate,
												 UpdatedDate = reply.UpdatedDate,
											 }).Select(x => x.Result).OrderByDescending(x => x.CreatedDate),
										 }).Select(x => x.Result).OrderByDescending(x => x.CreatedDate),
				}).Select(x => x.Result).OrderByDescending(x => x.CreatedDate),
			};

			return vm;
		}

		public async Task UpdateUserProfileImage(string id, AddImgViewModel model)
		{
			var request = new HttpClient();
			await request.GetAsync(model.ImgUrl);
			var user = await GetUserAsync(id);

			var image = user.Images.FirstOrDefault(x => x.ImageType == Enums.ImageType.ProfileImage);
			image.ImageType = Enums.ImageType.PostImage;
			_context.Images.Update(image);

			user.Images.Add(new Image
			{
				Id = Guid.NewGuid().ToString(),
				ImageType = Enums.ImageType.ProfileImage,
			});
			await _userManager.UpdateAsync(user);

			_context.SaveChanges();
		}

		public string GetIdOfAuthorizedUser(ClaimsPrincipal User)
		{
			return User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
		}

		public bool RequestIsAuthorized(ClaimsPrincipal User, string id)
		{
			return id == GetIdOfAuthorizedUser(User);
		}
	}
}