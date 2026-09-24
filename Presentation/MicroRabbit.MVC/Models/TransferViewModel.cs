namespace MicroRabbit.MVC.Models;

public class TransferViewModel
{
    public string TransferNotes { get; set; }

    public int SourceAccount { get; set; }

    public int TargetAccount { get; set; }

    public decimal Amount { get; set; }
}
