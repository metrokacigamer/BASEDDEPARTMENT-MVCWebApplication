using BASEDDEPARTMENT.EntityModels;
using BASEDDEPARTMENT.Models;
using BASEDDEPARTMENT.Repositories;

namespace BASEDDEPARTMENT.Services.PostService
{
    public class PostService: IPostService
	{
		private readonly IRepository<Post> _postRepository;
		private readonly MyDBContext _context;

		public PostService(IRepository<Post> postRepository, MyDBContext context)
		{
			_postRepository = postRepository;
			_context = context;
		}

		public async Task Create(Post entity)
		{
			_postRepository.Create(entity);
			await Task.CompletedTask;
		}

		public async Task Delete(Post entity)
		{
			_postRepository.Delete(entity);
			await Task.CompletedTask;
		}

		public async Task<Post> Get(string id)
		{
			return await Task.FromResult(_postRepository.Get(id));
		}

		public async Task<IEnumerable<Post>> GetAll()
		{
			return await Task.FromResult(_postRepository.GetAll());
		}

		public async Task Update(Post entity)
		{
			_postRepository.Update(entity);
			await Task.CompletedTask;
		}

		public async Task<PostViewModel> GetPostViewModel(string postId)
		{
			var post = _postRepository.Get(postId);
			var postVM = await Task.FromResult(new PostViewModel
			{
				UserId = post.UserId,
				UserName = post.User.UserName,
				UserImgUrl = post.User.ImgUrl,
				Content = post.Content,
				CreatedDate = post.CreatedDate,
				UpdatedDate = post.UpdatedDate,
				PostId = post.Id,
				Comments = post.Comments.Where(d => d.ParentComment == null)
								.Select(c => new CommentViewModel //need to add Take(N) and Replies
								{
									UserName = _context.Users.FirstOrDefault(x => x.Id == c.UserId).UserName,
									UserId = c.UserId,
									Id = c.Id,
									Content = c.Content,
									AuthorProfileImage = _context.Users.FirstOrDefault(x => x.Id == c.UserId).ImgUrl,
									CreatedDate = c.CreatedDate,
									UpdatedDate = c.UpdatedDate,
									Replies = c.Comments.Select(w => new CommentViewModel
									{
										UserName = _context.Users.FirstOrDefault(x => x.Id == w.UserId).UserName,
										UserId = w.UserId,
										Id = w.Id,
										Content = w.Content,
										AuthorProfileImage = _context.Users.FirstOrDefault(x => x.Id == w.UserId).ImgUrl,
										CreatedDate = w.CreatedDate,
										UpdatedDate = w.UpdatedDate,
									}).OrderByDescending(x => x.CreatedDate),
								})
								.OrderByDescending(x => x.CreatedDate),
			});
			return postVM;
		}
	}
}
