namespace OutboxWithDbContextInterface.Models;

public class NewChildMessage
{
    public required string Id { get; set; }
    public required string Name { get; set; }
}