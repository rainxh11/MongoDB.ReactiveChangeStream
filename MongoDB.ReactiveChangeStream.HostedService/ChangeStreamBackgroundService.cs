using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using MongoDB.Driver;

namespace MongoDB.ReactiveChangeStream.HostedService
{
    internal class ChangeStreamBackgroundService<T> : BackgroundService
    {
        private Func<IServiceProvider, CancellationToken, IObservable<ChangeStreamDocument<T>>,
                IObservable<ChangeStreamDocument<T>>>
            _function;

        private IServiceProvider _provider;
        private IMongoDatabase _database;
        private readonly string _collectionName;

        public ChangeStreamBackgroundService(
            string collectionName,
            IMongoDatabase database,
            IServiceProvider provider,
            Func<IServiceProvider, CancellationToken, IObservable<ChangeStreamDocument<T>>,
                    IObservable<ChangeStreamDocument<T>>>
                changeFunction)
        {
            _function = changeFunction;
            _provider = provider;
            _collectionName = collectionName;
            _database = database;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var observable = _database
                .GetCollection<T>(_collectionName)
                .CreateObservableChangeStream(x => true);
            await _function(_provider, stoppingToken, observable);
        }
    }
}