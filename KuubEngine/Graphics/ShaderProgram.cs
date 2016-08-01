// <copyright file="ShaderProgram.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using KuubEngine.Graphics.Exceptions;
using NLog;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics
{
    /// <summary>
    /// Managed wrapper for OpenGL Shader Programs
    /// </summary>
    public class ShaderProgram : IBindable
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        private int id;

        /// <inheritdoc/>
        public int ID
        {
            get
            {
                if (id == 0)
                {
                    id = GL.CreateProgram();
                }

                return id;
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (id != 0)
            {
                GL.DeleteProgram(ID);
                id = 0;
            }
        }

        /// <inheritdoc/>
        public void Bind()
        {
            GL.UseProgram(ID);
        }

        /// <inheritdoc/>
        public void Unbind()
        {
            GL.UseProgram(0);
        }

        /// <summary>
        /// Link all attached <see cref="Shader"/>
        /// </summary>
        public void Link()
        {
            int status;

            GL.LinkProgram(ID);
            GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out status);
            if (status != 1)
            {
                throw new ShaderLinkException(GL.GetProgramInfoLog(ID));
            }

#if DEBUG
            GL.ValidateProgram(ID);
            GL.GetProgram(ID, GetProgramParameterName.ValidateStatus, out status);
            if (status != 1)
            {
                throw new ShaderValidateException(GL.GetProgramInfoLog(ID));
            }
#endif

            int length;
            GL.GetProgram(ID, GetProgramParameterName.InfoLogLength, out length);
            if (length > 1)
            {
                log.Warn("ShaderProgram {0} info log not empty:\n\t{1}", ID, GL.GetProgramInfoLog(ID));
            }

            int attributes;
            GL.GetProgram(ID, GetProgramParameterName.ActiveAttributes, out attributes);
            log.Debug("Program {0} linked with {1} attributes:", ID, attributes);
            for (int i = 0; i < attributes; i++)
            {
                int size;
                ActiveAttribType type;
                string attrib = GL.GetActiveAttrib(ID, i, out size, out type);
                log.Debug("\t{0}: {1} {2} {3}", i, size, type, attrib);
            }
        }

        /// <summary>
        /// Get location of the specified attribute
        /// </summary>
        /// <param name="name">Name of the attribute</param>
        /// <returns>Index of the attribute</returns>
        public int GetAttribLocation(string name)
        {
            int index = GL.GetAttribLocation(ID, name);
            if (index < 0)
            {
                throw new ArgumentException("Couldn't find attrib location", nameof(name));
            }

            return index;
        }

        /// <summary>
        /// Get location of the specified uniform
        /// </summary>
        /// <param name="name">Name of the uniform</param>
        /// <returns>Index of the uniform</returns>
        public int GetUniformLocation(string name)
        {
            int index = GL.GetUniformLocation(ID, name);
            if (index < 0)
            {
                throw new ArgumentException("Couldn't find uniform location", nameof(name));
            }

            return index;
        }

        /// <summary>
        /// Set specified uniform value
        /// </summary>
        /// <param name="name">Name of the uniform</param>
        /// <param name="value">Int value</param>
        public void SetUniform(string name, int value)
        {
            Bind();
            GL.Uniform1(GetUniformLocation(name), value);
        }

        /// <summary>
        /// Set specified uniform value
        /// </summary>
        /// <param name="name">Name of the uniform</param>
        /// <param name="value">Float value</param>
        public void SetUniform(string name, float value)
        {
            Bind();
            GL.Uniform1(GetUniformLocation(name), value);
        }

        /// <summary>
        /// Set specified uniform value
        /// </summary>
        /// <param name="name">Name of the uniform</param>
        /// <param name="value"><see cref="Vector2"/> value</param>
        public void SetUniform(string name, Vector2 value)
        {
            Bind();
            GL.Uniform2(GetUniformLocation(name), value);
        }

        /// <summary>
        /// Set specified uniform value
        /// </summary>
        /// <param name="name">Name of the uniform</param>
        /// <param name="value"><see cref="Vector3"/> value</param>
        public void SetUniform(string name, Vector3 value)
        {
            Bind();
            GL.Uniform3(GetUniformLocation(name), value);
        }

        /// <summary>
        /// Set specified uniform value
        /// </summary>
        /// <param name="name">Name of the uniform</param>
        /// <param name="value"><see cref="Vector4"/> value</param>
        public void SetUniform(string name, Vector4 value)
        {
            Bind();
            GL.Uniform4(GetUniformLocation(name), value);
        }

        /// <summary>
        /// Set specified uniform value
        /// </summary>
        /// <param name="name">Name of the uniform</param>
        /// <param name="value"><see cref="Color4"/> value</param>
        public void SetUniform(string name, Color4 value)
        {
            Bind();
            GL.Uniform4(GetUniformLocation(name), value);
        }

        /// <summary>
        /// Set specified uniform value
        /// </summary>
        /// <param name="name">Name of the uniform</param>
        /// <param name="value"><see cref="Matrix4"/> value</param>
        public void SetUniform(string name, Matrix4 value)
        {
            Bind();
            GL.UniformMatrix4(GetUniformLocation(name), false, ref value);
        }
    }
}