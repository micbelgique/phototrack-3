using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trasys.Dev.Tools.Logger
{
    /// <summary>
    /// Write traces in a ApplicationTraces.log file in the application local folder.
    /// </summary>
    public static class Trace
    {
        private static StorageFileEventListener _storage = null;        

        /// <summary>
        /// Gets the Log Filename generated when you write a trace.
        /// </summary>
        public static string LogFileName { get; private set; }

        /// <summary>
        /// Write an informational message into a ApplicationTraces.log file.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Info(string message, params object[] args)
        {
            if (_storage == null) InitializeTracesEngine();

            MetroEventSource.Log.Info(String.Format(message, args));
        }

        /// <summary>
        /// Write a warning message into a ApplicationTraces.log file.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Warn(string message, params object[] args)
        {
            if (_storage == null) InitializeTracesEngine();

            MetroEventSource.Log.Warn(String.Format(message, args));
        }

        /// <summary>
        /// Write a debug message into a ApplicationTraces.log file.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Debug(string message, params object[] args)
        {
            if (_storage == null) InitializeTracesEngine();

            MetroEventSource.Log.Debug(String.Format(message, args));
        }

        /// <summary>
        /// Write an error message into a ApplicationTraces.log file.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Error(string message, params object[] args)
        {
            if (_storage == null) InitializeTracesEngine();

            MetroEventSource.Log.Error(String.Format(message, args));
        }

        /// <summary>
        /// Write a critical message into a ApplicationTraces.log file.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        public static void Critical(string message, params object[] args)
        {
            if (_storage == null) InitializeTracesEngine();

            MetroEventSource.Log.Critical(String.Format(message, args));
        }

        /// <summary>
        /// Initializes a new instance of Traces Logger.
        /// </summary>
        private static void InitializeTracesEngine()
        {
            _storage = new StorageFileEventListener("ApplicationTraces");
            LogFileName = _storage.FullLogFilename;
            _storage.EnableEvents(MetroEventSource.Log, System.Diagnostics.Tracing.EventLevel.LogAlways);
        }
    }
}
