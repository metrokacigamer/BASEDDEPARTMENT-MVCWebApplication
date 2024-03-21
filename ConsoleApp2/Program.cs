using Microsoft.EntityFrameworkCore;

namespace ConsoleApp2
{
	internal class Program
	{
		static void Main(string[] args)
		{
			using var _context = new MydbContext();
			//var user = new User
			//{
			//	Name = "Test",
			//};

			//_context.Users.Add(user);
			//_context.SaveChanges();

			//var post = new Post
			//{
			//	User = user,
			//	Content = "Test Post",
			//};

			//_context.Posts.Add(post);
			//_context.SaveChanges();

			//var comment = new Comment
			//{
			//	Content = "Test Comment",
			//	User = user,
			//	Post = post
			//};

			//_context.Comments.Add(comment);
			//_context.SaveChanges();

			//var reply = new Comment
			//{
			//	Content = "Test Reply",
			//	User = user,
			//	Post = post,
			//	ParentComment = comment,
			//};

			//_context.Comments.Add(reply);
			//_context.SaveChanges();

			var userFromDB = _context.Users
								.Include(x => x.Posts)
								.Include(x => x.Comments)
								.FirstOrDefault();

			_context.Users.Remove(userFromDB!);
			_context.SaveChanges();

		}
	}
}
