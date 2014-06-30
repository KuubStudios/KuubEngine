namespace KuubEngine.Content {
    public interface IAsset {
        bool Loaded { get; set; }

        void Load();
        void Unload();
    }
}