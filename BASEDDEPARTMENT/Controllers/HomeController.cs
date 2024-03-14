using BASEDDEPARTMENT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BASEDDEPARTMENT.Controllers
{
	[Authorize]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly MyDBContext _context;
		private readonly UserManager<AppUser> _userManager;


		public HomeController(ILogger<HomeController> logger, MyDBContext dbContext, UserManager<AppUser> userManager)
		{
			_logger = logger;
			_context = dbContext;
			_userManager = userManager;
		}

		public async Task<IActionResult> Index(int pageSize = 5)
		{
			var posts = _context.Posts.Take(pageSize).ToList();
			var indexPostVMs = new List<IndexPostViewModel>();
			foreach (var post in posts)
			{
				var user = await _userManager.FindByIdAsync(post.UserId!);
				var imgUrl = user!.ImgUrl;
				if (imgUrl == default)
				{
					imgUrl = @"~/images/EEUy6MCU0AErfve.png";
				}


				var postVM = new IndexPostViewModel
				{
					Content = post.Content,
					PostId = post.Id,
					UserId = post.UserId,
					UserName = user!.UserName,
					UserImgUrl = imgUrl,
					CreatedDate = post.CreatedDate,
					UpdatedDate = post.UpdatedDate,
				};
				indexPostVMs.Add(postVM);
			}

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
