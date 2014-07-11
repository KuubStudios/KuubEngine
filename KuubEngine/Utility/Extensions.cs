using System;

namespace KuubEngine.Utility {
    public static class Extensions {
        public static string Format(this string input, params object[] args) {
            return String.Format(input, args);
        }
    }
}