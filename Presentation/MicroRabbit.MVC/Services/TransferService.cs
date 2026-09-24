using System.Text;
using System.Text.Json;
using MicroRabbit.MVC.Models.DTO;

namespace MicroRabbit.MVC.Services;

public class TransferService : ITransferService
{
    private readonly HttpClient _apiClient;

    public TransferService(HttpClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task TransferAsync(TransferDTO transfer)
    {
        var uri = "https://localhost:7255/api/Banking";
        var transferContent = new StringContent(
            JsonSerializer.Serialize(transfer),
            Encoding.UTF8,
            "application/json");

        var response = await _apiClient.PostAsync(uri, transferContent);
        response.EnsureSuccessStatusCode();
    }
}
