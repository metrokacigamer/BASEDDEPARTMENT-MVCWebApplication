using BASEDDEPARTMENT.Entities;
using BASEDDEPARTMENT.Models;
using BASEDDEPARTMENT.Repositories;
using Microsoft.Extensions.Hosting;
using System.ComponentModel.Design;

namespace BASEDDEPARTMENT.Services.CommentService
{
	public class CommentService: ICommentService
	{
		private readonly IRepository<Comment> _commentRepository;
		private readonly MyDBContext _context;
		public CommentService(IRepository<Comment> commentRepository, MyDBContext context)
		{
			_commentRepository = commentRepository;
			_context = context;
		}

		public bool Create(Comment comment)
		{
			try
			{
				_commentRepository.Create(comment);
				return true;
			}
			catch (Exception)
			{
				return false;
			}
		}

		public async Task<Comment> Get(string id)
		{
			return await Task.FromResult(_commentRepository.Get(id));
		}

		public async Task<IEnumerable<Comment>> GetAll() 
		{
			return await Task.FromResult(_commentRepository.GetAll());
		}

		public async Task<bool> Delete(string id)
		{
			var comment = _commentRepository.Get(id);
			if(comment != null)
			{
				_commentRepository.Delete(comment);
				return await Task.FromResult(true);
			}

			return await Task.FromResult(false);
		}

		public async Task<bool> Update(Comment comment)
		{
			try
			{
				comment.UpdatedDate = DateTime.Now;

				_commentRepository.Update(comment);
				return await Task.FromResult(true);
			}
			catch (Exception) 
			{
				return await Task.FromResult(false);
			}
		}

		public async Task<string> GetAuthorProfileImage(string userId)
		{
			return await Task.FromResult(_context.Users.FirstOrDefault(x => x.Id == userId).Images.FirstOrDefault(x => x.ImageType == Enums.ImageType.ProfileImage)?.ImgUrl);
		}

		public async Task<IEnumerable<CommentViewModel>> GetCommentGenerations(string commentId, int generations)
		{
			if(generations == 0)
			{
				return Enumerable.Empty<CommentViewModel>();
			}
			var comment = _commentRepository.Get(commentId);

			if(comment.Comments.Count == 0)
			{
				return Enumerable.Empty<CommentViewModel>();
			}

			var replies = new List<CommentViewModel>();
			foreach(var reply in comment.Comments)
			{
				replies.Add(new CommentViewModel
				{
					HasParentComments = reply.ParentCommentId != default ? true : false,
					HasChildComments = reply.Comments.Count > 0 ? true : false,
					AuthorProfileImage = await GetAuthorProfileImage(reply.UserId),
					Content = reply.Content,
					CreatedDate = reply.CreatedDate,
					UpdatedDate = reply.UpdatedDate,
					Id = reply.Id,
					UserId = reply.UserId,
					UserName = reply.User.UserName,
					Replies = await GetCommentGenerations(reply.Id, generations - 1),
				});
			}
			return replies;
		}

		public async Task<CommentViewModel> GetViewModel(string commentId)
		{
			var comment = _commentRepository.Get(commentId);
			var post = comment.Post;
			var commentVM = new CommentViewModel
			{
				HasParentComments = comment.ParentCommentId != default ? true : false,
				HasChildComments = comment.Comments.Count > 0 ? true: false,
				AuthorProfileImage = await GetAuthorProfileImage(comment.UserId),
				Content = comment.Content,
				CreatedDate = comment.CreatedDate,
				UpdatedDate = comment.UpdatedDate,
				Id = comment.Id,
				UserId = comment.UserId,
				UserName = comment.User.UserName,
				Replies = await GetCommentGenerations(comment.Id, 2),
				Post = new PostViewModel
				{
					PostId = post.Id,
					UserId = post.UserId,
					UserName = post.User.UserName,
					UserImgUrl = await GetAuthorProfileImage(post.UserId),
					Content = post.Content,
					CreatedDate = post.CreatedDate,
					UpdatedDate = post.UpdatedDate,
				},
			};

			return await Task.FromResult(commentVM);
		}

		public async Task<CommentThreadViewModel> GetReplyViewModel(string replyId)
		{
			var comment = _commentRepository.Get(replyId);
			var post = _context.Posts.FirstOrDefault(x => x.Id == comment.PostId);

			var replyVM = new CommentThreadViewModel
			{
				HasParentComments = comment.ParentCommentId != default ? true : false,
				HasChildComments = comment.Comments.Count > 0 ? true : false,
				Id = comment.Id,
				UserId = comment.UserId,
				UserName = comment.User.UserName,
				UserProfileImage = await GetAuthorProfileImage(comment.UserId),
				Post = new PostViewModel
				{
					PostId = post.Id,
					UserId = post.UserId,
					UserName = post.User.UserName,
					UserImgUrl = await GetAuthorProfileImage(post.UserId),
					Content = post.Content,
					CreatedDate = post.CreatedDate,
					UpdatedDate = post.UpdatedDate,
				},
				Content = comment.Content,
				CreatedDate = comment.CreatedDate,
				UpdatedDate = comment.UpdatedDate,
				ParentComments = await GetParentGenerations(comment.Id, 2),
			};


			return await Task.FromResult(replyVM);
		}

		private async Task<Dictionary<int, CommentViewModel>> GetParentGenerations(string commentId, int generations)
		{
			int i = 0;
			var parentComments = new Dictionary<int, CommentViewModel>();
			var comment = _commentRepository.Get(commentId);
			var hasParent = comment.ParentCommentId != null ? true : false;
			while(i < generations && hasParent)
			{
				parentComments[i] = new CommentViewModel
				{
					Id = comment.ParentComment.Id,
					UserId = comment.ParentComment.UserId,
					UserName = comment.ParentComment.User.UserName,
					AuthorProfileImage = await GetAuthorProfileImage(comment.ParentComment.UserId),
					Content = comment.ParentComment.Content,
					CreatedDate = comment.ParentComment.CreatedDate,
					UpdatedDate = comment.ParentComment.UpdatedDate,
					HasParentComments = comment.ParentComment.ParentCommentId != default ? true : false,
					HasChildComments = true,
				};
				hasParent = parentComments[i].HasParentComments;
				if (!hasParent)
				{
					break;
				}
				comment = _commentRepository.Get(parentComments[i].Id);
				
				i++;
			}
			return await Task.FromResult(parentComments);
		}

		public async Task<IEnumerable<PostViewModel>> GenerateCommentSectionForEachPost (IEnumerable<PostViewModel> posts)
		{
			var postVMs = new List<PostViewModel>(posts);
			foreach (var postVM in postVMs)
			{
				postVM.Comments = await GenerateCommentSectionForPost(postVM.PostId);
			}
			return postVMs;
		}

		public async Task<IEnumerable<CommentViewModel>> GenerateCommentSectionForPost(string postId)
		{
			var post = _context.Posts.FirstOrDefault(x => x.Id == postId);
			var commentVMs = post.Comments
								.Where(x => x.ParentCommentId == default)
								.Select(async x => await GetViewModel(x.Id)).Select(x => x.Result);

			return await Task.FromResult(commentVMs);
		}
	}
}
