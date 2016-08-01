// <copyright file="IGLObject.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;

namespace KuubEngine.Graphics
{
    /// <summary>
    /// Interface for OpenGL objecte
    /// </summary>
    public interface IGLObject : IDisposable
    {
        /// <summary>
        /// Gets the ID that represents this GL object
        /// </summary>
        int ID { get; }
    }
}