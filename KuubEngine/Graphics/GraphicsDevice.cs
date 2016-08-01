// <copyright file="GraphicsDevice.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using KuubEngine.Utility;
using NLog;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Graphics
{
    /// <summary>
    /// Helper class for querying graphics adapter
    /// </summary>
    public static class GraphicsDevice
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Gets a value indicating whether the graphics adapter is using nvidia drivers
        /// </summary>
        public static bool NvidiaCard { get; private set; }

        /// <summary>
        /// Gets the <see cref="Version"/> of OpenGL being used
        /// </summary>
        public static Version GLVersion { get; private set; }

        /// <summary>
        /// Gets a value indicating whether gets if the graphics adapter supports GL3
        /// </summary>
        public static bool SupportsGL3
        {
            get { return GLVersion.Major >= 3; }
        }

        /// <summary>
        /// Populates static fields after OpenGL context is created
        /// </summary>
        internal static void Initialize()
        {
            log.Info("Checking Graphics Device");

            string vendor = GL.GetString(StringName.Vendor);
            NvidiaCard = vendor.StartsWith("NVIDIA", StringComparison.OrdinalIgnoreCase);
            log.Info("Vendor: {0}", vendor);
            log.Info("Renderer: {0}", GL.GetString(StringName.Renderer));

            try
            {
                string version = GL.GetString(StringName.Version);
                GLVersion = new Version(version.Split(' ')[0]);
                Util.CheckGLError();
                log.Info("GL Version: {0} ({1})", GLVersion, version);
            }
            catch (Exception e)
            {
                log.Warn("Failed to parse GL Version");
                GLVersion = new Version(GL.GetInteger(GetPName.MajorVersion), GL.GetInteger(GetPName.MinorVersion));
                log.Info("GL Version: {0} ({1})", GLVersion, e.Message);
            }

            log.Info("GLSL: {0}", GL.GetString(StringName.ShadingLanguageVersion));
        }

        /// <summary>
        /// Checks whether the graphics adapter supports specified extension
        /// </summary>
        /// <param name="extension">OpenGL extension name</param>
        /// <returns>If extension is supported or not</returns>
        public static bool Supports(string extension)
        {
            return GL.GetString(StringName.Extensions).Contains(extension);
        }
    }
}