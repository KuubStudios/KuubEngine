using System;
using System.Diagnostics;

namespace KuubEngine.Content {
    public abstract class Asset : IDisposable {
        public bool Loaded { get; protected set; }

#if DEBUG
        ~Asset() {
            Debug.Assert(Loaded == false, this + " didn't unload properly!");
        }
#endif

        public virtual void Load(string path) {
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