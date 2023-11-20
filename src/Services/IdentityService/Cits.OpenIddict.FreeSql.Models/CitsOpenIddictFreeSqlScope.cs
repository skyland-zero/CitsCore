using FreeSql.DataAnnotations;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Cits.OpenIddict.FreeSql.Models;

/// <summary>
/// Represents an OpenIddict scope.
/// </summary>
public class CitsOpenIddictFreeSqlScope : CitsOpenIddictFreeSqlScope<string>
{
    public CitsOpenIddictFreeSqlScope()
    {
        // Generate a new string identifier.
        Id = Guid.NewGuid().ToString();
    }
}

/// <summary>
/// Represents an OpenIddict scope.
/// </summary>
[DebuggerDisplay("Id = {Id.ToString(),nq} ; Name = {Name,nq}")]
[Table(Name = "OpenIddictScopes")]
[Index(nameof(Name), nameof(Name))]
public class CitsOpenIddictFreeSqlScope<TKey> where TKey : notnull, IEquatable<TKey>
{
    /// <summary>
    /// Gets or sets the concurrency token.
    /// </summary>
    [Column(StringLength = 50)]
    public virtual string? ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the public description associated with the current scope.
    /// </summary>
    public virtual string? Description { get; set; }

    /// <summary>
    /// Gets or sets the localized public descriptions associated
    /// with the current scope, serialized as a JSON object.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    public virtual string? Descriptions { get; set; }

    /// <summary>
    /// Gets or sets the display name associated with the current scope.
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
    /// Gets or sets the unique identifier associated with the current scope.
    /// </summary>
    [Column(IsPrimary = true)]
    public virtual TKey? Id { get; set; }

    /// <summary>
    /// Gets or sets the unique name associated with the current scope.
    /// </summary>
    [Column(StringLength = 200)]
    public virtual string? Name { get; set; }

    /// <summary>
    /// Gets or sets the additional properties serialized as a JSON object,
    /// or <see langword="null"/> if no bag was associated with the current scope.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    public virtual string? Properties { get; set; }

    /// <summary>
    /// Gets or sets the resources associated with the
    /// current scope, serialized as a JSON array.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.Json)]
    public virtual string? Resources { get; set; }
}