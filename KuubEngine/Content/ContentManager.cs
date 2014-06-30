using System;
using System.Collections.Generic;

namespace KuubEngine.Content {
    public class ContentLoadException : Exception {
        public ContentLoadException(string message) : base(message) {}
    }

    public class ContentManager {
        private readonly Dictionary<string, IAsset> loadedAssets = new Dictionary<string, IAsset>();

        public string BasePath { get; set; }

        public ContentManager(string basepath = "") {
            BasePath = basepath;
        }

        public T Load<T>(string path, bool async = true) where T : IAsset {
            path = BasePath + path;

            IAsset existing;
            if(loadedAssets.TryGetValue(path, out existing)) {
                if(existing is T) return (T)existing;
                throw new ContentLoadException(path + " is already loaded as a different type in this ContentManager!");
            }

            T asset = (T)Activator.CreateInstance(typeof(T), path);
            asset.Load();
            return asset;
        }

        public void Unload() {
            foreach(var kv in loadedAssets) if(kv.Value != null && kv.Value.Loaded) kv.Value.Unload();
            loadedAssets.Clear();
        }
    }
}