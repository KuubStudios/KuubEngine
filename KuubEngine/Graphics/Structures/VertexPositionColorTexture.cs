// <copyright file="VertexPositionColorTexture.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics.Structures
{
    /// <summary>
    /// Vertex with a position, color and uv component
    /// </summary>
    public struct VertexPositionColorTexture
    {
        /// <summary>
        /// Vertex Declaration
        /// </summary>
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, 3, VertexAttribPointerType.Float, 0),
            new VertexElement(1, 4, VertexAttribPointerType.Float, 3 * Marshal.SizeOf<float>()),
            new VertexElement(2, 2, VertexAttribPointerType.Float, 7 * Marshal.SizeOf<float>()));

        /// <summary>
        /// Gets the position
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// Gets the color
        /// </summary>
        public Color4 Color { get; }

        /// <summary>
        /// Gets the texture coordinates
        /// </summary>
        public Vector2 UV { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPositionColorTexture"/> struct.
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="color">Color</param>
        /// <param name="uv">UV</param>
        public VertexPositionColorTexture(Vector3 position, Color4 color, Vector2 uv)
        {
            Position = position;
            Color = color;
            UV = uv;
        }
    }
}