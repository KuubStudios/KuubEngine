// <copyright file="VertexDeclaration.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Runtime.InteropServices;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics.Structures
{
    /// <summary>
    /// Describes vertex attribute layouts
    /// </summary>
    public class VertexDeclaration
    {
        /// <summary>
        /// Gets the <see cref="VertexElement"/> this declaration is made up of
        /// </summary>
        public VertexElement[] Elements { get; }

        /// <summary>
        /// Gets the stride
        /// </summary>
        public int Stride { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VertexDeclaration"/> class.
        /// </summary>
        /// <param name="elements">Elements this declaration consists of</param>
        public VertexDeclaration(params VertexElement[] elements)
        {
            Elements = elements;

            Stride = 0;
            for (int i = 0; i < elements.Length; i++)
            {
                Stride += GetElementSize(elements[i]);
            }
        }

        private int GetElementSize(VertexElement element)
        {
            switch (element.Type)
            {
                case VertexAttribPointerType.Byte:
                case VertexAttribPointerType.UnsignedByte:
                    return element.Size * Marshal.SizeOf<byte>();
                case VertexAttribPointerType.Double:
                    return element.Size * Marshal.SizeOf<double>();
                case VertexAttribPointerType.Float:
                    return element.Size * Marshal.SizeOf<float>();
                case VertexAttribPointerType.Int:
                case VertexAttribPointerType.UnsignedInt:
                    return element.Size * Marshal.SizeOf<int>();
                case VertexAttribPointerType.Short:
                case VertexAttribPointerType.UnsignedShort:
                    return element.Size * Marshal.SizeOf<short>();
            }

            return 0;
        }
    }
}