// <copyright file="Util.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Diagnostics;
using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;

namespace KuubEngine.Utility
{
    /// <summary>
    /// Utility functions
    /// </summary>
    public static class Util
    {
        /// <summary>
        /// Check for OpenGL errors
        /// </summary>
        [Conditional("DEBUG")]
        public static void CheckGLError()
        {
            ErrorCode error = GL.GetError();
            if (error != ErrorCode.NoError)
            {
                throw new GraphicsErrorException("GL.GetError() returned " + error);
            }
        }

        /// <summary>
        /// Check for OpenAL errors
        /// </summary>
        [Conditional("DEBUG")]
        public static void CheckALError()
        {
            ALError error = AL.GetError();
            if (error != ALError.NoError)
            {
                throw new AudioException("AL.GetError() returned " + error);
            }
        }
    }
}