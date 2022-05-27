<img src="https://raw.githubusercontent.com/rainxh11/MongoDB.ReactiveChangeStream/master/Assets/rxmongo.svg" width="300">

|Package Name|Version|Downloads|
|-|-|-|
|MongoDB.ReactiveChangeStream|[![Latest version](https://img.shields.io/nuget/v/MongoDB.ReactiveChangeStream.svg)](https://www.nuget.org/packages/MongoDB.ReactiveChangeStream/)|![Downloads](https://img.shields.io/nuget/dt/MongoDB.ReactiveChangeStream.svg)|
|[MongoDB.Entities.ReactiveChangeStream](https://github.com/rainxh11/MongoDB.ReactiveChangeStream/tree/master/MongoDB.Entities.ReactiveChangeStream)|[![Latest version](https://img.shields.io/nuget/v/MongoDB.Entities.ReactiveChangeStream.svg)](https://www.nuget.org/packages/Entities.ReactiveChangeStream/)|![Downloads](https://img.shields.io/nuget/dt/MongoDB.Entities.ReactiveChangeStream.svg)|


# MongoDB.ReactiveChangeStream
Extension for [MongoDB C# Driver](https://www.nuget.org/packages/MongoDB.Driver/) to handle MongoDB Change Streams as an `IObservale` 
MongoDB Change Streams are a feature that allow the database to notify subscribers for any change, [*more about Change Streams*](https://www.mongodb.com/docs/manual/changeStreams/)
# Installation
**First**, install the [`MongoDB.ReactiveChangeStream`](https://www.nuget.org/packages/MongoDB.ReactiveChangeStream) Nuget package into your app
```powershell
PM> Install-Package MongoDB.ReactiveChangeStream
```

# Example Usage
```csharp
using System.Reactive.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.ReactiveChangeStream;

var client = new MongoClient("mongodb://localhost:27017/foo?replicaSet=rs0");

var database = client.GetDatabase("foo");

var collection = database.GetCollection<BsonDocument>("bar");

var changeObservable = collection.CreateObservableChangeStream(filter =>
    filter.OperationType == ChangeStreamOperationType.Delete ||
    filter.OperationType == ChangeStreamOperationType.Insert ||
    filter.OperationType == ChangeStreamOperationType.Update ||
    filter.OperationType == ChangeStreamOperationType.Replace);

changeObservable
    .Buffer(TimeSpan.FromSeconds(5))
    .Do(changes =>
    {
        // Do something
    })
    .Subscribe();
```
##### Note:
Make sure you connecting to a MongoDB instance with Replica Set Enabled, as Change Streams require it.
*read more about how to initialize a Replica Set [here...](https://www.mongodb.com/docs/manual/tutorial/convert-standalone-to-replica-set/)*
