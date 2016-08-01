// <copyright file="ShaderCompileException.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace KuubEngine.Graphics.Exceptions
{
    /// <summary>
    /// Represents errors that occur during <see cref="Shader"/> compilation
    /// </summary>
    public class ShaderCompileException : Exception
    {
        /// <summary>
        /// Gets or sets the source code of the <see cref="Shader"/> that failed to compile
        /// </summary>
        public string SourceCode { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderCompileException"/> class.
        /// </summary>
        /// <param name="message">Error message</param>
        /// <param name="sourcecode">Source code</param>
        public ShaderCompileException(string message, string sourcecode) : base(message)
        {
            SourceCode = sourcecode;
        }
    }
}