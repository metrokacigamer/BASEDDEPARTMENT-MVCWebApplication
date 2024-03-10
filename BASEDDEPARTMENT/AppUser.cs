using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT
{
	public class AppUser: IdentityUser
	{
		[DataType(DataType.ImageUrl)]
        public string? ImgUrl { get; set; }
    }
}
