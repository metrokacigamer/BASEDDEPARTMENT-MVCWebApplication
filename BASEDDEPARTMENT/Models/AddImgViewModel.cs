using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Models
{
    public class AddImgViewModel
    {
        [Required]
        [DataType(DataType.ImageUrl)]
        [DisplayName("Upload Image URL here")]
        public string ImgUrl { get; set; }
    }
}
