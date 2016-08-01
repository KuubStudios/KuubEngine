// <copyright file="Texture2D.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics
{
    /// <summary>
    /// Managed wrapper for OpenGL Texture2D
    /// </summary>
    public class Texture2D : Texture
    {
        private int actualSize;

        /// <summary>
        /// Gets the bitmap with data
        /// </summary>
        public Bitmap Bitmap { get; private set; }

        /// <summary>
        /// Gets or sets the texture filter for minifying operations
        /// </summary>
        public TextureMinFilter MinFilter { get; set; }

        /// <summary>
        /// Gets or sets the texture filter for magnifying operations
        /// </summary>
        public TextureMagFilter MagFilter { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2D"/> class.
        /// </summary>
        /// <param name="bitmap">Bitmap</param>
        /// <param name="unit">TextureUnit</param>
        public Texture2D(Bitmap bitmap, TextureUnit unit = TextureUnit.Texture0) : base(TextureTarget.Texture2D, unit, bitmap.Width, bitmap.Height)
        {
            MinFilter = TextureMinFilter.Linear;
            MagFilter = TextureMagFilter.Nearest;

            SetData(bitmap);
            Load();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture2D"/> class
        /// </summary>
        /// <param name="path">Path to bitmap to load</param>
        public Texture2D(string path) : this(new Bitmap(path))
        {
        }

        /// <inheritdoc/>
        public override void Dispose()
        {
            base.Dispose();
            Bitmap.Dispose();
        }

        /// <inheritdoc/>
        protected override void Load()
        {
            GL.TexParameter(TextureTarget, TextureParameterName.TextureMinFilter, (int)MinFilter);
            GL.TexParameter(TextureTarget, TextureParameterName.TextureMagFilter, (int)MagFilter);

            BitmapData data = Bitmap.LockBits(new Rectangle(0, 0, Bitmap.Width, Bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL4.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            Bitmap.UnlockBits(data);
        }

        /// <summary>
        /// Load bitmap into memory
        /// </summary>
        /// <param name="bitmap">Bitmap to load</param>
        public void SetData(Bitmap bitmap)
        {
            actualSize = MathHelper.NextPowerOfTwo(Math.Max(bitmap.Width, bitmap.Height));
            if (actualSize == bitmap.Width && actualSize == bitmap.Height)
            {
                Bitmap = bitmap;
            }
            else
            {
                Bitmap = new Bitmap(actualSize, actualSize);

                for (int x = 0; x < Width; ++x)
                {
                    for (int y = 0; y < Height; ++y)
                    {
                        Bitmap.SetPixel(x, y, bitmap.GetPixel(x, y));
                    }
                }
            }
        }

        /// <summary>
        /// Get actual coordinates in texture after scaling to nearest power of two
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <returns>Adjusted coordinates</returns>
        public Vector2 GetCoords(float x, float y)
        {
            return new Vector2(x / actualSize, y / actualSize);
        }
    }
}