using BASEDDEPARTMENT.EntityModels;

namespace BASEDDEPARTMENT.Services.CommentService
{
    public interface ICommentService
    {
		Task<bool> Update(Comment comment);
		Task<Comment> Get(string id);
		bool Create(Comment comment);
		Task<bool> Delete(string commentId);
	}
}