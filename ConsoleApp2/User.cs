namespace ConsoleApp2
{
	public class User
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public virtual IEnumerable<Post> Posts{ get; set; }
		public virtual IEnumerable<Comment> Comments { get; set; }
	}
}