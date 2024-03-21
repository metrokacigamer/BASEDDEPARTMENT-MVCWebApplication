using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class PostViewModel
	{
		public string UserId {  get; set; }
		public string UserName { get; set; }
		public string PostId { get; set; }
        [Required]
		public string Content { get; set; }
		[Required]
		[DataType(DataType.DateTime)]
		public DateTime? CreatedDate { get; init; }
		public string UserImgUrl { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime? UpdatedDate { get; set; }

		public IEnumerable<CommentViewModel> Comments { get; set; }
		public IEnumerable<ImageViewModel> Images { get; set; }
	}
}