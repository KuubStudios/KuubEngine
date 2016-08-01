// <copyright file="Program.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Diagnostics;
using KuubEngine.Core;

namespace TestGame
{
    /// <summary>
    /// Program entry point
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Program entry point
        /// </summary>
        /// <param name="args">Command line arguments</param>
        internal static void Main(string[] args)
        {
            EngineManager.Start<Main>(args, !Debugger.IsAttached);
        }
    }
}