using Api.Services.Interfaces;

namespace Api.Worker;

public class MinuteWorker : BackgroundService
{
    private readonly IBlockchainService _blockchainService;
    public MinuteWorker(IBlockchainService blockchainService)
    {
        _blockchainService = blockchainService;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await _blockchainService.TriggerMinute();

            await Task.Delay(1000, stoppingToken);
        }
    }
}
