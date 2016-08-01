// <copyright file="VertexPositionColor.cs" company="Kuub Studios">
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
    /// Vertex with a position and color component
    /// </summary>
    public struct VertexPositionColor
    {
        /// <summary>
        /// Vertex Declaration
        /// </summary>
        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, 3, VertexAttribPointerType.Float, 0),
            new VertexElement(1, 4, VertexAttribPointerType.Float, 3 * Marshal.SizeOf<float>()));

        /// <summary>
        /// Gets the position
        /// </summary>
        public Vector3 Position { get; }

        /// <summary>
        /// Gets the color
        /// </summary>
        public Color4 Color { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexPositionColor"/> struct.
        /// </summary>
        /// <param name="position">Position</param>
        /// <param name="color">Color</param>
        public VertexPositionColor(Vector3 position, Color4 color)
        {
            Position = position;
            Color = color;
        }
    }
}