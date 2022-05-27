using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using MongoDB.Driver;

namespace MongoDB.Entities.ReactiveChangeStream;

/// <summary>
///     Reactive Extension for MongoDB Entities Change Streams
/// </summary>
public static class Extensions
{
    /// <summary>
    ///     Convert Watcher OnChangesCSD Event to IObservable.
    ///     Make sure to start the Watcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="watcher">Watcher</param>
    /// <returns></returns>
    public static IObservable<ChangeStreamDocument<T>> ToObservableChangeStream<T>(this Watcher<T> watcher)
        where T : IEntity
    {
        return Observable
            .FromEvent<IEnumerable<ChangeStreamDocument<T>>>(
                x => watcher.OnChangesCSD += x,
                x => watcher.OnChangesCSD -= x)
            .SelectMany(x => x);
    }

    /// <summary>
    ///     Watch Current Entity for changes using its ID
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    public static IObservable<ChangeStreamDocument<T>> ObserveChanges<T>(this T entity,
        CancellationToken ct = default) where T : IEntity
    {
        var watcher = DB.Watcher<T>(nameof(T));

        watcher.Start(EventType.Created | EventType.Deleted | EventType.Updated,
            x => x.DocumentKey == entity.ID,
            cancellation: ct);

        return watcher.ToObservableChangeStream();
    }
}