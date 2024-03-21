using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class CommentViewModel
	{
		public string Id { get; set; }
		[Required]
		public string UserId { get; set; }
		public string UserName { get; set; }
		[Required]
		public string Content { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime? CreatedDate { get; init; }

		[DataType(DataType.DateTime)]
		public DateTime? UpdatedDate { get; set; }

		[Required]
		public string AuthorProfileImage { get; set; }
		public string ParentCommentId { get; set; }
		public IEnumerable<CommentViewModel> Replies { get; init; }
		public bool HasChildComments { get; set; }
		public bool HasParentComments { get; set; }
		public PostViewModel Post { get; set; }
	}
}