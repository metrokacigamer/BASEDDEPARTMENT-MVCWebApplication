using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Entities
{
    public class Post
    {
        public string Content { get; set; }
        [Required]
        public string Id { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; init; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public string UserId { get; set; }
        [Required]
        public virtual AppUser User { get; init; }
		public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
