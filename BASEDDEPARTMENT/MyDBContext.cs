using BASEDDEPARTMENT.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace BASEDDEPARTMENT
{
    public class MyDBContext: IdentityDbContext<AppUser>
	{
        public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Image> Images { get; set; }
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

			builder.Entity<Image>()
				.HasOne(x => x.User)
				.WithMany(x => x.Images)
				.HasForeignKey(image => image.UserId)
				.OnDelete(DeleteBehavior.ClientCascade)
				.IsRequired();

			builder.Entity<Image>()
				.HasOne(x => x.Post)
				.WithMany(x => x.Images)
				.HasForeignKey(image => image.PostId)
				.OnDelete(DeleteBehavior.ClientCascade)
				.IsRequired(false);

			builder.Entity<Image>()
				.HasOne(x => x.Comment)
				.WithMany(x => x.Images)
				.HasForeignKey(image => image.CommentId)
				.OnDelete(DeleteBehavior.ClientCascade)
				.IsRequired(false);

			builder.Entity<Image>()
				.HasIndex(image => new { image.UserId, image.PostId })
				.HasFilter("[PostId] IS NOT NULL AND [CommentId] IS NOT NULL");

			builder.Entity<Image>()
				.HasIndex(image => new { image.UserId, image.CommentId })
				.HasFilter("[CommentId] IS NOT NULL AND [PostId] IS NOT NULL");

		}
	}
}
