using BASEDDEPARTMENT.Entities;
using BASEDDEPARTMENT.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace BASEDDEPARTMENT.Services.AccountService
{
	public interface IAccountService
	{
		string GetIdOfAuthorizedUser(ClaimsPrincipal User);
		Task AddToRoleAsync(AppUser user, string roleName);
		Task AssignRoleAndSignIn(AppUser user, string roleName, bool rememberMe);
		Task AssignUserRole(AssignRoleViewModel viewModel);
		Task<IdentityResult> ChangePasswordAsync(AppUser user, string currentPassword, string newPassword);
		Task<IdentityResult> ChangeUserEmailAsync(AppUser user, string newEmail, string token);
		Task<bool> CheckPasswordAsync(AppUser user, string password);
		Task CreateRoleAsync(string identityRole);
		Task<IdentityResult> CreateUserAsync(AppUser user, string password);
		Task<UserProfileViewModel> CreateUserProfileViewModel(string userId);
		Task<string> GenerateChangeEmailTokenAsync(AppUser user, string newEmail);
		CreateRoleViewModel GenerateCreateRoleViewModel();
		Task<EditProfileViewModel> GenerateEditProfileViewModel(string actionName, string id);
		AssignRoleViewModel GetAssignRoleViewModel(string searchString, int currentPage, int pageSize);
		IdentityRole GetRole(string roleId);
		IEnumerable<IdentityRole> GetRoles();
		Task<AppUser> GetUserAsync(string userId);
		Task<AppUser> GetUserByEmailAsync(string email);
		Task<AppUser> GetUserByNameAsync(string userName);
		Task<IEnumerable<Post>> GetUserPostsAsync(string userId);
		IdentityUserRole<string> GetUserRole(string userId);
		Task<SignInResult> PasswordSignInAsync(LoginViewModel model, bool isPersistent, bool lockoutOnFailure);
		Task RemoveFromRoleAsync(AppUser user, string roleName);
		Task<IdentityResult> SetEmailAsync(AppUser user, string email);
		Task<IdentityResult> UpdateUserAsync(AppUser user);
		Task UpdateUserProfileImage(string id, AddImgViewModel model);
		Task UserSignOutAsync();
		Task<bool> UserWithSuchNameExists(string userName);
		bool RequestIsAuthorized(ClaimsPrincipal user, string id);
	}
}