// <copyright file="ShaderValidateException.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace KuubEngine.Graphics.Exceptions
{
    /// <summary>
    /// Represents errors that occur during validation of a <see cref="Shader"/> after linking to a <see cref="ShaderProgram"/>
    /// </summary>
    public class ShaderValidateException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShaderValidateException"/> class.
        /// </summary>
        /// <param name="message">Message</param>
        public ShaderValidateException(string message) : base(message)
        {
        }
    }
}