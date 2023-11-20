using Cits.OpenIddict.FreeSql.Models;
using Cits.OpenIddict.FreeSql.Stores;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Cits.OpenIddict.FreeSql.Resolvers;

public sealed class CitsOpenIddictFreeSqlAuthorizationResolver : IOpenIddictAuthorizationStoreResolver
{
    private readonly ConcurrentDictionary<Type, Type> _cache = new ConcurrentDictionary<Type, Type>();
    private readonly IServiceProvider _provider;

    public CitsOpenIddictFreeSqlAuthorizationResolver(IServiceProvider provider)
        => _provider = provider ?? throw new ArgumentNullException(nameof(provider));

    /// <summary>
    /// Returns an authorization store compatible with the specified authorization type or throws an
    /// <see cref="InvalidOperationException"/> if no store can be built using the specified type.
    /// </summary>
    /// <typeparam name="TAuthorization">The type of the Authorization entity.</typeparam>
    /// <returns>An <see cref="IOpenIddictAuthorizationStore{TAuthorization}"/>.</returns>
    public IOpenIddictAuthorizationStore<TAuthorization> Get<TAuthorization>() where TAuthorization : class
    {
        var store = _provider.GetService<IOpenIddictAuthorizationStore<TAuthorization>>();
        if (store is not null)
        {
            return store;
        }

        var type = _cache.GetOrAdd(typeof(TAuthorization), key =>
        {
            if (!typeof(CitsOpenIddictFreeSqlAuthorization).IsAssignableFrom(key))
            {
                throw new InvalidOperationException(SR.GetResourceString(SR.ID0258));
            }

            return typeof(CitsOpenIddictFreeSqlAuthorizationStore<>).MakeGenericType(key);
        });

        return (IOpenIddictAuthorizationStore<TAuthorization>)_provider.GetRequiredService(type);
    }
}
