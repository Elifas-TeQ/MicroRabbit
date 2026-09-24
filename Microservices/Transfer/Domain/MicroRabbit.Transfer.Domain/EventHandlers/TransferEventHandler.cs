using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.Events;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Domain.EventHandlers;

public class TransferEventHandler : IEventHandler<TransferCreatedEvent>
{
    private readonly ITransferRepository _transferRepository;

    public TransferEventHandler(ITransferRepository transferRepository)
    {
        _transferRepository = transferRepository;
    }

    public async Task HandleAsync(TransferCreatedEvent @event)
    {
        var transfer = new TransferLog
        {
            Source = @event.Source,
            Target = @event.Target,
            Amount = @event.Amount
        };

        await _transferRepository.AddAsync(transfer);
    }
}
