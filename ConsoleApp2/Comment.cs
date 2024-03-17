namespace ConsoleApp2
{
	public class Comment
	{
		public int Id { get; set; }
		public string Content { get; set; }
		public virtual User User { get; set; }
		public int UserId { get; set; }
		public virtual Post Post { get; set; }
        public int PostId { get; set; }
		public virtual IEnumerable<Comment> Comments { get; set; }
		public virtual Comment ParentComment { get; set; }
		public int? ParentCommentId { get; set; }
    }
}