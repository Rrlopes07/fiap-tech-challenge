namespace ContactsApi.Test;

public class EndpointClientTests
{
	private readonly Mock<IClientRepository> _mockRepo;
	private readonly Mock<IDddRepository> _mockDdd;
	private readonly Mock<ICacheService> _mockCache;

    public EndpointClientTests()
    {
        _mockCache = new Mock<ICacheService>();
		_mockDdd = new Mock<IDddRepository>();
		_mockRepo = new Mock<IClientRepository>();
    }

    [Fact]
	public async Task WhenThereIsNoCachedInfoGetAllShouldReturnListWithResponses()
	{
		var firstClient = GenerateClient();

		_mockRepo.Setup(m => m.FindAll())
			.ReturnsAsync([firstClient]);

		_mockCache.Setup(m => m.Get(It.Is<Guid>(id => id == firstClient.DddId).ToString()))
			.Returns(null);

		var result = await ClientEndpoints.GetAllClients(_mockCache.Object, _mockRepo.Object, null);

		Assert.IsType<Ok<List<ClientResponse>>>(result);

		Assert.NotNull(result.Value);
		Assert.NotEmpty(result.Value);

		Assert.Collection(result.Value, client1 =>
		{
			Assert.Equal(firstClient.Id, client1.Id);
			Assert.Equal(firstClient.Name, client1.Name);
			Assert.Equal(firstClient.Email, client1.Email);
			Assert.Equal(firstClient.Telephone, client1.Telephone);
			Assert.Equal(firstClient.DddId, client1.DddId);
		});
	}

	[Fact]
	public async Task WhenThereCachedInfoGetAllShouldReturnListWithResponses()
	{
		var firstClient = GenerateClient();
		var response = new ClientResponse(firstClient.Id, firstClient.Name, firstClient.Telephone, firstClient.Email, firstClient.DddId);

		_mockRepo.Setup(m => m.FindAll()).ReturnsAsync([ firstClient, firstClient ]);

		_mockCache.Setup(m => m.Get("clients-"))
			.Returns(new List<ClientResponse> { response });

		var result = await ClientEndpoints.GetAllClients(_mockCache.Object, _mockRepo.Object, null);

		Assert.IsType<Ok<List<ClientResponse>>>(result);

		Assert.NotNull(result.Value);
		Assert.NotEmpty(result.Value);

		Assert.Collection(result.Value, client1 =>
		{
			Assert.Equal(firstClient.Id, client1.Id);
			Assert.Equal(firstClient.Name, client1.Name);
			Assert.Equal(firstClient.Email, client1.Email);
			Assert.Equal(firstClient.Telephone, client1.Telephone);
			Assert.Equal(firstClient.DddId, client1.DddId);
		});
	}

	[Fact]
	public async Task GetClientShouldReturnNotFoundIfNotExists()
	{
		Guid idToSearch = Guid.NewGuid();

		_mockRepo.Setup(m => m.FindById(It.Is<Guid>(id => id == idToSearch)))
			.ReturnsAsync((Client?)null);

		var result = await ClientEndpoints.FindById(_mockRepo.Object, idToSearch);

		Assert.IsType<Results<Ok<ClientResponse>, NotFound<string>>>(result);

		var notFoundResult = (NotFound<string>)result.Result;

		Assert.NotNull(notFoundResult);
	}

	[Fact]
	public void CreateClientGeneratesNewClient()
	{
		var newClient = GenerateClient();
		var clientRequest = new ClientRequest(newClient.Name, newClient.Telephone, newClient.Email, newClient.DddId);
		var clients = new List<Client>();

		_mockDdd.Setup(m => m.FindById(It.Is<Guid>(id => id == newClient.DddId)))
			.ReturnsAsync(newClient.Ddd);

		_mockRepo.Setup(m => m.Add(It.Is<Client>(c => c.Name == newClient.Name && c.Telephone == newClient.Telephone && c.Email == newClient.Email && c.DddId == newClient.DddId)))
			.Callback<Client>(clients.Add);

		_mockCache.Setup(m => m.Delete(It.Is<Guid>(id => id == newClient.DddId).ToString()));

		var result = ClientEndpoints.AddClient(_mockCache.Object, _mockRepo.Object, _mockDdd.Object, clientRequest);

		Assert.IsType<Created>(result.Result);
		Assert.NotEmpty(clients);

		Assert.Collection(clients, client =>
		{
			Assert.Equal(newClient.Name, client.Name);
			Assert.Equal(newClient.Telephone, client.Telephone);
			Assert.Equal(newClient.Email, client.Email);
			Assert.Equal(newClient.DddId, client.DddId);
		});
	}

	[Fact]
	public async Task UpdateClientShouldUpdateClientInDatabase()
	{
		var client = GenerateClient();

		var updatedClient = new ClientRequestEdit(client.Id, "Roberto", "456123456", "roberto@email.com");

		_mockRepo.Setup(m => m.FindById(It.Is<Guid>(id => id == client.Id)))
			.ReturnsAsync(client);

		_mockRepo.Setup(m => m.Update(It.Is<Client>(c => c.Name == updatedClient.Name && c.Telephone == updatedClient.Telephone && c.Email == updatedClient.Email)));

		_mockCache.Setup(m => m.Delete(It.Is<Guid>(id => id == client.DddId).ToString()));

		var result = await ClientEndpoints.UpdateClient(_mockCache.Object, _mockRepo.Object, updatedClient);
		Assert.IsType<Ok>(result);
	}

	[Fact]
	public async Task DeleteClientShouldDeleteInDatabase()
	{
		var client = GenerateClient();

		_mockRepo.Setup(m => m.FindById(It.Is<Guid>(id => id == client.Id)))
			.ReturnsAsync(client);

		_mockRepo.Setup(m => m.Delete(It.Is<Client>(d => d.Id == client.Id)));

		_mockCache.Setup(m => m.Delete(It.Is<Guid>(id => id == client.Id).ToString()));

		var result = await ClientEndpoints.DeleteClient(_mockCache.Object, _mockRepo.Object, client.Id);

		Assert.IsType<NoContent>(result);
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