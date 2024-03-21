using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
	public class MydbContext: DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Post> Posts{ get; set; }
		public DbSet<Comment> Comments { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer(@"Server=DESKTOP-KKLNCBA;Database=TestDB0316;Trusted_Connection=True; Encrypt=False")
				.UseLazyLoadingProxies();
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Post>()
				.HasOne(p => p.User)
				.WithMany(x => x.Posts)
				.HasForeignKey(x => x.UserId)
				.IsRequired();

			modelBuilder.Entity<Comment>()
				.HasOne(x => x.User)
				.WithMany(x=> x.Comments)
				.HasForeignKey(x => x.UserId)
				.OnDelete(DeleteBehavior.ClientCascade)
				.IsRequired();

			modelBuilder.Entity<Comment>()
				.HasOne(x => x.Post)
				.WithMany(x => x.Comments)
				.HasForeignKey(x => x.PostId)
				.OnDelete(DeleteBehavior.ClientCascade)
				.IsRequired();

			modelBuilder.Entity<Comment>()
				.HasOne(x => x.ParentComment)
				.WithMany(x => x.Comments)
				.HasForeignKey(x => x.ParentCommentId)
				.OnDelete(DeleteBehavior.ClientCascade);
		}
	}
}
