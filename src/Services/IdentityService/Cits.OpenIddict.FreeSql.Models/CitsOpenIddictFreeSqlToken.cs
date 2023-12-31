﻿using FreeSql.DataAnnotations;
using System.Diagnostics;

namespace Cits.OpenIddict.FreeSql.Models;

/// <summary>
/// Represents an OpenIddict token.
/// </summary>
public class CitsOpenIddictFreeSqlToken : CitsOpenIddictFreeSqlToken<string, CitsOpenIddictFreeSqlApplication, CitsOpenIddictFreeSqlAuthorization>
{
    public CitsOpenIddictFreeSqlToken()
    {
        // Generate a new string identifier.
        Id = Guid.NewGuid().ToString();
    }
}

/// <summary>
/// Represents an OpenIddict token.
/// </summary>
public class CitsOpenIddictFreeSqlToken<TKey> : CitsOpenIddictFreeSqlToken<TKey, CitsOpenIddictFreeSqlApplication<TKey>, CitsOpenIddictFreeSqlAuthorization<TKey>>
    where TKey : notnull, IEquatable<TKey>
{
}

/// <summary>
/// Represents an OpenIddict token.
/// </summary>
[DebuggerDisplay("Id = {Id.ToString(),nq} ; Subject = {Subject,nq} ; Type = {Type,nq} ; Status = {Status,nq}")]
[Table(Name = "OpenIddictTokens")]
[Index(nameof(ReferenceId), nameof(ReferenceId), true)]
[Index(nameof(ApplicationId) + nameof(Id), $"{nameof(ApplicationId)}{nameof(Id)},{nameof(Status)},{nameof(Subject)},{nameof(Type)}")]
public class CitsOpenIddictFreeSqlToken<TKey, TApplication, TAuthorization>
    where TKey : notnull, IEquatable<TKey>
    where TApplication : class
    where TAuthorization : class
{
    /// <summary>
    /// Gets or sets the application associated with the current token.
    /// </summary>
    public virtual string? ApplicationId { get; set; }

    /// <summary>
    /// Gets or sets the authorization associated with the current token.
    /// </summary>
    public virtual string? AuthorizationId { get; set; }

    /// <summary>
    /// Gets or sets the concurrency token.
    /// </summary>
    [Column(StringLength = 50)]
    public virtual string? ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the UTC creation date of the current token.
    /// </summary>
    public virtual DateTime? CreationDate { get; set; }

    /// <summary>
    /// Gets or sets the UTC expiration date of the current token.
    /// </summary>
    public virtual DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier associated with the current token.
    /// </summary>
    [Column(IsPrimary = true)]
    public virtual TKey? Id { get; set; }

    /// <summary>
    /// Gets or sets the payload of the current token, if applicable.
    /// Note: this property is only used for reference tokens
    /// and may be encrypted for security reasons.
    /// </summary>
    public virtual string? Payload { get; set; }

    /// <summary>
    /// Gets or sets the additional properties serialized as a JSON object,
    /// or <see langword="null"/> if no bag was associated with the current token.
    /// </summary>
    public virtual string? Properties { get; set; }

    /// <summary>
    /// Gets or sets the UTC redemption date of the current token.
    /// </summary>
    public virtual DateTime? RedemptionDate { get; set; }

    /// <summary>
    /// Gets or sets the reference identifier associated
    /// with the current token, if applicable.
    /// Note: this property is only used for reference tokens
    /// and may be hashed or encrypted for security reasons.
    /// </summary>
    [Column(StringLength = 100)]
    public virtual string? ReferenceId { get; set; }

    /// <summary>
    /// Gets or sets the status of the current token.
    /// </summary>
    [Column(StringLength = 50)]
    public virtual string? Status { get; set; }

    /// <summary>
    /// Gets or sets the subject associated with the current token.
    /// </summary>
    [Column(StringLength = 400)]
    public virtual string? Subject { get; set; }

    /// <summary>
    /// Gets or sets the type of the current token.
    /// </summary>
    [Column(StringLength = 50)]
    public virtual string? Type { get; set; }

    /// <summary>
    /// 导航属性
    /// </summary>
    [Navigate(nameof(AuthorizationId))]
    public virtual TAuthorization? Authorization { get; set; }

    /// <summary>
    /// 导航属性
    /// </summary>
    [Navigate(nameof(ApplicationId))]
    public virtual TApplication? Application { get; set; }
}