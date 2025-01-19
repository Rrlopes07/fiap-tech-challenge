namespace ContactsApi.Extensions;

public static class DddExtensions
{
	public static Ddd ToDdd(this DddRequest request)
		=> new() 
		{ 
			Id = Guid.NewGuid(), 
			Region = request.Region, 
			DddNumber = request.DddNumber
		};

	public static DddResponse ToResponse(this Ddd ddd)
	=> new(ddd.Id, ddd.Region, ddd.DddNumber);

	public static List<DddResponse> ToResponseList(this IList<Ddd> list)
		=> list.Select(d => d.ToResponse()).ToList();
}
