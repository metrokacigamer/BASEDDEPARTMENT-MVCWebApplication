using BASEDDEPARTMENT.EntityModels;
using BASEDDEPARTMENT.Models;
using BASEDDEPARTMENT.Services.PostService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BASEDDEPARTMENT.Controllers
{
    public class PostController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly MyDBContext _context;
		private readonly IPostService _postService;

		public PostController(UserManager<AppUser> userManager, MyDBContext context, IPostService postService)
		{
			_userManager = userManager;
			_context = context;
			_postService = postService;
		}


		[HttpGet]
		public async Task<IActionResult> GetPost(string postId)
		{
			var postVM = await _postService.GetPostViewModel(postId);
			return View(postVM);
		}



		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddPost(string userId, string postContent)
		{
			if (ModelState.IsValid)
			{
				if(userId != User.FindFirst(ClaimTypes.NameIdentifier)!.Value)
				{
					return RedirectToAction("Profile", "Account");
				}

				var user = await _userManager.FindByIdAsync(userId);
				await _postService.Create(
					new Post 
					{
						Id = Guid.NewGuid().ToString(),
						Content = postContent,
						CreatedDate = DateTime.Now,
						UpdatedDate = DateTime.Now,
						User = user,
					});

			}
			return RedirectToAction("Profile", "Account");
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> EditPost(string postId)
		{
			if (ModelState.IsValid)
			{
				var post = await _postService.Get(postId);
				var notAuthorizedOrNoSuchPost = post!.UserId != User.FindFirst(ClaimTypes.NameIdentifier)!.Value || post == default;
				if (notAuthorizedOrNoSuchPost)
				{
					return RedirectToAction("Profile", "Account");
				}
				var postVM = new EditPostViewModel { PostId = post!.Id, Content = post.Content };
				return View(postVM);
			}
			return RedirectToAction("Profile", "Account");
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> EditPost(EditPostViewModel model)
		{
			if(ModelState.IsValid)
			{
				var post = await _postService.Get(model.PostId!);
				post!.Content = model.Content;
				post!.UpdatedDate = DateTime.Now;
				await _postService.Update(post);
				
				return RedirectToAction("Profile", "Account");
			}

			return View(model);
		}
		
		[HttpGet]
		[Authorize]
		public async Task<IActionResult> DeletePost(string postId)
		{
			if (ModelState.IsValid)
			{
				var post = await _postService.Get(postId);
				var notAuthorizedOrNoSuchPost = post!.UserId != User.FindFirst(ClaimTypes.NameIdentifier)!.Value || post == default;
				if (notAuthorizedOrNoSuchPost)
				{
					return RedirectToAction("Profile", "Account");
				}

				await _postService.Delete(post!);
			}
			return RedirectToAction("Profile", "Account");
		}
	}
}
