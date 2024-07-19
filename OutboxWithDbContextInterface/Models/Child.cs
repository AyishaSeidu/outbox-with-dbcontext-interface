namespace OutboxWithDbContextInterface.Models;

public class Child
{
    public long Id { get; set; }
    public string ExternalId { get; set; }
    public string? Name { get; set; }
    public long ParentId { get; set; }
    public Parent Parent { get; set; }
}