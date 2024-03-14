using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BASEDDEPARTMENT
{
	public class MyDBContext: IdentityDbContext<AppUser>
	{
        public DbSet<Post> Posts { get; set; }
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }
	}
}
