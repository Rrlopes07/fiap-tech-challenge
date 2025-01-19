namespace Core.Entity;

public class Client : EntityBase
{
    public required string Name { get; set; }
    public required string Telephone { get; set; }
    public required string Email { get; set; }
    public Guid DddId { get; set; }
    public Ddd Ddd { get; set; }
}
