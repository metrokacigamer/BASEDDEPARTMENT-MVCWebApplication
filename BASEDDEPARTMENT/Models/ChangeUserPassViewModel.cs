using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class ChangeUserPassViewModel
	{
		public string UserName { get; set; }
		public string Id { get; set; }

		[Required(ErrorMessage = "You must enter your current password")]
		[DataType(DataType.Password)]
		public string CurrentPassword { get; set; }

		[Required(ErrorMessage = "You must enter a new password")]
		[DataType(DataType.Password)]
		public string NewPassword { get;  set; }

		[Required(ErrorMessage = "You must confirm a new password")]
		[DataType(DataType.Password)]
		public string ConfirmNewPassword { get; set; }

	}
}
