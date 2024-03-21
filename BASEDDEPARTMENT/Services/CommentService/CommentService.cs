using BASEDDEPARTMENT.EntityModels;
using BASEDDEPARTMENT.Repositories;

namespace BASEDDEPARTMENT.Services.CommentService
{
	public class CommentService: ICommentService
	{
		private readonly IRepository<Comment> _commentRepository;

		public CommentService(IRepository<Comment> commentRepository)
		{
			_commentRepository = commentRepository;
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
	}
}
