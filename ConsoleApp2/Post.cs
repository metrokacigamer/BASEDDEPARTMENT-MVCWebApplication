namespace ConsoleApp2
{
	public class Post
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public virtual User User { get; set; }
        public int UserId { get; set; }
        public virtual IEnumerable<Comment> Comments { get; set; }
    }
}