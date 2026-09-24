using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain;

public interface ITransferRepository
{
    IEnumerable<TransferLog> GetTransferLogs();

    Task AddAsync(TransferLog transferLog);
}
