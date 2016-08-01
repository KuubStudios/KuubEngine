// <copyright file="ShaderLinkException.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace KuubEngine.Graphics.Exceptions
{
    /// <summary>
    /// Represents errors that occur during linking a <see cref="Shader"/> to a <see cref="ShaderProgram"/>
    /// </summary>
    public class ShaderLinkException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderLinkException"/> class.
        /// </summary>
        /// <param name="message">Message</param>
        public ShaderLinkException(string message) : base(message)
        {
        }
    }
}