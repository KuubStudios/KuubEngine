// <copyright file="IVertexType.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace KuubEngine.Graphics.Structures
{
    /// <summary>
    /// Interface for custom vertex structures
    /// </summary>
    public interface IVertexType
    {
        /// <summary>
        /// Gets the <see cref="VertexDeclaration"/>
        /// </summary>
        VertexDeclaration VertexDeclaration { get; }
    }
}