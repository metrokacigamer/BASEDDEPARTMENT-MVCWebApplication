namespace BASEDDEPARTMENT.Models
{
	public class ImageViewModel
	{
        public string ImageId { get; set; }
		public string UserId { get; set; }
		public string UserName { get; set; }
		public DateTime UploadedDate { get; set; }
        public string ImagePath { get; set; }
		public string ProfileImagePath { get; set; }
    }
}
