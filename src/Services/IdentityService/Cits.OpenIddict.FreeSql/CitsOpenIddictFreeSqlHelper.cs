using FreeSql;
using System.Runtime.CompilerServices;

namespace Cits.OpenIddict.FreeSql;

internal static class CitsOpenIddictFreeSqlHelper
{
    internal static IAsyncEnumerable<T> AsAsyncEnumerable<T>(this ISelect<T> source, CancellationToken cancellationToken)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return ExecuteAsync(source, cancellationToken);

        static async IAsyncEnumerable<T> ExecuteAsync(ISelect<T> source, [EnumeratorCancellation] CancellationToken cancellationToken)
        {

            foreach (var element in await source.ToListAsync(cancellationToken))
            {
                yield return element;
            }
        }
    }

    internal static IAsyncEnumerable<T> AsAsyncEnumerable<T>(this IQueryable<T> source, CancellationToken cancellationToken)
    {
        if (source is null)
        {
            throw new ArgumentNullException(nameof(source));
        }

        return ExecuteAsync(source, cancellationToken);

        static async IAsyncEnumerable<T> ExecuteAsync(IQueryable<T> source, [EnumeratorCancellation] CancellationToken cancellationToken)
        {

            foreach (var element in source.ToList())
            {
                yield return element;
            }
        }
    }
}
