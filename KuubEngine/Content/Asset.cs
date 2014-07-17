using System;
using System.Diagnostics;
using System.IO;

using Microsoft.SqlServer.Server;

namespace KuubEngine.Content {
    public abstract class Asset : IDisposable {
        public bool Loaded { get; protected set; }

#if DEBUG
        ~Asset() {
            Debug.Assert(Loaded == false, this + " didn't unload properly!");
        }
#endif

        public virtual void Load(string path) {
            Load(File.Open(path, FileMode.Open), Path.GetDirectoryName(path));
        }

        public virtual void Load(Stream file, string root) {
            Loaded = true;
        }

        public virtual void Unload() {
            Loaded = false;
        }

        public void Dispose() {
            if(Loaded) Unload();
        }
    }
}