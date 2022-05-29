using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Entities;

namespace MongoDB.Entities
{
    /// <summary>
    /// MongoDB Entities DB Context
    /// </summary>
    public partial class DBContext
    {
        internal ConcurrentDictionary<(string database, string name), Watcher<IEntity>>
            _watchers { get; private set; } =
            new ConcurrentDictionary<(string, string), Watcher<IEntity>>();

        /// <summary>
        /// Retrieves the 'change-stream' watcher instance for a given unique name. 
        /// If an instance for the name does not exist, it will return a new instance. 
        /// If an instance already exists, that instance will be returned.
        /// </summary>
        /// <typeparam name="T">The entity type to get a watcher for</typeparam>
        /// <param name="name">A unique name for the watcher of this entity type. Names can be duplicate among different entity types.</param>
        public Watcher<T> Watcher<T>(string database, string name) where T : IEntity
        {
            if (_watchers.TryGetValue((database, name), out Watcher<IEntity> _watcher))
                return _watcher as Watcher<T>;

            var watcher = DB.Watcher<T>($"{database}_{name}");
            _watchers.TryAdd((database, name), watcher as Watcher<IEntity>);

            return watcher;
        }

        /// <summary>
        /// Returns all the watchers for a given entity type
        /// </summary>
        /// <typeparam name="T">The entity type to get the watcher of</typeparam>
        public IEnumerable<Watcher<T>> Watchers<T>() where T : IEntity => _watchers
            .Select(x => x.Value as Watcher<T>);

        /// <summary>
        /// Returns all the watchers for a given entity type of specific database
        /// </summary>
        /// <typeparam name="T">The entity type to get the watcher of</typeparam>
        /// <param name="database">The database name</param>
        /// <returns></returns>
        public IEnumerable<Watcher<T>> Watchers<T>(string database) where T : IEntity =>
            _watchers
                .Where(x => x.Key.database == database)
                .Select(x => x.Value as Watcher<T>);
    }
}