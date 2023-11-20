using FreeSql.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Cits.OpenIddict.FreeSql.Models;

/// <summary>
/// Represents an OpenIddict application.
/// </summary>
public class CitsOpenIddictFreeSqlApplication : CitsOpenIddictFreeSqlApplication<string, CitsOpenIddictFreeSqlAuthorization, CitsOpenIddictFreeSqlToken>
{
    public CitsOpenIddictFreeSqlApplication()
    {
        // Generate a new string identifier.
        Id = Guid.NewGuid().ToString();
    }
}

/// <summary>
/// Represents an OpenIddict application.
/// </summary>
public class CitsOpenIddictFreeSqlApplication<TKey> : CitsOpenIddictFreeSqlApplication<TKey, CitsOpenIddictFreeSqlAuthorization<TKey>, CitsOpenIddictFreeSqlToken<TKey>>
    where TKey : notnull, IEquatable<TKey>
{
}

/// <summary>
/// Represents an OpenIddict application.
/// </summary>
[DebuggerDisplay("Id = {Id.ToString(),nq} ; ClientId = {ClientId,nq} ; Type = {Type,nq}")]
[Table(Name = "OpenIddictApplications")]
[Index(nameof(ClientId), nameof(ClientId), true)]
public class CitsOpenIddictFreeSqlApplication<TKey, TAuthorization, TToken>
    where TKey : notnull, IEquatable<TKey>
    where TAuthorization : class
    where TToken : class
{


    /// <summary>
    /// Gets or sets the client identifier associated with the current application.
    /// </summary>
    [Column(StringLength = 100)]
    public virtual string? ClientId { get; set; }

    /// <summary>
    /// Gets or sets the client secret associated with the current application.
    /// Note: depending on the application manager used to create this instance,
    /// this property may be hashed or encrypted for security reasons.
    /// </summary>
    public virtual string? ClientSecret { get; set; }

    /// <summary>
    /// Gets or sets the concurrency token.
    /// </summary>
    [Column(StringLength = 50)]
    public virtual string? ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the consent type associated with the current application.
    /// </summary>
    [Column(StringLength = 50)]
    public virtual string? ConsentType { get; set; }

    /// <summary>
    /// Gets or sets the display name associated with the current application.
    /// </summary>
    public virtual string? DisplayName { get; set; }

    /// <summary>
    /// Gets or sets the localized display names
    /// associated with the current application,
    /// serialized as a JSON object.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    public virtual string? DisplayNames { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier associated with the current application.
    /// </summary>
    [Column(IsPrimary = true)]
    public virtual TKey? Id { get; set; }

    /// <summary>
    /// Gets or sets the permissions associated with the
    /// current application, serialized as a JSON array.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    public virtual string? Permissions { get; set; }

    /// <summary>
    /// Gets or sets the post-logout redirect URIs associated with
    /// the current application, serialized as a JSON array.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    public virtual string? PostLogoutRedirectUris { get; set; }

    /// <summary>
    /// Gets or sets the additional properties serialized as a JSON object,
    /// or <see langword="null"/> if no bag was associated with the current application.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    public virtual string? Properties { get; set; }

    /// <summary>
    /// Gets or sets the redirect URIs associated with the
    /// current application, serialized as a JSON array.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    public virtual string? RedirectUris { get; set; }

    /// <summary>
    /// Gets or sets the requirements associated with the
    /// current application, serialized as a JSON array.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    public virtual string? Requirements { get; set; }

    /// <summary>
    /// Gets or sets the application type associated with the current application.
    /// </summary>
    [Column(StringLength = 50)]
    public virtual string? Type { get; set; }

    /// <summary>
    /// Gets the list of the authorizations associated with this application.
    /// </summary>
    [Navigate(nameof(CitsOpenIddictFreeSqlToken.ApplicationId))]
    public virtual List<TAuthorization>? Authorizations { get; set; }

    /// <summary>
    /// Gets the list of the tokens associated with this application.
    /// </summary>
    [Navigate(nameof(CitsOpenIddictFreeSqlToken.ApplicationId))]
    public virtual List<TToken>? Tokens { get; set; }
}
