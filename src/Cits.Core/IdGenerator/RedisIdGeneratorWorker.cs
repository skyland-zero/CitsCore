namespace Cits.Core.IdGenerator;

public class RedisIdGeneratorWorker : IIdGeneratorWorker
{
    public Task InitWorkerIdsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ushort> GetWorkerIdAsync()
    {
        throw new NotImplementedException();
    }

    public Task RefreshWorkerIdScoreAsync()
    {
        throw new NotImplementedException();
    }
}