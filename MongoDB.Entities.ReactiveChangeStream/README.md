# MongoDB.Entities.ReactiveChangeStream
Extension for [MongoDB Entities Library](https://www.nuget.org/packages/MongoDB.Entities) to handle MongoDB Change Streams as an `IObservale` 
MongoDB Change Streams are a feature that allow the database to notify subscribers for any change, [*more about Change Streams*](https://www.mongodb.com/docs/manual/changeStreams/)
For MongoDB Official C# Driver use [`MongoDB.ReactiveChangeStream`](https://www.nuget.org/packages/MongoDB.ReactiveChangeStream) 

# [MongoDB.Entities](https://mongodb-entities.com/)
A light-weight .net standard library with barely any overhead that aims to simplify access to mongodb by abstracting the official driver while adding useful features on top of it resulting in an elegant API surface which produces beautiful, human friendly data access code.

# Installation
**First**, install the [`MongoDB.Entities.ReactiveChangeStream`](https://www.nuget.org/packages/MongoDB.Entities.ReactiveChangeStream) Nuget package into your app
```powershell
PM> Install-Package MongoDB.Entities.ReactiveChangeStream
```

# Example Usage
#### Create Reactive Change Stream directly:
```csharp
using System.Reactive.Linq;
using MongoDB.Driver;
using MongoDB.Entities;

DB.InitAsync("foo", 
    MongoClientSettings.FromConnectionString("mongodb://localhost:27017/foo?replicaSet=rs0"));

var changeObservable = DBEx.StartReactiveWatcher<Entity>();

changeObservable
    .Buffer(TimeSpan.FromSeconds(5))
    .Do(changes =>
    {
        // Do something
    })
    .Subscribe();
```

#### From a Watcher instance:
```csharp
using System.Reactive.Linq;
using MongoDB.Driver;
using MongoDB.Entities;

DB.InitAsync("foo", 
    MongoClientSettings.FromConnectionString("mongodb://localhost:27017/foo?replicaSet=rs0"));

var watcher = DB.Watcher<Entity>("watcher");

watcher.Start(EventType.Created | EventType.Deleted | EventType.Updated);

watcher
    .ToObservableChangeStream();
    .Buffer(TimeSpan.FromSeconds(5))
    .Do(changes =>
    {
        // Do something
    })
    .Subscribe();
```

#### Watch individual Entities changes filtered by their IDs:
```csharp
using System.Reactive.Linq;
using MongoDB.Driver;
using MongoDB.Entities;
using MongoDB.Entities.ReactiveChangeStream;

var foo = DB.Entity<Foo>();

foo
    .ObserveChanges()
    .Buffer(TimeSpan.FromSeconds(5))
    .Do(changes =>
    {
        // Do something
    })
    .Subscribe();
```

```csharp
class Foo : Entity
{
    /*
     */
}
```
##### Note:
Make sure you connecting to a MongoDB instance with Replica Set Enabled, as Change Streams require it.
*read more about how to initialize a Replica Set [here...](https://www.mongodb.com/docs/manual/tutorial/convert-standalone-to-replica-set/)*
