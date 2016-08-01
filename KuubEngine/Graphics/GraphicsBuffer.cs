// <copyright file="GraphicsBuffer.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.Runtime.InteropServices;
using KuubEngine.Graphics.Structures;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics
{
    /// <summary>
    /// Managed wrapper for OpenGL Buffers
    /// </summary>
    public class GraphicsBuffer : IBindable
    {
        private int id;

        /// <inheritdoc/>
        public int ID
        {
            get
            {
                if (id == 0)
                {
                    GL.GenBuffers(1, out id);
                }

                return id;
            }
        }

        /// <summary>
        /// Gets the length of the data
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Gets the <see cref="BufferTarget"/>
        /// </summary>
        public BufferTarget Target { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphicsBuffer"/> class.
        /// </summary>
        /// <param name="target"><see cref="BufferTarget"/></param>
        public GraphicsBuffer(BufferTarget target)
        {
            Target = target;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("GraphicsBuffer {0} {1}", Target, id);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (id != 0)
            {
                GL.DeleteBuffer(id);
                id = 0;
            }
        }

        /// <inheritdoc/>
        public void Bind()
        {
            GL.BindBuffer(Target, ID);
        }

        /// <inheritdoc/>
        public void Unbind()
        {
            GL.BindBuffer(Target, 0);
        }

        /// <summary>
        /// Set buffer data
        /// </summary>
        /// <typeparam name="T">Type of element</typeparam>
        /// <param name="data">Actual data to buffer</param>
        /// <param name="usage"><see cref="BufferUsageHint"/></param>
        public void SetData<T>(T[] data, BufferUsageHint usage = BufferUsageHint.StaticDraw) where T : struct
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }

            Length = data.Length;

            Bind();
            GL.BufferData(Target, new IntPtr(Length * Marshal.SizeOf(typeof(T))), data, usage);
            Unbind();
        }
    }
}