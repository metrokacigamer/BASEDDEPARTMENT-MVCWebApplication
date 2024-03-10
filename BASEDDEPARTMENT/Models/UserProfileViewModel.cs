namespace BASEDDEPARTMENT.Models
{
	public class UserProfileViewModel
	{
        public string? Id { get; set; }
        public string? ImgUrl { get; set; }
		public string? UserName { get; set; }
        public List<string>? Posts { get; set; }
    }
}
