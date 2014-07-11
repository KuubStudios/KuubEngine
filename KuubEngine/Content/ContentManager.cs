using System;
using System.Collections.Generic;

using KuubEngine.Utility;

namespace KuubEngine.Content {
    public class ContentManager {
        private readonly Dictionary<string, Asset> loadedAssets = new Dictionary<string, Asset>();

        public string BasePath { get; set; }

        public ContentManager(string basepath = "") {
            BasePath = basepath;
        }

        public T Load<T>(string path, bool async = true) where T : Asset {
            path = BasePath + path;

            Asset existing;
            if(loadedAssets.TryGetValue(path, out existing)) {
                if(existing is T) return (T)existing;
                throw new ContentLoadException(path + " is already loaded as a different type in this ContentManager!");
            }

            T asset = (T)Activator.CreateInstance(typeof(T));
            asset.Load(path);

            loadedAssets[path] = asset;

            return asset;
        }

        public void Unload() {
            foreach(var kv in loadedAssets) if(kv.Value != null && kv.Value.Loaded) kv.Value.Unload();
            loadedAssets.Clear();
        }
    }
}