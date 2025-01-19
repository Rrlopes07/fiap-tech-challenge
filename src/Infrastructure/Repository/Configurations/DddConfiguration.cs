namespace Infrastructure.Repository.Configurations;

public class DddConfiguration : IEntityTypeConfiguration<Ddd>
{
	public void Configure(EntityTypeBuilder<Ddd> builder)
	{
		builder.ToTable("Ddd");
		builder.HasKey(e => e.Id);
		builder.Property(p => p.Id)
			.HasColumnType("uuid")
			.ValueGeneratedNever()
			.HasDefaultValueSql("gen_random_uuid()");
		builder.Property(p => p.Region)
			.HasColumnName("Name")
			.HasColumnType("varchar(100)")
			.IsRequired();
		builder.Property(p => p.DddNumber)
			.HasColumnType("smallint")
			.IsRequired();
	}
}
