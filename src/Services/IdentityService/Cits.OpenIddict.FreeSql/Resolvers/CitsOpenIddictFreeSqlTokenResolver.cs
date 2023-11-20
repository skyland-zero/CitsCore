using Cits.OpenIddict.FreeSql.Models;
using Cits.OpenIddict.FreeSql.Stores;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Cits.OpenIddict.FreeSql.Resolvers;

public sealed class CitsOpenIddictFreeSqlTokenResolver : IOpenIddictTokenStoreResolver
{
    private readonly ConcurrentDictionary<Type, Type> _cache = new ConcurrentDictionary<Type, Type>();
    private readonly IServiceProvider _provider;

    public CitsOpenIddictFreeSqlTokenResolver(IServiceProvider provider)
        => _provider = provider ?? throw new ArgumentNullException(nameof(provider));

    /// <summary>
    /// Returns a token store compatible with the specified token type or throws an
    /// <see cref="InvalidOperationException"/> if no store can be built using the specified type.
    /// </summary>
    /// <typeparam name="TToken">The type of the Token entity.</typeparam>
    /// <returns>An <see cref="IOpenIddictTokenStore{TToken}"/>.</returns>
    public IOpenIddictTokenStore<TToken> Get<TToken>() where TToken : class
    {
        var store = _provider.GetService<IOpenIddictTokenStore<TToken>>();
        if (store is not null)
        {
            return store;
        }

        var type = _cache.GetOrAdd(typeof(TToken), key =>
        {
            if (!typeof(CitsOpenIddictFreeSqlToken).IsAssignableFrom(key))
            {
                throw new InvalidOperationException(SR.GetResourceString(SR.ID0260));
            }

            return typeof(CitsOpenIddictFreeSqlTokenStore<>).MakeGenericType(key);
        });

        return (IOpenIddictTokenStore<TToken>)_provider.GetRequiredService(type);
    }
}
