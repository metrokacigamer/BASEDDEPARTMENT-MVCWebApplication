using BASEDDEPARTMENT.Entities;
using BASEDDEPARTMENT.Models;
using BASEDDEPARTMENT.Services.AccountService;
using BASEDDEPARTMENT.Services.CommentService;
using BASEDDEPARTMENT.Services.ImageService;
using BASEDDEPARTMENT.Services.PostService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace BASEDDEPARTMENT.Controllers
{
    public class PostController : Controller
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly MyDBContext _context;
		private readonly IPostService _postService;
		private readonly IAccountService _accountService;
		private readonly IImageService _imageService;
		private readonly ICommentService _commentService;

		public PostController(UserManager<AppUser> userManager, MyDBContext context, IPostService postService, IAccountService accountService, IImageService imageService, ICommentService commentService)
		{
			_userManager = userManager;
			_context = context;
			_postService = postService;
			_accountService = accountService;
			_imageService = imageService;
			_commentService = commentService;
		}


		[HttpGet]
		public async Task<IActionResult> GetPost(string postId)
		{
			var postVM = await _postService.GetPostViewModel(postId);
			postVM.Comments = await _commentService.GenerateCommentSectionForPost(postId);

			return View(postVM);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddPost(string userId, string postContent, IFormFile imageFile)
		{
			if (ModelState.IsValid)
			{
				if(!_accountService.RequestIsAuthorized(User, userId))
				{
					return RedirectToAction("Profile", "Account");
				}
			
				string fileExtension = Path.GetExtension(imageFile.FileName).ToLower();

				if (imageFile != null && imageFile.Length > 0 && !(fileExtension != ".jpg" && fileExtension != ".jpeg" && fileExtension != ".png"))
				{
					var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
					var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
					var relativePath = $@"~/images/{fileName}";
					using (var stream = new FileStream(imagePath, FileMode.Create))
					{
						await imageFile.CopyToAsync(stream);
					}

					var _user = await _accountService.GetUserAsync(userId);

					var post = new Post
					{
						Id = Guid.NewGuid().ToString(),
						Content = postContent,
						CreatedDate = DateTime.Now,
						UpdatedDate = DateTime.Now,
						User = _user,
					};

					await _postService.Create(post);

					post = await _postService.Get(post.Id);

					var image = new Image
					{
						Id = Guid.NewGuid().ToString(),
						ImageType = Enums.ImageType.PostImage,
						ImgUrl = relativePath,
						Post = post,
						User = _user,
						UploadDate = DateTime.Now,
					};
					
					await _imageService.AddImage(image);

					return RedirectToAction("GetPost", "Post", new { postId = post.Id });
				}
				
				var user = await _userManager.FindByIdAsync(userId);
				
				var _post = new Post 
				{
					Id = Guid.NewGuid().ToString(),
					Content = postContent,
					CreatedDate = DateTime.Now,
					UpdatedDate = DateTime.Now,
					User = user,
				};
				
				await _postService.Create(_post);

				return RedirectToAction("GetPost", "Post", new { postId = _post.Id });
			}
			return RedirectToAction("Index", "Home");
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

		[HttpGet]
		public async Task<IActionResult> GetImage(string imageId)
		{
			var image = await _imageService.GetImage(imageId);
			var imageVM = new ImageViewModel
			{
				ImageId = imageId,
				UploadedDate = image.UploadDate,
				UserId = image.UserId,
				UserName = (await _accountService.GetUserAsync(image.UserId)).UserName,
				ImagePath = image.ImgUrl,
				ProfileImagePath = (await _accountService.GetUserAsync(image.UserId)).Images
												.FirstOrDefault(x => x.ImageType == Enums.ImageType.ProfileImage).ImgUrl,
			};

			return View(imageVM);
		}
	}
}
