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
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC;

public class DependencyContainer
{
    public static void RegisterServices(IServiceCollection services)
    {
        // Shared cross-service registration
        services.AddSingleton<IEventBus, RabbitMqBus>();
    }

    public static void RegisterBankingServices(IServiceCollection services)
    {
        // Banking-specific registrations
        services.AddTransient<IRequestHandler<CreateTransferCommand, bool>, TransferCommandHandler>();
        services.AddTransient<IAccountService, AccountService>();
        services.AddTransient<IAccountRepository, AccountRepository>();
        //services.AddTransient<BankingDbContext>();
    }

    public static void RegisterTransferServices(IServiceCollection services)
    {
        // Transfer-specific registrations
        services.AddTransient<ITransferService, TransferService>();
        services.AddTransient<ITransferRepository, TransferRepository>();
        //services.AddTransient<TransferDbContext>();
    }
}
