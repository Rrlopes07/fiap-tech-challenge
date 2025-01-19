namespace Core.Entity;

public class Ddd : EntityBase
{
    public required string Region { get; set; }
    public int DddNumber {  get; set; }
    public ICollection<Client>? Clients {  get; set; }
}
