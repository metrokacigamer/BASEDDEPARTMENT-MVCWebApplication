using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace BASEDDEPARTMENT.Models
{
	public class ChangeUserEmailViewModel
	{
		public string UserName { get; set; }
		public string Id { get; set; }
		public string Email { get; set; }

		[Required]
		[EmailAddress]
		[DataType(DataType.EmailAddress)]
		[DisplayName("Enter new Email address")]
		public string NewEmail { get; set; }

		[Required]
		[DisplayName("Enter your password")]
		[DataType(DataType.Password)]
		public string Password { get; set; }
    }
}
