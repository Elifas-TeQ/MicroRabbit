using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Services;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Data.Repository;
using MicroRabbit.Transfer.Domain;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC;

public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        // Shared cross-service registration
        services.AddSingleton<IEventBus, RabbitMqBus>();
    }

    /// Banking-specific registrations
    public static void RegisterBankingServices(IServiceCollection services)
    {
        // Domain Commands
        services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();

        // Application Services
        services.AddTransient<IAccountService, AccountService>();

        // Data
        services.AddTransient<IAccountRepository, AccountRepository>();
        //services.AddTransient<BankingDbContext>();
    }

    /// Transfer-specific registrations
    public static void RegisterTransferServices(IServiceCollection services)
    {
        // Domain Events
        services.AddTransient<IEventHandler<TransferCreatedEvent>,  TransferEventHandler>();

        // Application Services
        services.AddTransient<ITransferService, TransferService>();

        // Data
        services.AddTransient<ITransferRepository, TransferRepository>();
        //services.AddTransient<TransferDbContext>();
    }
}
