using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Linq;
using MongoDB.Driver;

namespace MongoDB.ReactiveChangeStream;

/// <summary>
/// Reactive Extension for MongoDB C# Driver Change Streams
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Create and start a Reactive Watcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="database"></param>
    /// <param name="collectionName"></param>
    /// <param name="filter"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IObservable<ChangeStreamDocument<T>> CreateObservableChangeStream<T>(this IMongoDatabase database,
        string collectionName,
        FilterDefinition<ChangeStreamDocument<T>> filter,
        Action<ChangeStreamOptions> options = null)
    {
        var changeStreamOptions = new ChangeStreamOptions();
        options?.Invoke(changeStreamOptions);

        return Observable.Defer(() =>
        {
            return Observable.Create<IEnumerable<ChangeStreamDocument<T>>>(async (observer, ct) =>
            {
                try
                {
                    var collection = database.GetCollection<T>(collectionName);
                    var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<T>>()
                        .Match(filter);

                    var changeQueueStream =
                        await collection.WatchAsync(pipeline, changeStreamOptions, ct);

                    do
                    {
                        observer.OnNext(changeQueueStream.Current);
                    } while (await changeQueueStream.MoveNextAsync(ct));
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            });
        }).SelectMany(x => x);
    }

    /// <summary>
    /// Create and start a Reactive Watcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="database"></param>
    /// <param name="collectionName"></param>
    /// <param name="filter"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IObservable<ChangeStreamDocument<T>> CreateObservableChangeStream<T>(this IMongoDatabase database,
        string collectionName,
        Expression<Func<ChangeStreamDocument<T>, bool>> filter,
        Action<ChangeStreamOptions> options = null)
    {
        var changeStreamOptions = new ChangeStreamOptions();
        options?.Invoke(changeStreamOptions);

        return Observable.Defer(() =>
        {
            return Observable.Create<IEnumerable<ChangeStreamDocument<T>>>(async (observer, ct) =>
            {
                try
                {
                    var collection = database.GetCollection<T>(collectionName);
                    var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<T>>()
                        .Match(filter);

                    var changeQueueStream =
                        await collection.WatchAsync(pipeline, changeStreamOptions, ct);

                    do
                    {
                        observer.OnNext(changeQueueStream.Current);
                    } while (await changeQueueStream.MoveNextAsync(ct));
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            });
        }).SelectMany(x => x);
    }

    /// <summary>
    /// Create and start a Reactive Watcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="filter"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IObservable<ChangeStreamDocument<T>> CreateObservableChangeStream<T>(
        this IMongoCollection<T> collection,
        FilterDefinition<ChangeStreamDocument<T>> filter,
        Action<ChangeStreamOptions> options = null)
    {
        var changeStreamOptions = new ChangeStreamOptions();
        options?.Invoke(changeStreamOptions);

        return Observable.Defer(() =>
        {
            return Observable.Create<IEnumerable<ChangeStreamDocument<T>>>(async (observer, ct) =>
            {
                try
                {
                    var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<T>>()
                        .Match(filter);

                    var changeQueueStream =
                        await collection.WatchAsync(pipeline, changeStreamOptions, ct);

                    do
                    {
                        observer.OnNext(changeQueueStream.Current);
                    } while (await changeQueueStream.MoveNextAsync(ct));
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            });
        }).SelectMany(x => x);
    }

    /// <summary>
    /// Create and start a Reactive Watcher
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="filter"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public static IObservable<ChangeStreamDocument<T>> CreateObservableChangeStream<T>(
        this IMongoCollection<T> collection,
        Expression<Func<ChangeStreamDocument<T>, bool>> filter,
        Action<ChangeStreamOptions> options = null)
    {
        var changeStreamOptions = new ChangeStreamOptions();
        options?.Invoke(changeStreamOptions);

        return Observable.Defer(() =>
        {
            return Observable.Create<IEnumerable<ChangeStreamDocument<T>>>(async (observer, ct) =>
            {
                try
                {
                    var pipeline = new EmptyPipelineDefinition<ChangeStreamDocument<T>>()
                        .Match(filter);

                    var changeQueueStream =
                        await collection.WatchAsync(pipeline, changeStreamOptions, ct);

                    do
                    {
                        observer.OnNext(changeQueueStream.Current);
                    } while (await changeQueueStream.MoveNextAsync(ct));
                }
                catch (Exception ex)
                {
                    observer.OnError(ex);
                }
            });
        }).SelectMany(x => x);
    }
}