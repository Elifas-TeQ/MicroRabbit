namespace MicroRabbit.Transfer.Domain.Models;

public class TransferLog
{
    public int Id { get; set; }
    
    public int Source { get; set; }

    public int Target { get; set; }

    public decimal Amount { get; set; }
}
