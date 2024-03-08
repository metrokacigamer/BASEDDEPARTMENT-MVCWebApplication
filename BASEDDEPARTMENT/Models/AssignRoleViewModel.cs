using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
    public class AssignRoleViewModel
    {
        [Required]
        public string UserId { get; set; }
		
        [Required]
		public string Role { get; set; }

        public IEnumerable<IdentityUser> Users { get; set; }

        public IEnumerable<IdentityUserRole<string>> UserRoles { get; set; }
	}
}
