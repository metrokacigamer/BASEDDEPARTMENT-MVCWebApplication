using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class RegisterViewModel
	{
		[Required]
		[StringLength(20, MinimumLength =5, ErrorMessage = "Username length should be anywhere between 5 and 20")]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
