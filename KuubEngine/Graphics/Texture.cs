using System;
using System.Diagnostics;

using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics {
    public abstract class Texture : IDisposable {
        public static Texture Current { get; private set; }

        private int id;
        public int ID {
            get {
                if(id == 0) GL.GenTextures(1, out id);
                return id;
            }
        }

        public bool Loaded { get; private set; }

        public TextureTarget TextureTarget { get; private set; }
        public TextureUnit TextureUnit { get; private set; }

        /// <summary>
        ///     Width of the texture in pixels
        /// </summary>
        public int Width { get; private set; }

        /// <summary>
        ///     Height of the texture in pixels
        /// </summary>
        public int Height { get; private set; }

        /// <summary>
        ///     Depth of the texture in pixels
        /// </summary>
        public int Depth { get; private set; }

        protected Texture(TextureTarget target, TextureUnit unit, int width, int height, int depth = 1) {
            TextureTarget = target;
            TextureUnit = unit;
            Width = width;
            Height = height;
            Depth = depth;

            Invalidate();
        }

        protected Texture(TextureTarget target, int width, int height, int depth = 1) : this(target, TextureUnit.Texture0, width, height, depth) {}

#if DEBUG
        ~Texture() {
            Debug.Assert(id == 0, this + " leaked!");
        }
#endif

        public virtual void Dispose() {
            if(id != 0) {
                GL.DeleteTexture(id);
                id = 0;
            }

            Loaded = false;
        }

        protected abstract void Load();

        public void Bind() {
            if(Current == this) return;

            GL.ActiveTexture(TextureUnit);
            GL.BindTexture(TextureTarget, ID);
            Current = this;

            if(!Loaded) {
                Load();
                Loaded = true;
            }
        }

        public void Invalidate() {
            Loaded = false;
        }
    }
}