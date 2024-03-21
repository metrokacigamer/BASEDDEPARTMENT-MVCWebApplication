using BASEDDEPARTMENT.Entities;
using BASEDDEPARTMENT.Models;
using BASEDDEPARTMENT.Services.AccountService;
using BASEDDEPARTMENT.Services.CommentService;
using BASEDDEPARTMENT.Services.PostService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;

namespace BASEDDEPARTMENT.Controllers
{
    [Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly MyDBContext _context;
		private readonly UserManager<AppUser> _userManager;
		private readonly IAccountService _accountService;
		private readonly IPostService _postService;
		private readonly ICommentService _commentService;

		public HomeController(ILogger<HomeController> logger, MyDBContext dbContext, UserManager<AppUser> userManager, IAccountService accountService, IPostService postService, ICommentService commentService)
		{
			_logger = logger;
			_context = dbContext;
			_userManager = userManager;
			_accountService = accountService;
			_postService = postService;
			_commentService = commentService;
		}

		public async Task<IActionResult> Index(int pageSize = 5)
		{
			var posts = _context.Posts.Take(pageSize).ToList();
			var indexPostVMs = await Task.FromResult(posts.Select(x => _postService.GetPostViewModel(x.Id).Result));
			var temp = new List<PostViewModel>(indexPostVMs);

			indexPostVMs = await _commentService.GenerateCommentSectionForEachPost(indexPostVMs);
			

			return View(indexPostVMs);
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
