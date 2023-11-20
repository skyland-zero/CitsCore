using Cits.OpenIddict.FreeSql;
using Cits.OpenIddict.FreeSql.Models;
using OpenIddict.Core;
using System.ComponentModel;

namespace Microsoft.Extensions.DependencyInjection;

public sealed class CitsOpenIddictFreeSqlBuilder
{
    /// <summary>
    /// Initializes a new instance of <see cref="CitsOpenIddictFreeSqlBuilder"/>.
    /// </summary>
    /// <param name="services">The services collection.</param>
    public CitsOpenIddictFreeSqlBuilder(IServiceCollection services)
        => Services = services ?? throw new ArgumentNullException(nameof(services));

    /// <summary>
    /// Gets the services collection.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public IServiceCollection Services { get; }

    /// <summary>
    /// Amends the default OpenIddict MongoDB configuration.
    /// </summary>
    /// <param name="configuration">The delegate used to configure the OpenIddict options.</param>
    /// <remarks>This extension can be safely called multiple times.</remarks>
    /// <returns>The <see cref="CitsOpenIddictFreeSqlBuilder"/> instance.</returns>
    public CitsOpenIddictFreeSqlBuilder Configure(Action<CitsOpenIddictFreeSqlOptions> configuration)
    {
        if (configuration is null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        Services.Configure(configuration);

        return this;
    }

    /// <summary>
    /// Configures OpenIddict to use the specified entity as the default application entity.
    /// </summary>
    /// <returns>The <see cref="CitsOpenIddictFreeSqlBuilder"/> instance.</returns>
    public CitsOpenIddictFreeSqlBuilder ReplaceDefaultApplicationEntity<TApplication>()
        where TApplication : CitsOpenIddictFreeSqlApplication
    {
        Services.Configure<OpenIddictCoreOptions>(options => options.DefaultApplicationType = typeof(TApplication));

        return this;
    }

    /// <summary>
    /// Configures OpenIddict to use the specified entity as the default authorization entity.
    /// </summary>
    /// <returns>The <see cref="CitsOpenIddictFreeSqlBuilder"/> instance.</returns>
    public CitsOpenIddictFreeSqlBuilder ReplaceDefaultAuthorizationEntity<TAuthorization>()
        where TAuthorization : CitsOpenIddictFreeSqlAuthorization
    {
        Services.Configure<OpenIddictCoreOptions>(options => options.DefaultAuthorizationType = typeof(TAuthorization));

        return this;
    }

    /// <summary>
    /// Configures OpenIddict to use the specified entity as the default scope entity.
    /// </summary>
    /// <returns>The <see cref="CitsOpenIddictFreeSqlBuilder"/> instance.</returns>
    public CitsOpenIddictFreeSqlBuilder ReplaceDefaultScopeEntity<TScope>()
        where TScope : CitsOpenIddictFreeSqlScope
    {
        Services.Configure<OpenIddictCoreOptions>(options => options.DefaultScopeType = typeof(TScope));

        return this;
    }

    /// <summary>
    /// Configures OpenIddict to use the specified entity as the default token entity.
    /// </summary>
    /// <returns>The <see cref="CitsOpenIddictFreeSqlBuilder"/> instance.</returns>
    public CitsOpenIddictFreeSqlBuilder ReplaceDefaultTokenEntity<TToken>()
        where TToken : CitsOpenIddictFreeSqlToken
    {
        Services.Configure<OpenIddictCoreOptions>(options => options.DefaultTokenType = typeof(TToken));

        return this;
    }


    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override bool Equals(object? obj) => base.Equals(obj);

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override int GetHashCode() => base.GetHashCode();

    /// <inheritdoc/>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public override string? ToString() => base.ToString();
}
