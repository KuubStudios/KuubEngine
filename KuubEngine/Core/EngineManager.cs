using System;
using System.IO;

using KuubEngine.Diagnostics;

using OpenTK;

namespace KuubEngine.Core {
    public static class EngineManager {
        /// <summary>
        ///     Starts a new game instance and runs it
        /// </summary>
        /// <typeparam name="T">Game to start</typeparam>
        /// <param name="args">Command line arguments</param>
        /// <param name="catchErrors">Catch unhandled exceptions and show the crashreporter</param>
        public static void Start<T>(string[] args, bool catchErrors = true) where T : Game, new() {
            if(catchErrors) {
                AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) => {
                    Log.Error("Unhandled exception: {0}", eventArgs.ExceptionObject);
                    File.WriteAllText("crash.txt", eventArgs.ExceptionObject.ToString());
                    Environment.Exit(1);
                };
            }

            using(Toolkit.Init()) using(T game = new T()) game.Run(args);
        }
    }
}