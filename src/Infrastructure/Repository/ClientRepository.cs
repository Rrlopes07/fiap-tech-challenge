namespace Infrastructure.Repository;

public class ClientRepository : EFRepository<Client>, IClientRepository
{
    public ClientRepository(ApplicationDbContext context) : base(context) { }
}
