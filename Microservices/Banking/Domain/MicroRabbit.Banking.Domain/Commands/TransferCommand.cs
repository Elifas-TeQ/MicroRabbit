using MicroRabbit.Domain.Core.Commands;

namespace MicroRabbit.Banking.Domain.Commands;

public abstract class TransferCommand : Command
{
    public int Source { get; protected set; }
    
    public int Target { get; protected set; }

    public decimal Amount { get; protected set; }
}
