// <copyright file="VertexElement.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics.Structures
{
    /// <summary>
    /// Describes a single vertex attribute
    /// </summary>
    public class VertexElement
    {
        /// <summary>
        /// Gets the index
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Gets the amount of components
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Gets the <see cref="VertexAttribPointerType"/>
        /// </summary>
        public VertexAttribPointerType Type { get; }

        /// <summary>
        /// Gets the offset
        /// </summary>
        public int Offset { get; }

        /// <summary>
        /// Gets a value indicating whether this element should be normalized
        /// </summary>
        public bool Normalized { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexElement"/> class.
        /// </summary>
        /// <param name="index">Index</param>
        /// <param name="size">Amount of components</param>
        /// <param name="type"><see cref="VertexAttribPointerType"/></param>
        /// <param name="offset">Offset</param>
        /// <param name="normalized">Normalize values</param>
        public VertexElement(int index, int size, VertexAttribPointerType type, int offset, bool normalized = false)
        {
            Index = index;
            Size = size;
            Type = type;
            Offset = offset;
            Normalized = normalized;
        }
    }
}