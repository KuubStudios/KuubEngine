using System;

using KuubEngine.Diagnostics;

using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics {
    public static class GraphicsDevice {
        public static bool NvidiaCard { get; private set; }
        public static Version GLVersion { get; private set; }

        public static bool SupportsGL3 {
            get { return GLVersion.Major >= 3; }
        }

        public static void Initialize() {
            Log.Info("Checking Graphics Device");

            string vendor = GL.GetString(StringName.Vendor);
            NvidiaCard = vendor.StartsWith("NVIDIA", StringComparison.OrdinalIgnoreCase);
            Log.Info("Vendor: {0}", vendor);
            Log.Info("Renderer: {0}", GL.GetString(StringName.Renderer));

            string version = GL.GetString(StringName.Version);
            try {
                GLVersion = new Version(version);
            } catch(Exception) {
                GLVersion = new Version(GL.GetInteger(GetPName.MajorVersion), GL.GetInteger(GetPName.MinorVersion));
            }

            Log.Info("GL Version: {0} ({1})", GLVersion, GL.GetString(StringName.Version));
            Log.Info("GLSL: {0}", GL.GetString(StringName.ShadingLanguageVersion));
        }

        public static bool Supports(string extension) {
            return GL.GetString(StringName.Extensions).Contains(extension);
        }
    }
}