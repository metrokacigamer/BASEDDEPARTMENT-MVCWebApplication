using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
    public class CreateRoleViewModel
    {
        [Required]
		[StringLength(30, MinimumLength = 1, ErrorMessage = "Role Name Length should be anywhere between 1 and 30")]
		public string RoleName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
