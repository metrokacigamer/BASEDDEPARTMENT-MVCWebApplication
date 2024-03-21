using BASEDDEPARTMENT.Entities;
using Microsoft.EntityFrameworkCore;

namespace BASEDDEPARTMENT.Repositories
{
	public class CommentRepository : IRepository<Comment>
	{
		private readonly MyDBContext _context;
		private readonly DbSet<Comment> _dbSet;

		public CommentRepository(MyDBContext context)
		{
			_context = context;
			_dbSet = _context.Set<Comment>();
		}

		public void Create(Comment entity)
		{
			_dbSet.Add(entity);
			_context.SaveChanges();
		}

		public void Delete(Comment entity)
		{
			var _entity = _dbSet.Include(x => x.Comments)
					.FirstOrDefault(e => e.Id == entity.Id);
			_dbSet.Remove(_entity!);
			_context.SaveChanges();
		}

		public Comment Get(string id)
		{
			return _dbSet.Find(id)!;
		}

		public IEnumerable<Comment> GetAll()
		{
			return _dbSet.ToList();
		}

		public void Update(Comment entity)
		{
			_dbSet.Update(entity);
			_context.SaveChanges();
		}
	}
}
