using MicroRabbit.MVC.Models.DTO;

namespace MicroRabbit.MVC.Services;

public interface ITransferService
{
    Task TransferAsync(TransferDTO transfer);
}
