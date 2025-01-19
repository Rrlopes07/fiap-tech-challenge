#nullable disable

namespace Infrastructure.Migrations;

[DbContext(typeof(ApplicationDbContext))]
partial class ApplicationDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
#pragma warning disable 612, 618
        modelBuilder
            .HasAnnotation("ProductVersion", "8.0.0")
            .HasAnnotation("Relational:MaxIdentifierLength", 63);

        NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

        modelBuilder.Entity("Core.Entity.Client", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("gen_random_uuid()");

                b.Property<Guid>("DddId")
                    .HasColumnType("uuid");

                b.Property<string>("Email")
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                b.Property<string>("Telephone")
                    .IsRequired()
                    .HasColumnType("varchar(100)");

                b.HasKey("Id");

                b.HasIndex("DddId");

                b.ToTable("Client", (string)null);
            });

        modelBuilder.Entity("Core.Entity.Ddd", b =>
            {
                b.Property<Guid>("Id")
                    .HasColumnType("uuid")
                    .HasDefaultValueSql("gen_random_uuid()");

                b.Property<short>("DddNumber")
                    .HasColumnType("smallint");

                b.Property<string>("Region")
                    .IsRequired()
                    .HasColumnType("varchar(100)")
                    .HasColumnName("Name");

                b.HasKey("Id");

                b.ToTable("Ddd", (string)null);
            });

        modelBuilder.Entity("Core.Entity.Client", b =>
            {
                b.HasOne("Core.Entity.Ddd", "Ddd")
                    .WithMany("Clients")
                    .HasForeignKey("DddId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();

                b.Navigation("Ddd");
            });

        modelBuilder.Entity("Core.Entity.Ddd", b =>
            {
                b.Navigation("Clients");
            });
#pragma warning restore 612, 618
    }
}
