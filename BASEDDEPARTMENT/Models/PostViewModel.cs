using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class PostViewModel
	{
		public string? PostId { get; set; }
		[Required]
		public string? Content { get; set; }
		[Required]
		[DataType(DataType.DateTime)]
		public DateTime? CreatedDate { get; init; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime? UpdatedDate { get; set; }
	}
}