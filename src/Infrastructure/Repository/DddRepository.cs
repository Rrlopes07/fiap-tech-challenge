namespace Infrastructure.Repository;

public class DddRepository : EFRepository<Ddd>, IDddRepository
{
    public DddRepository(ApplicationDbContext context) : base(context) { }
}
