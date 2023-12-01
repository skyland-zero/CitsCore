using System.Xml;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Yitter.IdGenerator;

namespace Cits.Core.IdGenerator;

public class WorkerNodeHostedService : BackgroundService
{
    private readonly ILogger<WorkerNodeHostedService> _logger;
    private readonly IIdGeneratorWorker _idGeneratorWorker;

    private readonly int _millisecondsDelay = 2_000;

    public WorkerNodeHostedService(ILogger<WorkerNodeHostedService> logger, IIdGeneratorWorker idGeneratorWorker)
    {
        _logger = logger;
        _idGeneratorWorker = idGeneratorWorker;
    }

    /// <summary>
    /// 程序启动时调用
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        await _idGeneratorWorker.InitWorkerIdsAsync();
        var workerId = await _idGeneratorWorker.GetWorkerIdAsync();
        var options = new IdGeneratorOptions
        {
            WorkerId = workerId
        };
        YitIdHelper.SetIdGenerator(options);

        await base.StartAsync(cancellationToken);
    }

    /// <summary>
    /// 程序停止时调用
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        await base.StopAsync(cancellationToken);

        var subtractionMilliseconds = 0 - (_millisecondsDelay * 1.5);
        var score = DateTime.Now.AddMilliseconds(subtractionMilliseconds).Millisecond;
        await _idGeneratorWorker.RefreshWorkerIdScoreAsync();
    }

    /// <summary>
    /// 程序启动时执行
    /// </summary>
    /// <param name="stoppingToken"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await Task.Delay(_millisecondsDelay, stoppingToken);
                
                if(stoppingToken.IsCancellationRequested) break;

                await _idGeneratorWorker.RefreshWorkerIdScoreAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);
                await Task.Delay(_millisecondsDelay, stoppingToken);
            }
        }
    }
}