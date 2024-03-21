using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class ChangeUserPassViewModel
	{
		public string UserName { get; set; }
		public string Id { get; set; }

		[Required(ErrorMessage = "You must enter your current password")]
		[DataType(DataType.Password)]
		[DisplayName("Current password")]
		public string CurrentPassword { get; set; }

		[Required(ErrorMessage = "You must enter a new password")]
		[DataType(DataType.Password)]
		[DisplayName("New password")]
		public string NewPassword { get;  set; }

		[Required(ErrorMessage = "You must confirm a new password")]
		[DataType(DataType.Password)]
		[DisplayName("Confirm new password")]
		public string ConfirmNewPassword { get; set; }

	}
}
