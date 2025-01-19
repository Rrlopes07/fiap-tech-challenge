namespace Infrastructure.Repository;

public class EFRepository<T>(ApplicationDbContext context) : IRepository<T> where T : EntityBase
{
	protected ApplicationDbContext _context = context;
	protected DbSet<T> _dbSet = context.Set<T>();

	public void Add(T entity)
	{
		_dbSet.AddAsync(entity);
		_context.SaveChanges();
	}

	public void Delete(T entity)
	{
		_dbSet.Remove(entity);
		_context.SaveChanges();
	}

	public async Task<IList<T>> FindAll()
		=> await _dbSet.ToListAsync();

	public async Task<T?> FindById(Guid id)
		=> await _dbSet.FirstOrDefaultAsync(e => e.Id.Equals(id));

	public void Update(T entity)
	{
		_dbSet.Update(entity);
		_context.SaveChanges();
	}
}
