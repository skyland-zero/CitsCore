using Cits.OpenIddict.FreeSql.Models;
using FreeSql;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Cits.OpenIddict.FreeSql.Stores;

public class CitsOpenIddictFreeSqlScopeStore<TScope> : IOpenIddictScopeStore<TScope>
    where TScope : CitsOpenIddictFreeSqlScope
{


    public CitsOpenIddictFreeSqlScopeStore(IFreeSql<CitsOpenIddictFreeSqlMark> freeSql, IMemoryCache cache)
    {
        Cache = cache;
        FreeSql = freeSql;
    }

    protected IMemoryCache Cache { get; }
    protected IFreeSql<CitsOpenIddictFreeSqlMark> FreeSql { get; }

    /// <inheritdoc/>
    public virtual async ValueTask<long> CountAsync(CancellationToken cancellationToken)
        => await FreeSql.Select<TScope>().CountAsync(cancellationToken);

    /// <inheritdoc/>
    public virtual async ValueTask<long> CountAsync<TResult>(Func<IQueryable<TScope>, IQueryable<TResult>> query, CancellationToken cancellationToken)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        return query(FreeSql.Select<TScope>().AsQueryable()).LongCount();
    }

    /// <inheritdoc/>
    public virtual async ValueTask CreateAsync(TScope scope, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        await FreeSql.Insert(scope).ExecuteAffrowsAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual async ValueTask DeleteAsync(TScope scope, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }
        
        await FreeSql.Delete<TScope>(scope).ExecuteAffrowsAsync(cancellationToken);  
    }

    /// <inheritdoc/>
    public virtual async ValueTask<TScope?> FindByIdAsync(string identifier, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0195), nameof(identifier));
        }

        return await FreeSql.Select<TScope>().Where(scope => scope.Id == identifier).ToOneAsync(cancellationToken);   
    }

    /// <inheritdoc/>
    public virtual async ValueTask<TScope?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(name))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0202), nameof(name));
        }

        return await FreeSql.Select<TScope>().Where(scope => scope.Name == name).ToOneAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TScope> FindByNamesAsync(
        ImmutableArray<string> names, CancellationToken cancellationToken)
    {
        if (names.Any(string.IsNullOrEmpty))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0203), nameof(names));
        }

        return ExecuteAsync(cancellationToken);

        async IAsyncEnumerable<TScope> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {

            var scopes = await FreeSql.Select<TScope>().Where(scope => names!.Contains(scope!.Name)).ToListAsync(cancellationToken);

            foreach (var scope in scopes)
            {
                yield return scope;
            }
        }
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TScope> FindByResourceAsync(
        string resource, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(resource))
        {
            throw new ArgumentException(SR.GetResourceString(SR.ID0062), nameof(resource));
        }

        return ExecuteAsync(cancellationToken);

        async IAsyncEnumerable<TScope> ExecuteAsync([EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var scopes = await FreeSql.Select<TScope>().Where(scope => scope.Resources!.Contains(resource)).ToListAsync(cancellationToken);

            foreach (var scope in scopes)
            {
                var resources = await GetResourcesAsync(scope, cancellationToken);
                if (resources.Contains(resource, StringComparer.Ordinal))
                {
                    yield return scope;
                }
            }
        }
    }

    /// <inheritdoc/>
    public virtual async ValueTask<TResult?> GetAsync<TState, TResult>(
        Func<IQueryable<TScope>, TState, IQueryable<TResult>> query,
        TState state, CancellationToken cancellationToken)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        return query(FreeSql.Select<TScope>().AsQueryable(), state).FirstOrDefault();
    }

    /// <inheritdoc/>
    public virtual ValueTask<string?> GetDescriptionAsync(TScope scope, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        return new(scope.Description);
    }

    /// <inheritdoc/>
    public virtual ValueTask<ImmutableDictionary<CultureInfo, string>> GetDescriptionsAsync(TScope scope, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        if (string.IsNullOrEmpty(scope.Descriptions))
        {
            return new(ImmutableDictionary.Create<CultureInfo, string>());
        }

        // Note: parsing the stringified descriptions is an expensive operation.
        // To mitigate that, the resulting object is stored in the memory cache.
        var key = string.Concat("42891062-8f69-43ba-9111-db7e8ded2553", "\x1e", scope.Descriptions);
        var descriptions = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                 .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(scope.Descriptions);
            var builder = ImmutableDictionary.CreateBuilder<CultureInfo, string>();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                var value = property.Value.GetString();
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                builder[CultureInfo.GetCultureInfo(property.Name)] = value;
            }

            return builder.ToImmutable();
        })!;

        return new(descriptions);
    }

    /// <inheritdoc/>
    public virtual ValueTask<string?> GetDisplayNameAsync(TScope scope, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        return new(scope.DisplayName);
    }

    /// <inheritdoc/>
    public virtual ValueTask<ImmutableDictionary<CultureInfo, string>> GetDisplayNamesAsync(TScope scope, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        if (string.IsNullOrEmpty(scope.DisplayNames))
        {
            return new(ImmutableDictionary.Create<CultureInfo, string>());
        }

        // Note: parsing the stringified display names is an expensive operation.
        // To mitigate that, the resulting object is stored in the memory cache.
        var key = string.Concat("e17d437b-bdd2-43f3-974e-46d524f4bae1", "\x1e", scope.DisplayNames);
        var names = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                 .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(scope.DisplayNames);
            var builder = ImmutableDictionary.CreateBuilder<CultureInfo, string>();

            foreach (var property in document.RootElement.EnumerateObject())
            {
                var value = property.Value.GetString();
                if (string.IsNullOrEmpty(value))
                {
                    continue;
                }

                builder[CultureInfo.GetCultureInfo(property.Name)] = value;
            }

            return builder.ToImmutable();
        })!;

        return new(names);
    }

    /// <inheritdoc/>
    public virtual ValueTask<string?> GetIdAsync(TScope scope, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        return new(scope.Id);
    }

    /// <inheritdoc/>
    public virtual ValueTask<string?> GetNameAsync(TScope scope, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        return new(scope.Name);
    }

    /// <inheritdoc/>
    public virtual ValueTask<ImmutableDictionary<string, JsonElement>> GetPropertiesAsync(TScope scope, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        if (string.IsNullOrEmpty(scope.Properties))
        {
            return new(ImmutableDictionary.Create<string, JsonElement>());
        }

        // Note: parsing the stringified properties is an expensive operation.
        // To mitigate that, the resulting object is stored in the memory cache.
        var key = string.Concat("78d8dfdd-3870-442e-b62e-dc9bf6eaeff7", "\x1e", scope.Properties);
        var properties = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                 .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(scope.Properties);
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
    public virtual ValueTask<ImmutableArray<string>> GetResourcesAsync(TScope scope, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        if (string.IsNullOrEmpty(scope.Resources))
        {
            return new(ImmutableArray.Create<string>());
        }

        // Note: parsing the stringified resources is an expensive operation.
        // To mitigate that, the resulting array is stored in the memory cache.
        var key = string.Concat("b6148250-aede-4fb9-a621-07c9bcf238c3", "\x1e", scope.Resources);
        var resources = Cache.GetOrCreate(key, entry =>
        {
            entry.SetPriority(CacheItemPriority.High)
                 .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            using var document = JsonDocument.Parse(scope.Resources);
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

        return new(resources);
    }

    /// <inheritdoc/>
    public virtual ValueTask<TScope> InstantiateAsync(CancellationToken cancellationToken)
    {
        try
        {
            return new(Activator.CreateInstance<TScope>());
        }

        catch (MemberAccessException exception)
        {
            return new(Task.FromException<TScope>(
                new InvalidOperationException(SR.GetResourceString(SR.ID0246), exception)));
        }
    }

    /// <inheritdoc/>
    public virtual IAsyncEnumerable<TScope> ListAsync(int? count, int? offset, CancellationToken cancellationToken)
    {
        var query = FreeSql.Select<TScope>().OrderBy(scope => scope.Id);

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
        Func<IQueryable<TScope>, TState, IQueryable<TResult>> query,
        TState state, CancellationToken cancellationToken)
    {
        if (query is null)
        {
            throw new ArgumentNullException(nameof(query));
        }

        return query(FreeSql.Select<TScope>().AsQueryable(), state).AsAsyncEnumerable(cancellationToken);
    }

    /// <inheritdoc/>
    public virtual ValueTask SetDescriptionAsync(TScope scope, string? description, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        scope.Description = description;

        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask SetDescriptionsAsync(TScope scope,
        ImmutableDictionary<CultureInfo, string> descriptions, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        if (descriptions is not { Count: > 0 })
        {
            scope.Descriptions = null;

            return default;
        }

        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Indented = false
        });

        writer.WriteStartObject();

        foreach (var description in descriptions)
        {
            writer.WritePropertyName(description.Key.Name);
            writer.WriteStringValue(description.Value);
        }

        writer.WriteEndObject();
        writer.Flush();

        scope.Descriptions = Encoding.UTF8.GetString(stream.ToArray());

        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask SetDisplayNameAsync(TScope scope, string? name, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        scope.DisplayName = name;

        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask SetDisplayNamesAsync(TScope scope,
        ImmutableDictionary<CultureInfo, string> names, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        if (names is not { Count: > 0 })
        {
            scope.DisplayNames = null;

            return default;
        }

        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Indented = false
        });

        writer.WriteStartObject();

        foreach (var name in names)
        {
            writer.WritePropertyName(name.Key.Name);
            writer.WriteStringValue(name.Value);
        }

        writer.WriteEndObject();
        writer.Flush();

        scope.DisplayNames = Encoding.UTF8.GetString(stream.ToArray());

        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask SetNameAsync(TScope scope, string? name, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        scope.Name = name;

        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask SetPropertiesAsync(TScope scope,
        ImmutableDictionary<string, JsonElement> properties, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        if (properties is not { Count: > 0 })
        {
            scope.Properties = null;

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

        scope.Properties = Encoding.UTF8.GetString(stream.ToArray());

        return default;
    }

    /// <inheritdoc/>
    public virtual ValueTask SetResourcesAsync(TScope scope, ImmutableArray<string> resources, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        if (resources.IsDefaultOrEmpty)
        {
            scope.Resources = null;

            return default;
        }

        using var stream = new MemoryStream();
        using var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            Indented = false
        });

        writer.WriteStartArray();

        foreach (var resource in resources)
        {
            writer.WriteStringValue(resource);
        }

        writer.WriteEndArray();
        writer.Flush();

        scope.Resources = Encoding.UTF8.GetString(stream.ToArray());

        return default;
    }

    /// <inheritdoc/>
    public virtual async ValueTask UpdateAsync(TScope scope, CancellationToken cancellationToken)
    {
        if (scope is null)
        {
            throw new ArgumentNullException(nameof(scope));
        }

        await FreeSql.Update<TScope>().SetSource(scope).ExecuteAffrowsAsync(cancellationToken);
    }
}
