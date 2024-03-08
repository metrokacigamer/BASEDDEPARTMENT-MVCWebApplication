using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class RegisterViewModel
	{
		[Required]
		public string UserName { get; set; }

		[Required]
		[DataType(DataType.Password)]
		public string Password { get; set; }
	}
}
