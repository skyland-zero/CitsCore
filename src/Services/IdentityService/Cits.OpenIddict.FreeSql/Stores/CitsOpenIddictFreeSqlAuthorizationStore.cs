using Cits.OpenIddict.FreeSql.Models;
using FreeSql;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using static OpenIddict.Abstractions.OpenIddictExceptions;

namespace Cits.OpenIddict.FreeSql.Stores;

public class CitsOpenIddictFreeSqlAuthorizationStore<TAuthorization> : IOpenIddictAuthorizationStore<TAuthorization>
    where TAuthorization : CitsOpenIddictFreeSqlAuthorization
{
    public CitsOpenIddictFreeSqlAuthorizationStore(IFreeSql<CitsOpenIddictFreeSqlMark> freeSql, IMemoryCache cache)
    {
        Cache = cache;
        FreeSql = freeSql;
    }

    protected IMemoryCache Cache { get; }
    protected IFreeSql<CitsOpenIddictFreeSqlMark> FreeSql { get; }


    /// <inheritdoc/>
    public virtual async ValueTask<long> CountAsync(CancellationToken cancellationToken)
        => await FreeSql.Select<TAuthorization>().CountAsync();

    /// <inheritdoc/>
    public virtual async ValueTask<long> CountAsync<TResult>(Func<IQueryable<TAuthorization>, IQueryable<TResult>> query, CancellationToken cancellationToken)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }
        //TODO async
        return query(FreeSql.Select<TAuthorization>().AsQueryable()).Count();
    }

    /// <inheritdoc/>
    public virtual async ValueTask CreateAsync(TAuthorization authorization, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        await FreeSql.Insert(authorization).ExecuteAffrowsAsync();
    }

    /// <inheritdoc/>
    public virtual async ValueTask DeleteAsync(TAuthorization authorization, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        using var uow = FreeSql.CreateUnitOfWork();
        if ((await uow.Orm.Delete<TAuthorization>()
            .Where(entity => entity.Id == authorization.Id && entity.ConcurrencyToken == authorization.ConcurrencyToken)
            .ExecuteAffrowsAsync(cancellationToken)) is 0)
        {
            throw new ConcurrencyException(SR.GetResourceString(SR.ID0241));
        }

        // Delete the tokens associated with the authorization.
        await uow.Orm.Delete<CitsOpenIddictFreeSqlToken>()
            .Where(a => a.AuthorizationId == authorization.Id)
            .ExecuteAffrowsAsync();

        uow.Commit();

    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TAuthorization> FindAsync(
        string subject, string client, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(subject))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0198), nameof(subject));
        }

        if (string.IsNullOrEmpty(client))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0124), nameof(client));
        }

        return ExecuteAsync(cancellationToken);

        async IAsyncEnumerable<TAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = await FreeSql.Select<TAuthorization>()
                .Where(authorization => authorization.Subject == subject && authorization.ApplicationId == client)
                .ToListAsync(cancellationToken);
            //TODO 查询优化，参考官方库
            foreach (var authorization in authorizations)
            {
                yield return authorization;
            }
        }

    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TAuthorization> FindAsync(
        string subject, string client,
        string status, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(subject))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0198), nameof(subject));
        }

        if (string.IsNullOrEmpty(client))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0124), nameof(client));
        }

        if (string.IsNullOrEmpty(status))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0199), nameof(status));
        }

        return ExecuteAsync(cancellationToken);

        async IAsyncEnumerable<TAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = await FreeSql.Select<TAuthorization>()
               .Where(authorization =>
                   authorization.Subject == subject &&
                   authorization.ApplicationId == client &&
                   authorization.Status == status)
               .ToListAsync(cancellationToken);
            //TODO 查询优化，参考官方库
            foreach (var authorization in authorizations)
            {
                yield return authorization;
            }
        }
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TAuthorization> FindAsync(
        string subject, string client,
        string status, string type, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(subject))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0198), nameof(subject));
        }

        if (string.IsNullOrEmpty(client))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0124), nameof(client));
        }

        if (string.IsNullOrEmpty(status))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0199), nameof(status));
        }

        if (string.IsNullOrEmpty(type))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0200), nameof(type));
        }

        return ExecuteAsync(cancellationToken);

        async IAsyncEnumerable<TAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = await FreeSql.Select<TAuthorization>()
               .Where(authorization =>
                   authorization.Subject == subject &&
                   authorization.ApplicationId == client &&
                   authorization.Status == status &&
                   authorization.Type == type)
               .ToListAsync(cancellationToken);
            //TODO 查询优化，参考官方库
            foreach (var authorization in authorizations)
            {
                yield return authorization;
            }
        }
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TAuthorization> FindAsync(
        string subject, string client,
        string status, string type,
        ImmutableArray<string> scopes, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(subject))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0198), nameof(subject));
        }

        if (string.IsNullOrEmpty(client))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0124), nameof(client));
        }

        if (string.IsNullOrEmpty(status))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0199), nameof(status));
        }

        if (string.IsNullOrEmpty(type))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0200), nameof(type));
        }

        return ExecuteAsync(cancellationToken);

        async IAsyncEnumerable<TAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {

            var authorizations = await FreeSql.Select<TAuthorization>()
               .Where(authorization =>
                   authorization.Subject == subject &&
                   authorization.ApplicationId == client &&
                   authorization.Status == status &&
                   authorization.Type == type)
               .ToListAsync(cancellationToken);

            foreach (var authorization in authorizations)
            {
                if ((await GetScopesAsync(authorization, cancellationToken))
                    .ToHashSet(StringComparer.Ordinal)
                    .IsSupersetOf(scopes))
                {
                    yield return authorization;
                }
            }
        }
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TAuthorization> FindByApplicationIdAsync(
        string identifier, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
        }

        return ExecuteAsync(cancellationToken);

        async IAsyncEnumerable<TAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = await FreeSql.Select<TAuthorization>()
               .Where(authorization => authorization.ApplicationId == identifier)
               .ToListAsync(cancellationToken);

            foreach (var authorization in authorizations)
            {
                yield return authorization;
            }
        }
    }

    /// <inheritdoc/>
    public virtual async ValueTask<TAuthorization?> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
        }

        return await FreeSql.Select<TAuthorization>()
            .Where(authorization => authorization.Id == identifier)
            .ToOneAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TAuthorization> FindBySubjectAsync(
        string subject, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(subject))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0198), nameof(subject));
        }

        return ExecuteAsync(cancellationToken);

        async IAsyncEnumerable<TAuthorization> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var authorizations = await FreeSql.Select<TAuthorization>()
               .Where(authorization => authorization.Subject == subject)
               .ToListAsync(cancellationToken);

            foreach (var authorization in authorizations)
            {
                yield return authorization;
            }
        }
    }

    /// <inheritdoc/>
    public virtual async ValueTask<string?> GetApplicationIdAsync(TAuthorization authorization, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        if (string.IsNullOrWhiteSpace(authorization.ApplicationId))
        {
            return null;
        }

        return authorization.ApplicationId;
    }

    /// <inheritdoc/>
    public virtual async ValueTask<TResult?> GetAsync<TState, TResult>(
        Func<IQueryable<TAuthorization>, TState, IQueryable<TResult>> query,
        TState state, CancellationToken cancellationToken)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        //TODO async
        return query(FreeSql.Select<TAuthorization>().AsQueryable(), state).FirstOrDefault();
    }

    /// <inheritdoc/>
    public virtual ValueTask<DateTimeOffset?> GetCreationDateAsync(TAuthorization authorization, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        if (authorization.CreationDate is null)
        {
            return new(result: null);
        }

        return new(DateTime.SpecifyKind(authorization.CreationDate.Value, DateTimeKind.Utc));
    }

    /// <inheritdoc/>
    public virtual ValueTask<string?> GetIdAsync(TAuthorization authorization, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        return new(authorization.Id);
    }

    /// <inheritdoc/>
    public virtual ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(TAuthorization authorization, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        if (string.IsNullOrEmpty(authorization.Properties))
        {
            return new(ImmutableDictionary.Create<string, JsonElement>());
        }

        // Note: parsing the stringified properties is an expensive operation.
        // To mitigate that, the resulting object is stored in the memory cache.
        var key = string.Concat("68056e1a-dbcf-412b-9a6a-d791c7dbe726", "\x1e", authorization.Properties);
        var properties = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                 .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(authorization.Properties);
            var builder = ImmutableDictionary.CreateBuilder<string, JsonElement>();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                builder[property.Name] = property.Value.Clone();
            }

            return builder.ToImmutable();
        })!;

        return new(properties);
    }

    /// <inheritdoc/>
    public virtual ValueTask<ImmutableArray<string>> GetScopesAsync(TAuthorization authorization, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        if (string.IsNullOrEmpty(authorization.Scopes))
        {
            return new(ImmutableArray.Create<string>());
        }

        // Note: parsing the stringified scopes is an expensive operation.
        // To mitigate that, the resulting array is stored in the memory cache.
        var key = string.Concat("2ba4ab0f-e2ec-4d48-b3bd-28e2bb660c75", "\x1e", authorization.Scopes);
        var scopes = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                 .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(authorization.Scopes);
            var builder = ImmutableArray.CreateBuilder<string>(document.RootElement.GetArrayLength());

            foreach (var element in document.RootElement.EnumerateArray())
            {
                var value = element.GetString();
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                builder.Add(value);
            }

            return builder.ToImmutable();
        });

        return new(scopes);
    }

    /// <inheritdoc/>
    public virtual ValueTask<string?> GetStatusAsync(TAuthorization authorization, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        return new(authorization.Status);
    }

    /// <inheritdoc/>
    public virtual ValueTask<string?> GetSubjectAsync(TAuthorization authorization, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        return new(authorization.Subject);
    }

    /// <inheritdoc/>
    public virtual ValueTask<string?> GetTypeAsync(TAuthorization authorization, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        return new(authorization.Type);
    }

    /// <inheritdoc/>
    public virtual ValueTask<TAuthorization> InstantiateAsync(CancellationToken cancellationToken)
    {
        try
        {
            return new(Activator.CreateInstance<TAuthorization>());
        }

        catch (MemberAccessException exception)
        {
            return new(Task.FromException<TAuthorization>(
                new InvalidOperationException(SR.GetResourceString(SR.ID0242), exception)));
        }
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TAuthorization> ListAsync(int? count, int? offset, CancellationToken cancellationToken)
    {
        var query = FreeSql.Select<TAuthorization>()
            .OrderBy(authorization => authorization.Id);

        if (offset.HasValue)
        {
            query = query.Skip(offset.Value);
        }

        if (count.HasValue)
        {
            query = query.Take(count.Value);
        }

        return query.AsAsyncEnumerable(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TResult> ListAsync<TState, TResult>(
        Func<IQueryable<TAuthorization>, TState, IQueryable<TResult>> query,
        TState state, CancellationToken cancellationToken)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        return query(FreeSql.Select<TAuthorization>().AsQueryable(), state).AsAsyncEnumerable(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async ValueTask PruneAsync(DateTimeOffset threshold, CancellationToken cancellationToken)
    {
        // Note: to avoid sending too many queries, the maximum number of elements
        // that can be removed by a single call to PruneAsync() is deliberately limited.
        for (var index = 0; index < 1_000; index++)
        {
            var date = threshold.UtcDateTime;

            using var uow = FreeSql.CreateUnitOfWork();

            var authorizations = await uow.Orm.Select<TAuthorization>()
                .Where(authorization => authorization.CreationDate < date)
                .Where(authorization => authorization.Status != Statuses.Valid ||
                    (authorization.Type == AuthorizationTypes.AdHoc && !authorization.Tokens.Any()))
                .OrderBy(authorization => authorization.Id)
                .Take(1_000)
                .ToListAsync(cancellationToken);

            if (authorizations.Count is 0)
            {
                break;
            }

            await uow.Orm.Delete<TAuthorization>(authorizations).ExecuteAffrowsAsync(cancellationToken);

            var ids = authorizations.Select(a => a.Id);
            await uow.Orm.Delete<CitsOpenIddictFreeSqlToken>().Where(token => ids.Contains(token.AuthorizationId)).ExecuteAffrowsAsync();

            uow.Commit();
        }

    }

    /// <inheritdoc/>
    public virtual async ValueTask SetApplicationIdAsync(TAuthorization authorization,
        string? identifier, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        if (!string.IsNullOrEmpty(identifier))
        {
            authorization.ApplicationId = identifier;
        }

        else
        {
            authorization.ApplicationId = null;
        }
    }

    /// <inheritdoc/>
    public virtual ValueTask SetCreationDateAsync(TAuthorization authorization,
        DateTimeOffset? date, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        authorization.CreationDate = date?.UtcDateTime;

        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask SetPropertiesAsync(TAuthorization authorization,
        ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        if (properties is not { Count: > 0 })
        {
            authorization.Properties = null;

            return default;
        }

        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Indented = false
        });

        writer.WriteStartObject();

        foreach (var property in properties)
        {
            writer.WritePropertyName(property.Key);
            property.Value.WriteTo(writer);
        }

        writer.WriteEndObject();
        writer.Flush();

        authorization.Properties = Encoding.UTF8.GetString(stream.ToArray());

        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask SetScopesAsync(TAuthorization authorization,
        ImmutableArray<string> scopes, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        if (scopes.IsDefaultOrEmpty)
        {
            authorization.Scopes = null;

            return default;
        }

        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Indented = false
        });

        writer.WriteStartArray();

        foreach (var scope in scopes)
        {
            writer.WriteStringValue(scope);
        }

        writer.WriteEndArray();
        writer.Flush();

        authorization.Scopes = Encoding.UTF8.GetString(stream.ToArray());

        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask SetStatusAsync(TAuthorization authorization,
        string? status, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        authorization.Status = status;

        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask SetSubjectAsync(TAuthorization authorization,
        string? subject, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        authorization.Subject = subject;

        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask SetTypeAsync(TAuthorization authorization,
        string? type, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        authorization.Type = type;

        return default;
    }

    /// <inheritdoc/>
    public virtual async ValueTask UpdateAsync(TAuthorization authorization, CancellationToken cancellationToken)
    {
        if (authorization is null)
        {
            throw new ArgumentNullException(nameof(authorization));
        }

        var timestamp = authorization.ConcurrencyToken;
        authorization.ConcurrencyToken = Guid.NewGuid().ToString();
        if ((await FreeSql.Update<TAuthorization>()
            .Where(entity => entity.Id == authorization.Id && entity.ConcurrencyToken == timestamp)
            .SetSource(authorization)
            .ExecuteAffrowsAsync(cancellationToken)) is 0)
        {
            throw new ConcurrencyException(SR.GetResourceString(SR.ID0241));
        }
    }
}
