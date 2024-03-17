namespace BASEDDEPARTMENT.Repositories
{
	public interface IRepository<T> where T : class
	{
		void Create(T entity);
		T Get(string id);
		IEnumerable<T> GetAll();
		void Delete (T entity);
		void Update (T entity);
	}
}
