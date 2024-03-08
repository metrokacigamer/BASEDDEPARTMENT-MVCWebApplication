using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BASEDDEPARTMENT
{
	public class MyDBContext: IdentityDbContext
	{
		public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }
	}
}
