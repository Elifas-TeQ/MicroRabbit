using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MicroRabbit.MVC.Models;
using MicroRabbit.MVC.Services;
using MicroRabbit.MVC.Models.DTO;

namespace MicroRabbit.MVC.Controllers;

public class HomeController : Controller
{
    private readonly ITransferService _transferService;

    public HomeController(ITransferService transferService)
    {
        _transferService = transferService;
    }

    [HttpPost]
    public async Task<IActionResult> Transfer(TransferViewModel viewModel)
    {
        var transfer = new TransferDTO
        {
            Source = viewModel.SourceAccount,
            Target = viewModel.TargetAccount,
            Amount = viewModel.Amount
        };

        await _transferService.TransferAsync(transfer);
        return View(nameof(Index));
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
