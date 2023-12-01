using Cits.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Cits.Core.IdGenerator;

public class DefaultIdGeneratorWorker : IIdGeneratorWorker
{
    private readonly ServiceOptions _serviceOptions;

    public DefaultIdGeneratorWorker(IOptionsMonitor<ServiceOptions> serviceOptions)
    {
        _serviceOptions = serviceOptions.CurrentValue;
    }

    public Task InitWorkerIdsAsync()
    {
        return Task.CompletedTask;
    }

    public Task<ushort> GetWorkerIdAsync()
    {
        var workerId = _serviceOptions.WorkerId;
        return Task.FromResult(workerId);
    }

    public Task RefreshWorkerIdScoreAsync()
    {
        return Task.CompletedTask;
    }
}