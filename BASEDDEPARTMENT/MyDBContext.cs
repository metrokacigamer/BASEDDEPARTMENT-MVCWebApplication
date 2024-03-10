using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BASEDDEPARTMENT
{
	public class MyDBContext: IdentityDbContext<AppUser>
	{
		public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }
	}
}
