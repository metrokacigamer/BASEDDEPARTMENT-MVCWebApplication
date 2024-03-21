using BASEDDEPARTMENT.Entities;
using BASEDDEPARTMENT.Models;

namespace BASEDDEPARTMENT.Services.PostService
{
    public interface IPostService
    {
        Task Create(Post entity);
        Task<Post> Get(string id);
        Task<IEnumerable<Post>> GetAll();
        Task Delete(Post entity);
        Task Update(Post entity);
        Task<PostViewModel> GetPostViewModel(string postId);
	}
}
