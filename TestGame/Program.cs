using System.Diagnostics;

using KuubEngine.Core;

namespace TestGame {
    internal class Program {
        private static void Main(string[] args) {
            EngineManager.Start<Main>(args, !Debugger.IsAttached);
        }
    }
}