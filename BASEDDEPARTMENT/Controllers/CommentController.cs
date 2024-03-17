﻿using BASEDDEPARTMENT.EntityModels;
using BASEDDEPARTMENT.Services.AccountService;
using BASEDDEPARTMENT.Services.CommentService;
using BASEDDEPARTMENT.Services.PostService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BASEDDEPARTMENT.Controllers
{
	public class CommentController : Controller
	{
		private readonly ICommentService _commentService;
		private readonly IPostService _postService;
		private readonly IAccountService _accountService;
		private readonly MyDBContext _context;
		public CommentController(MyDBContext context, IPostService postService, IAccountService accountService, ICommentService commentService)
		{
			_context = context;
			_postService = postService;
			_accountService = accountService;
			_commentService = commentService;
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddComment(string commentContent, string userId, string postId)
		{
			var post = await _postService.Get(postId);
			var newComment = new Comment
			{
				Id = Guid.NewGuid().ToString(),
				Content = commentContent,
				CreatedDate = DateTime.Now,
				UpdatedDate = DateTime.Now,
				User = await _accountService.GetUserAsync(userId),
				Post = post,
			};

			_commentService.Create(newComment);

			return RedirectToAction("Profile", "Account", new {userId = post.UserId});
		}

		[HttpGet]
		[Authorize]
		public IActionResult EditComment(string userId, string commentId)
		{
			if (userId == "69420")
			{
				return View();
			}
			return RedirectToAction("Profile", "Account", new {userId = new string(userId)});
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> DeleteComment(string userId, string commentId)
		{
			if(!_accountService.RequestIsAuthorized(User, userId))
			{
				return RedirectToAction("Profile", "Account", new {userId = new string(userId)});
			}
			await _commentService.Delete(commentId);
			return RedirectToAction("Profile", "Account", new { userId = "" });
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> AddReply(string replyContent, string userId, string parentCommentId)
		{
			var parentComment = await _commentService.Get(parentCommentId);
			var post = await _postService.Get(parentComment.PostId);
			var user = await _accountService.GetUserAsync(userId);
			var newComment = new Comment
			{
				Id = Guid.NewGuid().ToString(),
				Content = replyContent,
				CreatedDate = DateTime.Now,
				UpdatedDate = DateTime.Now,
				User = user,
				Post = post,
				ParentComment = parentComment
			};

			_commentService.Create(newComment);

			return RedirectToAction("Profile", "Account", new { userId = post.UserId });
		}
	}
}
