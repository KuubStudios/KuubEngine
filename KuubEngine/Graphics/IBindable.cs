// <copyright file="IBindable.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace KuubEngine.Graphics
{
    /// <summary>
    /// Interface for OpenGL objecte
    /// </summary>
    public interface IBindable : IGLObject
    {
        /// <summary>
        /// Binds this object on the current OpenGL context
        /// </summary>
        void Bind();

        /// <summary>
        /// Unbinds this object from the current OpenGL context
        /// </summary>
        void Unbind();
    }
}