using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Infra.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

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
        Title = "Banking Microservice",
        Version = "v1"
    });
});

DependencyContainer.RegisterServices(builder.Services);
DependencyContainer.RegisterBankingServices(builder.Services);

builder.Services.AddDbContext<BankingDbContext>((options) =>
{
    var connectionString = builder.Configuration.GetConnectionString("BankingDbConnection");
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
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Banking Microservice v1");
});

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
