using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class ChangeUserNameViewModel
	{
		public string UserName { get; set; }

		[Required(ErrorMessage = "You must enter a new user name to submit")]
		[StringLength(20, MinimumLength = 5, ErrorMessage = "New username length should be anywhere between 5 and 20")]
		public string NewUserName { get; set; }

		[Required(ErrorMessage = "Password is required")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
		public string Id { get; set; }
	}
}
