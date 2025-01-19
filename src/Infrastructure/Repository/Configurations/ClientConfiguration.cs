namespace Infrastructure.Repository.Configurations;

public class ClientConfiguration : IEntityTypeConfiguration<Client>
{
	public void Configure(EntityTypeBuilder<Client> builder)
	{
		builder.ToTable("Client");
		builder.HasKey(e => e.Id);
		builder.Property(p => p.Id)
			.HasColumnType("uuid")
			.ValueGeneratedNever()
			.HasDefaultValueSql("gen_random_uuid()");
		builder.Property(p => p.Name)
			.HasColumnType("varchar(100)")
			.IsRequired();
		builder.Property(p => p.Email)
			.HasColumnType("varchar(100)")
			.IsRequired();
		builder.Property(p => p.Telephone)
			.HasColumnType("varchar(100)")
			.IsRequired();
		builder.Property(p => p.DddId)
			.HasColumnType("uuid")
			.IsRequired();

		builder.HasOne(c => c.Ddd)
			.WithMany(d => d.Clients)
			.HasPrincipalKey(d => d.Id);
	}
}
