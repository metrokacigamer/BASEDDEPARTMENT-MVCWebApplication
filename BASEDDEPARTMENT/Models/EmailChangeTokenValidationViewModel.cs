using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class EmailChangeTokenValidationViewModel
	{
		public string Id { get; set; }
		public string Email { get; set; }
		public string TempToken { get; set; }

		[Required]
		[DisplayName("Copy your Token here: ")]
		public string Token { get; set; }

	}
}
