// <copyright file="VertexArray.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using KuubEngine.Graphics.Structures;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics
{
    /// <summary>
    /// Managed wrapper for OpenGL Vertex Arrays
    /// </summary>
    public class VertexArray : IBindable
    {
        private int id;

        /// <inheritdoc/>
        public int ID
        {
            get
            {
                if (id == 0)
                {
                    GL.GenVertexArrays(1, out id);
                }

                return id;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("VertexArray {0}", id);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (id != 0)
            {
                GL.DeleteVertexArray(id);
                id = 0;
            }
        }

        /// <inheritdoc/>
        public void Bind()
        {
            GL.BindVertexArray(ID);
        }

        /// <inheritdoc/>
        public void Unbind()
        {
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Bind the specified <see cref="GraphicsBuffer"/> to this Vertex Array
        /// </summary>
        /// <param name="buffer"><see cref="GraphicsBuffer"/> to bind</param>
        public void BindBuffer(GraphicsBuffer buffer)
        {
            Bind();
            buffer.Bind();
        }

        /// <summary>
        /// Bind the specified <see cref="GraphicsBuffer"/> to this Vertex Array with specified parameters
        /// </summary>
        /// <param name="buffer"><see cref="GraphicsBuffer"/> to bind</param>
        /// <param name="index">Index of the Vertex Attribute to bind</param>
        /// <param name="size">Number of components per vertex atribute. Must be 1-4</param>
        /// <param name="type"><see cref="VertexAttribPointerType"/></param>
        /// <param name="normalized">Normalized</param>
        /// <param name="stride">Byte offset between consecutive vertex attributes</param>
        /// <param name="offset">Offset</param>
        public void BindBuffer(GraphicsBuffer buffer, int index, int size, VertexAttribPointerType type, bool normalized, int stride, int offset)
        {
            BindBuffer(buffer);
            GL.EnableVertexAttribArray(index);
            GL.VertexAttribPointer(index, size, type, normalized, stride, offset);
        }

        /// <summary>
        /// Bind the specified <see cref="GraphicsBuffer"/> to this Vertex Array with specified <see cref="VertexDeclaration"/>
        /// </summary>
        /// <param name="buffer"><see cref="GraphicsBuffer"/> to bind</param>
        /// <param name="vertexDeclaration"><see cref="VertexDeclaration"/> to use for binding</param>
        public void BindBuffer(GraphicsBuffer buffer, VertexDeclaration vertexDeclaration)
        {
            for (int i = 0; i < vertexDeclaration.Elements.Length; i++)
            {
                VertexElement element = vertexDeclaration.Elements[i];
                BindBuffer(buffer, element.Index, element.Size, element.Type, element.Normalized, vertexDeclaration.Stride, element.Offset);
            }
        }
    }
}