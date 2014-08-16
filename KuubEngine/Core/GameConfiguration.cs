using System.IO;

using KuubEngine.Diagnostics;

using Newtonsoft.Json;

using OpenTK;
using OpenTK.Graphics;

namespace KuubEngine.Core {
    public class GameConfiguration {
        /// <summary>
        ///     Default fallback GameConfiguration, this shouldn't be used in an actual game
        /// </summary>
        public static readonly GameConfiguration Default;

        /// <summary>
        ///     Width of the GameWindow
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        ///     Height of the GameWindow
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        ///     GraphicsMode passed to the GameWindow constructor
        /// </summary>
        [JsonIgnore]
        public GraphicsMode GraphicsMode { get; set; }

        /// <summary>
        ///     GameWindowFlags passed to the GameWindow constructor
        /// </summary>
        public GameWindowFlags GameWindowFlags { get; set; }

        /// <summary>
        ///     DisplayDevice passed to the GameWindow constructor
        /// </summary>
        [JsonIgnore]
        public DisplayDevice DisplayDevice { get; set; }

        /// <summary>
        ///     Major OpenGL version passed to the GameWindow constructor
        /// </summary>
        public int Major { get; set; }

        /// <summary>
        ///     Minor OpenGL version passed to the GameWindow constructor
        /// </summary>
        public int Minor { get; set; }

        /// <summary>
        ///     GraphicsContextFlags passed to the GameWindow constructor
        /// </summary>
        [JsonIgnore]
        public GraphicsContextFlags GraphicsContextFlags { get; set; }

        /// <summary>
        ///     GameWindow caption, visible in windowed mode and taskbar
        /// </summary>
        [JsonIgnore]
        public string Caption { get; set; }

        /// <summary>
        ///     Internal game name which is passed to CrashReporter
        /// </summary>
        [JsonIgnore]
        public string Name { get; set; }

        static GameConfiguration() {
            Default = new GameConfiguration();
        }

        public GameConfiguration(int width, int height, string name, bool fullscreen = false) {
            Width = width;
            Height = height;
            GraphicsMode = GraphicsMode.Default;
            GameWindowFlags = fullscreen ? GameWindowFlags.Fullscreen : GameWindowFlags.FixedWindow;
            DisplayDevice = DisplayDevice.Default;
            GraphicsContextFlags = GraphicsContextFlags.ForwardCompatible;
            Major = 1;
            Minor = 5;
            Name = name;
            Caption = name;
        }

        public GameConfiguration(string name) : this(800, 600, name) {}

        public GameConfiguration() : this("KuubGame") {}

        public GameConfiguration LoadFile(string file) {
            if (!File.Exists(file)) {
                Log.Warn("Settings file {0} doesn't exist, creating default one", file);
                Default.SaveFile(file);
                return Default;
            }

            JsonConvert.PopulateObject(File.ReadAllText(file), this);
            return this;
        }

        public void SaveFile(string file) { // TODO: handle file saving exceptions
            using(FileStream fs = File.Open(file, FileMode.Create)) {
                using(StreamWriter sw = new StreamWriter(fs)) {
                    using(JsonWriter jw = new JsonTextWriter(sw)) {
                        jw.Formatting = Formatting.Indented;

                        JsonSerializer serializer = new JsonSerializer();
                        serializer.Serialize(jw, this);
                    }
                }
            }
        }
    }
}