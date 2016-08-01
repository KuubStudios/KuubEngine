// <copyright file="VertexPositionTexture.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics.Structures
{
    /// <summary>
    /// Vertex with a position and uv component
    /// </summary>
    public struct VertexPositionTexture
    {
        /// <summary>
        /// Vertex Declaration
        /// </summary>
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, 3, VertexAttribPointerType.Float, 0),
            new VertexElement(1, 2, VertexAttribPointerType.Float, 3 * Marshal.SizeOf<float>()));

        /// <summary>
        /// Gets the position
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// Gets the texture coordinates
        /// </summary>
        public Vector2 UV { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPositionTexture"/> struct.
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="uv">UV</param>
        public VertexPositionTexture(Vector3 position, Vector2 uv)
        {
            Position = position;
            UV = uv;
        }
    }
}