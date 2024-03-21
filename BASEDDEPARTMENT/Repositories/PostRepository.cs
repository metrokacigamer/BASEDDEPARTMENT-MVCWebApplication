
using BASEDDEPARTMENT.Entities;
using Microsoft.EntityFrameworkCore;

namespace BASEDDEPARTMENT.Repositories
{
	public class PostRepository : IRepository<Post>
	{
		private readonly MyDBContext _context;
		private readonly DbSet<Post> _dbSet;
		public PostRepository(MyDBContext context) 
		{
			_context = context;
			_dbSet = _context.Set<Post>();
		}

		public void Create(Post entity)
		{
			_dbSet.Add(entity);
			_context.SaveChanges();
		}

		public void Delete(Post entity)
		{
			var _entity = _dbSet.Include(x => x.Comments)
								.FirstOrDefault(e => e.Id == entity.Id);

			_dbSet.Remove(_entity!);
			_context.SaveChanges();
		}

		public Post Get(string id)
		{
			return _dbSet.Find(id)!;
		}

		public IEnumerable<Post> GetAll()
		{
			return _dbSet.ToList();
		}

		public void Update(Post entity)
		{
			_dbSet.Update(entity);
			_context.SaveChanges();
		}
	}
}
