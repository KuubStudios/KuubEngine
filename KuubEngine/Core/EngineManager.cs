using System;

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
            using(Toolkit.Init()) using(T game = new T()) game.Run(args);

            if(catchErrors) AppDomain.CurrentDomain.UnhandledException += (sender, eventArgs) => Log.Error("Unhandled exception: {0}", eventArgs.ExceptionObject);
        }
    }
}