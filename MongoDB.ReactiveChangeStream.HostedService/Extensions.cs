using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace MongoDB.ReactiveChangeStream.HostedService;

public static class Extensions
{
    public static IServiceCollection AddMongoDBChangeStreamsBackgroundService<T>(
        this IServiceCollection services,
        string collectionName,
        Func<IServiceProvider, CancellationToken, IObservable<ChangeStreamDocument<T>>,
            IObservable<ChangeStreamDocument<T>>> changeFunction)
    {
        return services
            .AddSingleton<IHostedService, ChangeStreamBackgroundService<T>>(provider =>
            {
                var mongo = provider.GetService<IMongoDatabase>();
                return new ChangeStreamBackgroundService<T>(collectionName, mongo, provider, changeFunction);
            });
    }

    public static IServiceCollection AddMongoDB(this IServiceCollection services,
        Func<IMongoDatabase> configure)
    {
        services.TryAddEnumerable(ServiceDescriptor.Singleton<IMongoDatabase>(x => configure()));
        return services;
    }
}