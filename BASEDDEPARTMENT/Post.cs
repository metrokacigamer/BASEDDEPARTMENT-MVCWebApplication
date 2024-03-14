using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT
{
	public class Post
	{
        [Required]
        public string? Content { get; set; }
        public string? Id { get; set; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime? CreatedDate { get; init; }

		[Required]
		[DataType(DataType.DateTime)]
		public DateTime? UpdatedDate { get; set; }

		[Required]
        public virtual AppUser? User { get; init; }
        public string? UserId { get; set; }
    }
}
