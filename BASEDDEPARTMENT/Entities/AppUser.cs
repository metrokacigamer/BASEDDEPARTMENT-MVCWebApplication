using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BASEDDEPARTMENT.Entities
{
    public class AppUser : IdentityUser
    {
        public virtual ICollection<Post> Posts { get; set; }
		public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Image> Images { get; set; }
	}
}
