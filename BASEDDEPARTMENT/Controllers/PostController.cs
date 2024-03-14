using BASEDDEPARTMENT.Models;
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

		public PostController(UserManager<AppUser> userManager, MyDBContext context)
		{
			_userManager = userManager;
			_context = context;
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
				user!.Posts!.Add(new Post {
					Id = Guid.NewGuid().ToString(),
					Content = postContent,
					CreatedDate = DateTime.Now,
					UpdatedDate = DateTime.Now,
				});

				_context.SaveChanges();		
			}
			return RedirectToAction("Profile", "Account");
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> EditPost(string postId)
		{
			if (ModelState.IsValid)
			{
				var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == postId);
				if (post!.UserId != User.FindFirst(ClaimTypes.NameIdentifier)!.Value || post == default)
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
				var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == model.PostId);
				post!.Content = model.Content;
				post!.UpdatedDate = DateTime.Now;
				_context.Posts.Update(post);
				_context.SaveChanges();
				
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
				var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == postId);
				if (post!.UserId != User.FindFirst(ClaimTypes.NameIdentifier)!.Value || post == default)
				{
					return RedirectToAction("Profile", "Account");
				}
				
				_context.Posts.Remove(post!);
				_context.SaveChanges();
			}
			return RedirectToAction("Profile", "Account");
		}
	}
}
