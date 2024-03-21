using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class CommentThreadViewModel
	{
        public string Id { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserProfileImage { get; set; }
        public Dictionary<int, CommentViewModel> ParentComments {  get; set; }
        public PostViewModel Post { get; set; }
        public string Content { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime? CreatedDate { get; init; }
		[DataType(DataType.DateTime)]
		public DateTime? UpdatedDate { get; set; }
		public ImageViewModel Images { get; set; }
		public bool HasChildComments { get; set; }
		public bool HasParentComments { get; set; }
	}
}
