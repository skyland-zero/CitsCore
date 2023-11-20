using FreeSql.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Cits.OpenIddict.FreeSql.Models;

/// <summary>
/// Represents an OpenIddict authorization.
/// </summary>
public class CitsOpenIddictFreeSqlAuthorization : CitsOpenIddictFreeSqlAuthorization<string, CitsOpenIddictFreeSqlApplication, CitsOpenIddictFreeSqlToken>
{
    public CitsOpenIddictFreeSqlAuthorization()
    {
        // Generate a new string identifier.
        Id = Guid.NewGuid().ToString();
    }
}

/// <summary>
/// Represents an OpenIddict authorization.
/// </summary>
public class CitsOpenIddictFreeSqlAuthorization<TKey> : CitsOpenIddictFreeSqlAuthorization<TKey, CitsOpenIddictFreeSqlApplication<TKey>, CitsOpenIddictFreeSqlToken<TKey>>
    where TKey : notnull, IEquatable<TKey>
{
}

/// <summary>
/// Represents an OpenIddict authorization.
/// </summary>
[DebuggerDisplay("Id = {Id.ToString(),nq} ; Subject = {Subject,nq} ; Type = {Type,nq} ; Status = {Status,nq}")]
[Table(Name = "OpenIddictAuthorizations")]
public class CitsOpenIddictFreeSqlAuthorization<TKey, TApplication, TToken>
    where TKey : notnull, IEquatable<TKey>
    where TApplication : class
    where TToken : class
{
    /// <summary>
    /// Gets or sets the application associated with the current authorization.
    /// </summary>
    [Column(StringLength = 50)]
    public virtual string? ApplicationId { get; set; }

    /// <summary>
    /// Gets or sets the concurrency token.
    /// </summary>
    [Column(StringLength = 50)]
    public virtual string? ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the UTC creation date of the current authorization.
    /// </summary>
    public virtual DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier associated with the current authorization.
    /// </summary>
    [Column(IsPrimary = true)]
    public virtual TKey? Id { get; set; }

    /// <summary>
    /// Gets or sets the additional properties serialized as a JSON object,
    /// or <see langword="null"/> if no bag was associated with the current authorization.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    public virtual string? Properties { get; set; }

    /// <summary>
    /// Gets or sets the scopes associated with the current
    /// authorization, serialized as a JSON array.
    /// </summary>
    public virtual string? Scopes { get; set; }

    /// <summary>
    /// Gets or sets the status of the current authorization.
    /// </summary>
    [Column(StringLength = 50)]
    public virtual string? Status { get; set; }

    /// <summary>
    /// Gets or sets the subject associated with the current authorization.
    /// </summary>
    [Column(StringLength = 400)]
    public virtual string? Subject { get; set; }

    /// <summary>
    /// Gets or sets the type of the current authorization.
    /// </summary>
    [Column(StringLength = 50)]
    public virtual string? Type { get; set; }

    /// <summary>
    /// Gets the list of tokens associated with the current authorization.
    /// </summary>
    [Navigate(nameof(CitsOpenIddictFreeSqlToken.AuthorizationId))]
    public virtual List<TToken> Tokens { get; set; }

    [Navigate(nameof(ApplicationId))]
    public virtual CitsOpenIddictFreeSqlApplication Application { get; set; }
}