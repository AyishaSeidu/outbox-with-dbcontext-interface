using MassTransit;
using OutboxWithDbContextInterface.Models;

namespace OutboxWithDbContextInterface.Consumers
{
    public class NewChildMessageConsumer : IConsumer<NewChildMessage>
    {
        public Task Consume(ConsumeContext<NewChildMessage> context)
        {
            Console.WriteLine($"New child added with Id {context.Message.Id} and Name {context.Message.Name}");
            return Task.CompletedTask;
        }
    }
}
