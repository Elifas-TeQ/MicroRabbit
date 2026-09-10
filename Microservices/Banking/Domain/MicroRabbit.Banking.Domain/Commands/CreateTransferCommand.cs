namespace MicroRabbit.Banking.Domain.Commands;

public class CreateTransferCommand : TransferCommand
{
    public CreateTransferCommand(int source, int target, decimal amount)
    {
        Source = source;
        Target = target;
        Amount = amount;
    }
}
