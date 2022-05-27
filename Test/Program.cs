using System.Reactive.Linq;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Entities.ReactiveChangeStream;

/*var client = new MongoClient("mongodb://localhost:27017/foo?replicaSet=rs0");

var database = client.GetDatabase("foo");

var collection = database.GetCollection<BsonDocument>("bar");

var changeObservable = collection.CreateObservableChangeStream(filter =>
    filter.OperationType == ChangeStreamOperationType.Delete ||
    filter.OperationType == ChangeStreamOperationType.Insert ||
    filter.OperationType == ChangeStreamOperationType.Update ||
    filter.OperationType == ChangeStreamOperationType.Replace);*/

DB.InitAsync("foo", MongoClientSettings.FromConnectionString("mongodb://localhost:27017/foo?replicaSet=rs0"));

//var changeObservable = DBEx.StartReactiveWatcher<Entity>();

//var watcher = DB.Watcher<Entity>("watcher");

//watcher.Start(EventType.Created | EventType.Deleted | EventType.Updated);

//var changeObservable = watcher.ToObservableChangeStream();

var foo = DB.Entity<Foo>();


foo
    .ObserveChanges()
    .Buffer(TimeSpan.FromSeconds(5))
    .Do(changes =>
    {
        // Do something
    })
    .Subscribe();

internal class Foo : Entity
{
    /*
     *
     */
}