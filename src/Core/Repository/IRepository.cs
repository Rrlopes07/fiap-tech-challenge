namespace Core.Repository;

public interface IRepository<T> where T : EntityBase
{
	Task<IList<T>> FindAll();
	Task<T?> FindById(Guid id);
	void Add(T entity);
	void Update(T entity);
	void Delete(T entity);
}
