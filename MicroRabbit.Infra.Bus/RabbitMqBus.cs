using System.Text;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroRabbit.Infra.Bus;

public sealed class RabbitMqBus : IEventBus
{
    private readonly IMediator _mediator;
    private readonly Dictionary<string, List<Type>> _handlers;
    private readonly List<Type> _eventTypes;

    public RabbitMqBus(IMediator mediator)
    {
        _mediator = mediator;
        _handlers = [];
        _eventTypes = [];
    }

    public Task SendCommand<T>(T command) where T : Command
    {
        return _mediator.Send(command);
    }

    public async Task PublishAsync<T>(T @event) where T : Event
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };

        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();

        var eventName = @event.GetType().Name;

        await channel.QueueDeclareAsync(eventName, durable: false, exclusive: false, autoDelete: false, arguments: null);

        var message = System.Text.Json.JsonSerializer.Serialize(@event);
        var body = Encoding.UTF8.GetBytes(message);

        await channel.BasicPublishAsync("", eventName, body);
    }

    public async Task SubscribeAsync<T, TH>()
        where T : Event
        where TH : IEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var handlerType = typeof(TH);

        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }

        if (!_handlers.ContainsKey(eventName))
        {
            _handlers.Add(eventName, []);
        }

        if (_handlers[eventName].Any((x) => x.GetType() == handlerType))
        {
            throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
        }

        _handlers[eventName].Add(handlerType);

        await StartBasicConsumeAsync<T>();
    }

    private async Task StartBasicConsumeAsync<T>() where T : Event
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
        };

        var connection = await factory.CreateConnectionAsync();
        var channel = await connection.CreateChannelAsync();

        var eventName = typeof(T).Name;

        await channel.QueueDeclareAsync(eventName, durable: false, exclusive: false, autoDelete: false);

        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += Consumer_Received;

        await channel.BasicConsumeAsync(eventName, autoAck: true, consumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
    {
        var eventName = e.RoutingKey;
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        try
        {
            await ProcessEventAsync(eventName, message).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing event '{eventName}': {ex.Message}");
        }
    }

    private async Task ProcessEventAsync(string eventName, string message)
    {
        if (_handlers.ContainsKey(eventName))
        {
            var subscriptions = _handlers[eventName];

            foreach (var subscription in subscriptions)
            {
                var handler = Activator.CreateInstance(subscription);
                if (handler == null) continue;

                var eventType = _eventTypes.SingleOrDefault((x) => x.Name == eventName);
                var @event = System.Text.Json.JsonSerializer.Deserialize(message, eventType);
                var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
            }
        }
    }
}
