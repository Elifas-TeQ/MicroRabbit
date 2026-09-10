using MicroRabbit.Domain.Core.Events;

namespace MicroRabbit.Banking.Domain.Events;

public class TransferCreatedEvent : Event
{
    public int Source { get; private set; }

    public int Target { get; private set; }

    public decimal Amount { get; private set; }

    public TransferCreatedEvent(int source, int target, decimal amount)
    {
        Source = source;
        Target = target;
        Amount = amount;
    }
}
