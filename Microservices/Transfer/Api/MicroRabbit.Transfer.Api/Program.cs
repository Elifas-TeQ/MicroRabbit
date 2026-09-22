using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Infra.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Domain.EventHandlers;
using MicroRabbit.Transfer.Domain.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();

builder.Services.AddMediatR((config) =>
{
    config.RegisterServicesFromAssembly(typeof(Program).Assembly);
});

builder.Services.AddSwaggerGen((setup) =>
{
    setup.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Transfer Microservice",
        Version = "v1"
    });
});

DependencyContainer.RegisterServices(builder.Services);
DependencyContainer.RegisterTransferServices(builder.Services);

builder.Services.AddDbContext<TransferDbContext>((options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("TransferDbConnection");
    options.UseSqlServer(connectionString);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseSwagger();
app.UseSwaggerUI((options) =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Transfer Microservice v1");
});

app.UseHttpsRedirection();

app.MapControllers();

var eventBus = app.Services.GetRequiredService<IEventBus>();
await eventBus.SubscribeAsync<TransferCreatedEvent, TransferEventHandler>();

app.Run();
