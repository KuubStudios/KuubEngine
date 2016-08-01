// <copyright file="Texture.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics
{
    /// <summary>
    /// Managed wrapper for OpenGL Texture
    /// </summary>
    public abstract class Texture : IBindable
    {
        private int id;

        /// <inheritdoc/>
        public int ID
        {
            get
            {
                if (id == 0)
                {
                    GL.GenTextures(1, out id);
                }

                return id;
            }
        }

        /// <summary>
        /// Gets the TextureTarget
        /// </summary>
        public TextureTarget TextureTarget { get; }

        /// <summary>
        /// Gets the TextureUnit
        /// </summary>
        public TextureUnit TextureUnit { get; }

        /// <summary>
        /// Gets or sets the width of the texture in pixels
        /// </summary>
        public int Width { get; protected set; }

        /// <summary>
        /// Gets or sets the height of the texture in pixels
        /// </summary>
        public int Height { get; protected set; }

        /// <summary>
        /// Gets or sets the depth of the texture
        /// </summary>
        public int Depth { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Texture"/> class.
        /// </summary>
        /// <param name="target">TextureTarget</param>
        /// <param name="unit">TextureUnit</param>
        /// <param name="width">Width in pixels</param>
        /// <param name="height">Height in pixels</param>
        /// <param name="depth">Depth</param>
        protected Texture(TextureTarget target, TextureUnit unit, int width, int height, int depth = 1)
        {
            TextureTarget = target;
            TextureUnit = unit;
            Width = width;
            Height = height;
            Depth = depth;
        }

        /// <inheritdoc/>
        public virtual void Dispose()
        {
            if (id != 0)
            {
                GL.DeleteTexture(id);
                id = 0;
            }
        }

        /// <inheritdoc/>
        public void Bind()
        {
            GL.BindTexture(TextureTarget, ID);
        }

        /// <inheritdoc/>
        public void Unbind()
        {
            GL.BindTexture(TextureTarget, 0);
        }

        /// <summary>
        /// Load actual image data into memory
        /// </summary>
        protected abstract void Load();
    }
}