using Cits.OpenIddict.FreeSql.Models;
using Cits.OpenIddict.FreeSql.Resolvers;
using Cits.OpenIddict.FreeSql.Stores;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection;

public static class CitsOpenIddictFreeSqlExtensions
{
    public static CitsOpenIddictFreeSqlBuilder UseFreeSql(this OpenIddictCoreBuilder builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder.DisableAdditionalFiltering();

        builder.SetDefaultApplicationEntity<CitsOpenIddictFreeSqlApplication>()
               .SetDefaultAuthorizationEntity<CitsOpenIddictFreeSqlAuthorization>()
               .SetDefaultScopeEntity<CitsOpenIddictFreeSqlScope>()
               .SetDefaultTokenEntity<CitsOpenIddictFreeSqlToken>();

        builder.ReplaceApplicationStoreResolver<CitsOpenIddictFreeSqlApplicationResolver>()
               .ReplaceAuthorizationStoreResolver<CitsOpenIddictFreeSqlAuthorizationResolver>()
               .ReplaceScopeStoreResolver<CitsOpenIddictFreeSqlScopeResolver>()
               .ReplaceTokenStoreResolver<CitsOpenIddictFreeSqlTokenResolver>();

        builder.Services.TryAddScoped(typeof(CitsOpenIddictFreeSqlApplicationStore<>));
        builder.Services.TryAddScoped(typeof(CitsOpenIddictFreeSqlAuthorizationStore<>));
        builder.Services.TryAddScoped(typeof(CitsOpenIddictFreeSqlScopeStore<>));
        builder.Services.TryAddScoped(typeof(CitsOpenIddictFreeSqlTokenStore<>));

        return new CitsOpenIddictFreeSqlBuilder(builder.Services);
    }

    public static OpenIddictCoreBuilder UseFreeSql(
        this OpenIddictCoreBuilder builder, Action<CitsOpenIddictFreeSqlBuilder> configuration)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        configuration(builder.UseFreeSql());

        return builder;
    }
}
