using BASEDDEPARTMENT.Entities;
using BASEDDEPARTMENT.Models;

namespace BASEDDEPARTMENT.Services.CommentService
{
    public interface ICommentService
    {
		Task<bool> Update(Comment comment);
		Task<Comment> Get(string id);
		bool Create(Comment comment);
		Task<bool> Delete(string commentId);
		Task<CommentViewModel> GetViewModel(string commentId);
		Task<CommentThreadViewModel> GetReplyViewModel(string replyId);
		Task<IEnumerable<PostViewModel>> GenerateCommentSectionForEachPost(IEnumerable<PostViewModel> posts);
		Task<IEnumerable<CommentViewModel>> GenerateCommentSectionForPost(string postId);
	}
}