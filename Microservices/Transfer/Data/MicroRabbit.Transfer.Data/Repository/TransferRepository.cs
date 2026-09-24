using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Data.Repository;

public class TransferRepository : ITransferRepository
{
    private readonly TransferDbContext _context;

    public TransferRepository(TransferDbContext context)
    {
        _context = context;
    }

    public IEnumerable<TransferLog> GetTransferLogs()
    {
        return _context.TransferLogs;
    }

    public async Task AddAsync(TransferLog transferLog)
    {
        _context.TransferLogs.Add(transferLog);
        await _context.SaveChangesAsync();
    }
}
