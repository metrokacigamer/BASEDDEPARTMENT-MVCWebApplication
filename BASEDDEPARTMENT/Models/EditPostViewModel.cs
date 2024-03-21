using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
    public class EditPostViewModel
    {
        public string PostId { get; set; }

        [Required]
        public string Content { get; set; }
    }
}