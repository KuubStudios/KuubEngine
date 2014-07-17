using System;
using System.Drawing;

using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics {
    public class Texture2D : Texture {
        public static readonly Texture2D Blank;
        private readonly int actualSize;

        public Bitmap Bitmap { get; private set; }
        public TextureMinFilter MinFilter { get; set; }
        public TextureMagFilter MagFilter { get; set; }

        static Texture2D() {
            Bitmap blank = new Bitmap(1, 1);
            blank.SetPixel(0, 0, Color.White);
            Blank = new Texture2D(blank);
        }

        public Texture2D(Bitmap bitmap) : base(TextureTarget.Texture2D, bitmap.Width, bitmap.Height) {
            MinFilter = TextureMinFilter.LinearMipmapNearest;
            MagFilter = TextureMagFilter.Nearest;

            actualSize = MathHelper.NextPowerOfTwo(Math.Max(bitmap.Width, bitmap.Height));
            if(actualSize == bitmap.Width && actualSize == bitmap.Height) Bitmap = bitmap;
            else {
                Bitmap = new Bitmap(actualSize, actualSize);

                for(int x = 0; x < Width; ++x) for(int y = 0; y < Height; ++y) Bitmap.SetPixel(x, y, bitmap.GetPixel(x, y));
            }
        }

        public Texture2D(string path) : this(new Bitmap(path)) {}
        public Texture2D(int width, int height) : this(new Bitmap(width, height)) {}

        public override void Dispose() {
            base.Dispose();
            Bitmap.Dispose();
        }

        protected override void Load() {
            GL.TexParameter(TextureTarget, TextureParameterName.TextureMinFilter, (int)MinFilter);
            GL.TexParameter(TextureTarget, TextureParameterName.TextureMagFilter, (int)MagFilter);

            System.Drawing.Imaging.BitmapData data = Bitmap.LockBits(new Rectangle(0, 0, Bitmap.Width, Bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget, 0, PixelInternalFormat.Rgba, Bitmap.Width, Bitmap.Height, 0, PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            Bitmap.UnlockBits(data);
        }

        public Vector2 GetCoords(float x, float y) {
            return new Vector2(x / actualSize, y / actualSize);
        }
    }
}