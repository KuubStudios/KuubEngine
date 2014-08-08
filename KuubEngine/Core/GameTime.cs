using System;
using System.Diagnostics;

using KuubEngine.Utility;

namespace KuubEngine.Core {
    /// <summary>
    ///     Used to keep track of time
    /// </summary>
    public class GameTime {
        private static readonly Stopwatch stopwatch;

        /// <summary>
        ///     Elapsed time since the last frame
        /// </summary>
        public TimeSpan Elapsed { get; set; }

        /// <summary>
        ///     Total time since game launch
        /// </summary>
        public static TimeSpan Total {
            get { return stopwatch.Elapsed; }
        }

        static GameTime() {
            stopwatch = Stopwatch.StartNew();
        }

        public GameTime() {
            Elapsed = TimeSpan.Zero;
        }

        internal void Update(double seconds) {
            Elapsed = TimeSpan.FromTicks((long)(seconds * 10000000));
        }

        public override string ToString() {
            return "{{ Total: {0}, Elapsed: {1} }}".Format(Total, Elapsed);
        }
    }
}