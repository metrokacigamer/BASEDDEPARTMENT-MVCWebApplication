using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
	public class AttachUserEmailViewModel
	{
		public string UserName { get; set; }
		public string Id { get; set; }

		[Required]
		[EmailAddress]
		[DataType(DataType.EmailAddress)]
		[DisplayName("Enter your Email")]
		public string Email { get; set; }

		[Required]
		[DisplayName("Enter your password")]
		[DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
