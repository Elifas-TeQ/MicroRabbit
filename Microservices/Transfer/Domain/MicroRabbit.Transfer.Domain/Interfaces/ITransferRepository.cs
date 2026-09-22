using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain;

public interface ITransferRepository
{
    IEnumerable<TransferLog> GetTransferLogs();
}
