using BASEDDEPARTMENT.EntityModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace BASEDDEPARTMENT
{
    public class MyDBContext: IdentityDbContext<AppUser>
	{
        public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
        public MyDBContext(DbContextOptions<MyDBContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<Comment>()
				.HasOne(x => x.User)
				.WithMany(x => x.Comments)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.ClientCascade)
				.IsRequired();

			builder.Entity<Comment>()
				.HasOne(x => x.Post)
				.WithMany(x => x.Comments)
				.HasForeignKey(x => x.PostId)
				.OnDelete(DeleteBehavior.ClientCascade)
				.IsRequired();

			builder.Entity<Comment>()
				.HasOne(x => x.ParentComment)
				.WithMany(x => x.Comments)
				.HasForeignKey(x => x.ParentCommentId)
				.OnDelete(DeleteBehavior.ClientCascade);
		}
	}
}
