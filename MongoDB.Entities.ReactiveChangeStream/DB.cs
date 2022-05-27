using System;
using System.Linq.Expressions;
using System.Threading;
using MongoDB.Driver;
using MongoDB.Entities.ReactiveChangeStream;

namespace MongoDB.Entities;

/// <summary>
///     DB Extension for MongoDB Entities
/// </summary>
public static class DBEx
{
    /// <summary>
    ///     Start a Reactive Watcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="filter">Predicate to filter out changes</param>
    /// <param name="options">Configure internal Watcher</param>
    /// <param name="ct">Cancellation Token</param>
    /// <returns></returns>
    public static IObservable<ChangeStreamDocument<T>> StartReactiveWatcher<T>(
        Expression<Func<ChangeStreamDocument<T>, bool>> filter = null,
        Action<Watcher<T>> options = null,
        CancellationToken ct = default)
        where T : IEntity
    {
        var watcher = DB.Watcher<T>(nameof(T));
        if (options == null)
            watcher.Start(EventType.Created | EventType.Deleted | EventType.Updated, filter, cancellation: ct);
        else options?.Invoke(watcher);

        return watcher.ToObservableChangeStream();
    }
}