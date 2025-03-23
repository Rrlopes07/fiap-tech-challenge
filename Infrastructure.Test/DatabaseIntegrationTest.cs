using Core.Entity;
using FluentAssertions;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;

namespace Infrastructure.Test;

public class DatabaseIntegrationTest : IAsyncLifetime
{
	private PostgreSqlContainer _container;
	private ApplicationDbContext _context;

    public DatabaseIntegrationTest()
    {
        _container = new PostgreSqlBuilder()
            .WithImage("postgres:latest")
            .WithDatabase("testdb")
            .WithUsername("testuser")
            .WithCleanUp(true)
            .Build();
    }

    public async Task InitializeAsync()
    {
        await _container.StartAsync();

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseNpgsql(_container.GetConnectionString())
            .Options;

        _context = new ApplicationDbContext(options);
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _container.DisposeAsync();
    }

    [Fact]
    public async Task ShouldAddAndFindDddById()
    {
        var repository = new DddRepository(_context);
        var ddd = GenerateDdd();

        repository.Add(ddd);
        var dddDb = repository.FindById(ddd.Id).Result;

        dddDb.Should().NotBeNull();
        dddDb!.Region.Should().Be(ddd.Region);
        dddDb!.DddNumber.Should().Be(ddd.DddNumber);
    }

	[Fact]
	public async Task ShouldUpdateDdd()
	{
        var repository = new DddRepository(_context);
		Random random = new();
		var ddd = GenerateDdd();
        Ddd secondDdd = new()
        {
            Region = random.Next(1000000, 10000000).ToString(),
            DddNumber = random.Next(10, 99)
        };

		repository.Add(ddd);

        ddd.Region = secondDdd.Region;
        ddd.DddNumber = secondDdd.DddNumber;

        repository.Update(ddd);

		var dddDb = repository.FindById(ddd.Id).Result;

		dddDb.Should().NotBeNull();
		dddDb!.Region.Should().Be(secondDdd.Region);
		dddDb!.DddNumber.Should().Be(secondDdd.DddNumber);
	}

    [Fact]
	public async Task ShouldListAllDdd()
	{
		var repository = new DddRepository(_context);
		var ddd = GenerateDdd();
        var secondDdd = GenerateDdd();

		repository.Add(ddd);
        repository.Add(secondDdd);

		var dddDb = repository.FindAll().Result;

		dddDb.Should().NotBeNull();
        dddDb.Should().HaveCount(2);
        dddDb.Should().Contain(p => p.Region.Equals(ddd.Region) && p.DddNumber.Equals(ddd.DddNumber));
		dddDb.Should().Contain(p => p.Region.Equals(secondDdd.Region) && p.DddNumber.Equals(secondDdd.DddNumber));
	}

    [Fact]
    public async Task ShouldExcludeDdd()
    {
		var repository = new DddRepository(_context);
		var ddd = GenerateDdd();

		repository.Add(ddd);
		repository.Delete(ddd);

		var dddDb = repository.FindAll().Result;

		dddDb.Should().HaveCount(0);
	}

    [Fact]
    public async Task ShouldInsertClientCorrelatedWithDdd()
    {
		var dddRepository = new DddRepository(_context);
		var clientRepository = new ClientRepository(_context);
		var ddd = GenerateDdd();
		var client = GenerateClient();
		client.DddId = ddd.Id;
		client.Ddd = ddd;

		dddRepository.Add(ddd);
		clientRepository.Add(client);

		var clientDb = clientRepository.FindById(client.Id).Result;

		clientDb.Should().NotBeNull();
		clientDb!.Name.Should().Be(client.Name);
		clientDb!.Telephone.Should().Be(client.Telephone);
		clientDb!.Email.Should().Be(client.Email);
		clientDb!.DddId.Should().Be(ddd.Id);
	}

	public static Ddd GenerateDdd()
	{
		Random random = new();

		return new()
		{
			Id = Guid.NewGuid(),
			Region = random.Next(1000000, 10000000).ToString(),
			DddNumber = random.Next(10, 99)
		};
	}

	private static Client GenerateClient()
	{
		Random random = new();
		var dddId = Guid.NewGuid();

		return new()
		{
			Id = Guid.NewGuid(),
			Name = random.Next(1000000, 10000000).ToString(),
			Telephone = random.Next(1000000000, 1000009999).ToString(),
			Email = $"{random.Next(1000000, 10000000)}@email.com",
			DddId = dddId,
			Ddd = new()
			{
				Id = dddId,
				Region = random.Next(1000000, 10000000).ToString(),
				DddNumber = random.Next(10, 99)
			}
		};
	}
}
