namespace Cits.Core.IdGenerator;

public interface IIdGeneratorWorker
{
    Task InitWorkerIdsAsync();

    Task<ushort> GetWorkerIdAsync();

    Task RefreshWorkerIdScoreAsync();
}