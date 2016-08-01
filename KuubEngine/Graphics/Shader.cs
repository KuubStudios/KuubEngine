// <copyright file="Shader.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using KuubEngine.Graphics.Exceptions;
using NLog;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics
{
    /// <summary>
    /// Managed wrapper for OpenGL Shader objects
    /// </summary>
    public class Shader : IGLObject
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
                    id = GL.CreateShader(Type);
                }

                return id;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="ShaderType"/> of this shader
        /// </summary>
        public ShaderType Type { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shader"/> class.
        /// </summary>
        /// <param name="type"><see cref="ShaderType"/></param>
        public Shader(ShaderType type)
        {
            Type = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Shader"/> class.
        /// </summary>
        /// <param name="type"><see cref="ShaderType"/></param>
        /// <param name="source">GLSL source to compile</param>
        public Shader(ShaderType type, string source) : this(type)
        {
            Load(source);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            if (id != 0)
            {
                GL.DeleteShader(id);
                id = 0;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return string.Format("{0} {1}", Type, id);
        }

        /// <summary>
        ///     Load and compile sourcecode
        /// </summary>
        /// <param name="source">GLSL sourcecode</param>
        public void Load(string source)
        {
            GL.ShaderSource(ID, source);
            GL.CompileShader(ID);

            int status;
            GL.GetShader(ID, ShaderParameter.CompileStatus, out status);
            if (status != 1)
            {
                throw new ShaderCompileException(GL.GetShaderInfoLog(ID), source);
            }

            int length;
            GL.GetShader(ID, ShaderParameter.InfoLogLength, out length);
            if (length > 1)
            {
                log.Warn("{0} info log not empty:\n\t{1}", this, GL.GetShaderInfoLog(ID).TrimEnd('\n').Replace("\n", "\n\t"));
            }
        }

        /// <summary>
        ///     Attaches this Shader to a ShaderProgram
        /// </summary>
        /// <param name="program"><see cref="ShaderProgram"/> to attach to</param>
        public void Attach(ShaderProgram program)
        {
            GL.AttachShader(program.ID, ID);
        }
    }
}