namespace BASEDDEPARTMENT.Models
{
	public class UserProfileViewModel
	{
        public string Id { get; set; }
        public string ImgUrl { get; set; }
		public string UserName { get; set; }
        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}
