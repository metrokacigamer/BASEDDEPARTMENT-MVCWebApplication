using BASEDDEPARTMENT.Enums;
using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Entities
{
	public class Image
	{
        [Required]
        public string Id { get; set; }
		[DataType(DataType.ImageUrl)]
		[Required]
        public string ImgUrl { get; set; }
        [Required]
        public string UserId { get; set; }
        public virtual AppUser User { get; set; }
        public string PostId { get; set; }
        public virtual Post Post { get; set; }
        public string CommentId { get; set; }
        public virtual Comment Comment { get; set; }
        public ImageType ImageType { get; set; }
        [Required]
        public DateTime UploadDate { get; set; }
    }
}
