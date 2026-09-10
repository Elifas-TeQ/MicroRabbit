using MediatR;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Events;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Domain;

public class TransferCommandHandler : IRequestHandler<CreateTransferCommand, bool>
{
    private readonly IEventBus _eventBus;

    public TransferCommandHandler(IEventBus eventBus)
    {
        _eventBus = eventBus;
    }

    public async Task<bool> Handle(CreateTransferCommand request, CancellationToken cancellationToken)
    {
        var @event = new TransferCreatedEvent(
            request.Source,
            request.Target,
            request.Amount);

        // Publish an event to the RabbitMQ.
        await _eventBus.PublishAsync(@event);

        return true;
    }
}
