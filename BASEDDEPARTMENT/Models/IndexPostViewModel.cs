using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class IndexPostViewModel
	{
		public string PostId { get; set; }
		public string UserId { get; set; }
		[Required]
		public string Content { get; set; }
		[Required]
		public string UserName { get; set; }
		[Required]
        public string UserImgUrl { get; set; }
		[Required]
		[DataType(DataType.DateTime)]
		public DateTime CreatedDate { get; init; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime UpdatedDate { get; set; }

	}
}
