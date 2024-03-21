using BASEDDEPARTMENT.Entities;
using BASEDDEPARTMENT.Models;
using BASEDDEPARTMENT.Repositories;
using Microsoft.Extensions.Hosting;

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

		public async Task<string> GetPostAuthorProfileImage(string postId)
		{
			var post = await Get(postId);
			return post.User.Images.FirstOrDefault(x => x.ImageType == Enums.ImageType.ProfileImage)?.ImgUrl;
		}

		public async Task<PostViewModel> GetPostViewModel(string postId)
		{
			var post = _postRepository.Get(postId);
			var postVM = await Task.FromResult(new PostViewModel
			{
				UserId = post.UserId,
				UserName = post.User.UserName,
				UserImgUrl = await GetPostAuthorProfileImage(postId),
				Content = post.Content,
				CreatedDate = post.CreatedDate,
				UpdatedDate = post.UpdatedDate,
				PostId = post.Id,
			});
			return postVM;
		}
	}
}
