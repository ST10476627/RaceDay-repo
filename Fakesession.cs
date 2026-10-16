using Microsoft.AspNetCore.Http;
using RaceDay__unitTest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaceDay__unitTest
{// start of namespace
    public class Fakesession: ISession
    {// start of class
     // stores the session values in memory
        private readonly Dictionary<string, byte[]> store = new Dictionary<string, byte[]>();

        public string Id => "test-session";
        public bool IsAvailable => true;
        public IEnumerable<string> Keys => store.Keys;

        public void Clear() => store.Clear();
        public void Remove(string key) => store.Remove(key);
        public void Set(string key, byte[] value) => store[key] = value;
        public bool TryGetValue(string key, out byte[]? value) => store.TryGetValue(key, out value);
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    }// end of class
}// end of namespace
