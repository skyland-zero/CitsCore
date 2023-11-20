using Cits.OpenIddict.FreeSql.Models;
using Cits.OpenIddict.FreeSql.Stores;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Concurrent;

namespace Cits.OpenIddict.FreeSql.Resolvers;

public sealed class CitsOpenIddictFreeSqlApplicationResolver : IOpenIddictApplicationStoreResolver
{
    private readonly ConcurrentDictionary<Type, Type> _cache = new ConcurrentDictionary<Type, Type>();
    private readonly IServiceProvider _provider;

    public CitsOpenIddictFreeSqlApplicationResolver(
        IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <summary>
    /// Returns an application store compatible with the specified application type or throws an
    /// <see cref="InvalidOperationException"/> if no store can be built using the specified type.
    /// </summary>
    /// <typeparam name="TApplication">The type of the Application entity.</typeparam>
    /// <returns>An <see cref="IOpenIddictApplicationStore{TApplication}"/>.</returns>
    public IOpenIddictApplicationStore<TApplication> Get<TApplication>() where TApplication : class
    {
        var store = _provider.GetService<IOpenIddictApplicationStore<TApplication>>();
        if (store is not null)
        {
            return store;
        }



        var type = _cache.GetOrAdd(typeof(TApplication), key =>
        {
            if (!typeof(CitsOpenIddictFreeSqlApplication).IsAssignableFrom(key))
            {
                throw new InvalidOperationException(SR.GetResourceString(SR.ID0257));
            }

            return typeof(CitsOpenIddictFreeSqlApplicationStore<>).MakeGenericType(key);
        });

        return (IOpenIddictApplicationStore<TApplication>)_provider.GetRequiredService(type);
    }
}
