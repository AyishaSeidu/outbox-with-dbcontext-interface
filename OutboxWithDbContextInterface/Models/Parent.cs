namespace OutboxWithDbContextInterface.Models;

public class Parent
{
    public long Id { get; set; }
    public required string ExternalId { get; set; }
    public List<string>? ChildrenIds { get; set; }
    public List<Child>? Children { get; set; }
}