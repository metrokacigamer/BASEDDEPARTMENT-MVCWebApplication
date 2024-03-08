using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
    public class CreateRoleViewModel
    {
        [Required]
        public string RoleName { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
