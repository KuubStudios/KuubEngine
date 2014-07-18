using System;
using System.Diagnostics;

using KuubEngine.Core;
using KuubEngine.Utility;

namespace KuubEngine.Diagnostics {
    public enum LogLevel {
        Debug,
        Info,
        Warn,
        Error
    }

    public static class Log {
        public static void Write(LogLevel level, string value) {
            if(level == LogLevel.Debug && !Debugger.IsAttached) return;

            Console.WriteLine("[{0:G}] [{1}] {2}", GameTime.Total, level.ToString().ToUpper(), value);
        }

        public static void Write(LogLevel level, object value) {
            Write(level, value.ToString());
        }

        public static void Write(LogLevel level, string format, params object[] args) {
            Write(level, args == null || args.Length == 0 ? format : format.Format(args));
        }

        public static void Debug(object value) {
            Write(LogLevel.Debug, value);
        }

        public static void Debug(string format, params object[] args) {
            Write(LogLevel.Debug, format, args);
        }

        public static void Info(object value) {
            Write(LogLevel.Info, value);
        }

        public static void Info(string format, params object[] args) {
            Write(LogLevel.Info, format, args);
        }

        public static void Warn(object value) {
            Write(LogLevel.Warn, value);
        }

        public static void Warn(string format, params object[] args) {
            Write(LogLevel.Warn, format, args);
        }

        public static void Error(object value) {
            Write(LogLevel.Error, value);
        }

        public static void Error(string format, params object[] args) {
            Write(LogLevel.Error, format, args);
        }
    }
}