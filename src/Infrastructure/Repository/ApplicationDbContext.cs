namespace Infrastructure.Repository;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;

	public ApplicationDbContext() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

	public ApplicationDbContext(IConfiguration configuration)
	    => _configuration = configuration;

	public DbSet<Client> Clients { get; set; }
    public DbSet<Ddd> Ddds { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseNpgsql(_configuration.GetConnectionString("ConnectionString"));
    }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
        => modelBuilder
            .ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
}
