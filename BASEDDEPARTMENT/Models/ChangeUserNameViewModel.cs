using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class ChangeUserNameViewModel
	{
		public string UserName { get; set; }

		[Required(ErrorMessage = "You must enter a new user name to submit")]
		public string NewUserName { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public string Id { get; set; }
	}
}
