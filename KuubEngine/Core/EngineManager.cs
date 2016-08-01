// <copyright file="EngineManager.cs" company="Kuub Studios">
// Copyright (c) Kuub Studios. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

using System;
using System.IO;
using NLog;
using OpenTK;

namespace KuubEngine.Core
{
    /// <summary>
    /// Helper class to start game instances
    /// </summary>
    public static class EngineManager
    {
        private static Logger log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Starts a new game instance and runs it
        /// </summary>
        /// <typeparam name="T"><see cref="Game"/> to start</typeparam>
        /// <param name="args">Command line arguments</param>
        /// <param name="catchErrors">Catch unhandled exceptions and show the crash reporter</param>
        public static void Start<T>(string[] args, bool catchErrors = true) where T : Game, new()
        {
            if (catchErrors)
            {
                AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) =>
                {
                    log.Fatal((Exception)eventArgs.ExceptionObject, "Unhandled exception in game");
                    File.WriteAllText("crash.txt", eventArgs.ExceptionObject.ToString());
                    Environment.Exit(1);
                };
            }

            using (Toolkit.Init())
            {
                using (T game = new T())
                {
                    game.Run(args);
                }
            }
        }
    }
}