namespace ContactsApi.Test;

public class EndpointDddTests
{
	private readonly Mock<IDddRepository> _mock;

    public EndpointDddTests()
    {
        _mock = new Mock<IDddRepository>();
    }

    [Fact]
	public async Task GetAllShouldReturnListWithResponses()
	{
		var firstDdd = GenerateDdd();
		var secondDdd = GenerateDdd();

		_mock.Setup(m => m.FindAll())
			.ReturnsAsync([firstDdd, secondDdd]);

		var result = await DddEndpoints.GetAllDdd(_mock.Object);

		Assert.IsType<Ok<List<DddResponse>>>(result);

		Assert.NotNull(result.Value);
		Assert.NotEmpty(result.Value);

		Assert.Collection(result.Value, ddd1 =>
		{ 
			Assert.Equal(firstDdd.Id, ddd1.Id);
			Assert.Equal(firstDdd.Region, ddd1.Region);
			Assert.Equal(firstDdd.DddNumber, ddd1.DddNumber);
		}, ddd2 =>
		{
			Assert.Equal(secondDdd.Id, ddd2.Id);
			Assert.Equal(secondDdd.Region, ddd2.Region);
			Assert.Equal(secondDdd.DddNumber, ddd2.DddNumber);
		});
	}

	[Fact]
	public async Task GetDddShouldReturnNotFoundIfNotExists()
	{
		Guid idToSearch = Guid.NewGuid();

		_mock.Setup(m => m.FindById(It.Is<Guid>(id => id == idToSearch)))
			.ReturnsAsync((Ddd?)null);

		var result = await DddEndpoints.FindById(_mock.Object, idToSearch);

		Assert.IsType<Results<Ok<DddResponse>, NotFound<string>>>(result);

		var notFoundResult = (NotFound<string>)result.Result;

		Assert.NotNull(notFoundResult);
	}

	[Fact]
	public void CreateDddGeneratesNewDdd()
	{
		var ddds = new List<Ddd>();
		var newDdd = new DddRequest("São Paulo", 11);

		_mock.Setup(m => m.Add(It.Is<Ddd>(d => d.Region == newDdd.Region && d.DddNumber == newDdd.DddNumber)))
			.Callback<Ddd>(ddds.Add);

		var result = DddEndpoints.AddDdd(_mock.Object, newDdd);

		Assert.IsType<Created>(result);
		Assert.NotEmpty(ddds);

		Assert.Collection(ddds, ddd =>
		{
			Assert.Equal("São Paulo", ddd.Region);
			Assert.Equal(11, ddd.DddNumber);
		});
	}

	[Fact]
	public async Task UpdateDddShouldUpdateDddInDatabase()
	{
		var existingDdd = GenerateDdd();

		var updatedDdd = new DddRequestEdit(existingDdd.Id, "Paraná", 41);

		_mock.Setup(m => m.FindById(It.Is<Guid>(id => id == existingDdd.Id)))
			.ReturnsAsync(existingDdd);

		_mock.Setup(m => m.Update(It.Is<Ddd>(d => d.Region == updatedDdd.Region && d.DddNumber == updatedDdd.DddNumber)))
			.Callback<Ddd>(ddd => existingDdd = ddd);

		var result = await DddEndpoints.UpdateDdd(_mock.Object, updatedDdd);

		Assert.IsType<Ok>(result);
	}

	[Fact]
	public async Task DeleteDddShouldDeleteInDatabase()
	{
		var ddd = GenerateDdd();

		var ddds = new List<Ddd> { ddd };

		_mock.Setup(m => m.FindById(It.Is<Guid>(id => id == ddd.Id)))
			.ReturnsAsync(ddd);

		_mock.Setup(m => m.Delete(It.Is<Ddd>(d => d.Id == ddd.Id)))
			.Callback<Ddd>(d => ddds.Remove(d));

		var result = await DddEndpoints.DeleteDdd(_mock.Object, ddd.Id);

		Assert.IsType<NoContent>(result);
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
}