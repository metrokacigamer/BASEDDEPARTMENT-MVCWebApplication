using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Entities
{
	public class Comment
	{
		[Required]
		public string Id { get; set; }
		public string Content { get; set; }

		[DataType(DataType.DateTime)]
		public DateTime CreatedDate { get; init; }

		[DataType(DataType.DateTime)]
		public DateTime UpdatedDate { get; set; }
		public virtual AppUser User { get; init; }
		public string UserId { get; set; }
		public string PostId { get; set; }
		public virtual Post Post { get; set; }
		public virtual Comment ParentComment { get; set; }
		public string? ParentCommentId { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
