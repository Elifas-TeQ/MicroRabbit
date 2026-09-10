namespace MicroRabbit.Banking.Domain.Models;

public class AccountTransfer
{
    public int Source { get; set; }
    
    public int Target { get; set; }

    public decimal Amount { get; set; }
}
