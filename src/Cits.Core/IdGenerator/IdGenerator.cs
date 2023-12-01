using Yitter.IdGenerator;

namespace Cits.Core.IdGenerator;

public static class IdGenerator
{
    public static long NextId()
    {
        return YitIdHelper.NextId();
    }

    public static string Id()
    {
        return YitIdHelper.NextId().ToString();
    }
}