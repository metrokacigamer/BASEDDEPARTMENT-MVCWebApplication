using BASEDDEPARTMENT.Entities;
using Microsoft.EntityFrameworkCore;

namespace BASEDDEPARTMENT.Repositories
{
	public class ImageRepository: IRepository<Image>
	{
		private readonly MyDBContext _context;
		private readonly DbSet<Image> _dbSet;

		public ImageRepository(MyDBContext context)
		{
			_context = context;
			_dbSet = _context.Set<Image>();
		}

		public void Create(Image entity)
		{
			_dbSet.Add(entity);
			_context.SaveChanges();
		}

		public void Delete(Image entity)
		{
			_dbSet.Remove(entity);
			_context.SaveChanges();
		}

		public Image Get(string id)
		{
			return _dbSet.Find(id)!;
		}

		public IEnumerable<Image> GetAll()
		{
			return _dbSet.ToList();
		}

		public void Update(Image entity)
		{
			_dbSet.Update(entity);
			_context.SaveChanges();
		}
	}
}
