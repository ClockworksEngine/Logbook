// Copyright Digital Descent, All rights reserved.

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using DigitalDescent.Clockworks.Runtime.Unity;
using DigitalDescent.Logbook.Capture;
using DigitalDescent.Logbook.Extensions;
using DigitalDescent.Logbook.Framework;
using DigitalDescent.Logbook.Microsoft;
using DigitalDescent.Logbook.Unity;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using UnityEngine;

using IUnityLogger = UnityEngine.ILogger;
using IUnityLogHandler = UnityEngine.ILogHandler;

#nullable enable
namespace DigitalDescent.Logbook
{
    /// <summary>
    /// Primary interface for working with the Logbook logging system.
    /// </summary>
    public static class Logging
    {
        private static Serilog.Core.Logger? _logger;
        private static LogbookSettings? _config;
        private static LoggingLevelSwitch? _level;
        private static LoggingCaptureSink? _captureSink;
        private static IUnityLogHandler? _internalLogHandler;

        /// <summary>
        /// Default logger name used for the Logbook logging system. 
        /// </summary>
        public const string DefaultLoggerName = "UnityEngine";

        /// <summary>
        /// Unity compatible <see cref="ILogger"/> implementation for the Logbook logging system.
        /// </summary>
        public static IUnityLogger? UnityLogger { get; private set; }

        /// <summary>
        /// Microsoft compatible <see cref="ILoggerFactory"/> implementation for the Logbook logging system.
        /// </summary>
        public static ILoggerFactory? LoggerFactory { get; private set; }

        /// <summary>
        /// Flag indicating whether the logger system has been initialized. This is used to prevent multiple initializations and ensure that logging is only performed when the system is ready.
        /// </summary>
        public static bool IsInitialized => _logger != null;

        /// <summary>
        /// The configuration settings for the logger system. This includes settings such as log directory, log file format, and other relevant configurations.
        /// </summary>
        public static LogbookSettings? Settings
        {
            internal set
            {
                if (IsInitialized)
                    throw new InvalidOperationException("Logger is already initialized.");

                _config = value;
            }
            get => _config;
        }

        /// <summary>
        /// Gets or sets the minimum log level required to write a message. Any level below the configured value will be ignored.
        /// </summary>
        /// <remarks>
        /// The default value is set by <see cref="LogbookSettings.MinimumLevel"/>.
        /// If for some reason this value is accessed prior to initialization, it will default to <see cref="LogEventLevel.Information"/>.
        /// </remarks>
        public static LogEventLevel MinimumLevel
        {
            get => _level?.MinimumLevel ?? LogEventLevel.Information;
            set => _level!.MinimumLevel = value;
        }

        /// <summary>
        /// Initializes the logger system and sets up the necessary configurations.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        internal static void Initialize()
        {
            // Verify that the logger is not already initialized to prevent multiple initializations.
            // This normally should not be possible, but we want to be safe and ensure that the logger is only initialized once.
            if (IsInitialized)
                throw new InvalidOperationException("Logger is already initialized.");

            // Wrap the initialization logic in a try-catch block to handle any exceptions that may occur during setup.
            try
            {
                // Configure Unity's stack trace logging behavior based on the log type
                // and whether the application is running in the editor.
                Application.SetStackTraceLogType(LogType.Log, Application.isEditor ? StackTraceLogType.ScriptOnly : StackTraceLogType.None);
                Application.SetStackTraceLogType(LogType.Warning, Application.isEditor ? StackTraceLogType.ScriptOnly : StackTraceLogType.None);
                Application.SetStackTraceLogType(LogType.Error, Application.isEditor ? StackTraceLogType.ScriptOnly : StackTraceLogType.None);

                // Initialize the logging capture sink and logger configuration
                Settings = LogbookSettings.Instance;
                _captureSink = new LoggingCaptureSink();
                LoggerFactory = new MicrosoftLoggerFactory();
                UnityLogger = new UnityLogger();

                _internalLogHandler ??= UnityEngine.Debug.unityLogger.logHandler;
                Application.quitting += OnShutdown;

                // Setup our logging level information
                _level = new LoggingLevelSwitch
                {
                    MinimumLevel = Settings.MinimumLevel
                };

                var logConfig = new LoggerConfiguration()
                    .MinimumLevel.ControlledBy(_level)
                    .WriteTo.Sink(_captureSink)
                    .WriteTo.Unity(logger: _internalLogHandler)
                    .Enrich.WithDemystifiedStackTraces();

                // Configure our custom targets.
                foreach (var target in Settings.Targets)
                    target.Initialize(logConfig);

                _logger = logConfig.CreateLogger();
                UnityEngine.Debug.unityLogger.logHandler = new UnityLogger();
                LogInternal($"Logbook initialized.", LogEventLevel.Information, nameof(Logging));
            }
            catch (Exception ex) { UnityEngine.Debug.LogError($"Failed to initialize LogBook: {ex.Message}"); }
        }

        /// <summary>
        /// Writes a message to the log.
        /// </summary>
        /// <param name="logType"><see cref="LogType"/> to log as.</param>
        /// <inheritdoc cref="LogInternal(string, LogEventLevel, string, int, UnityEngine.Object, Exception, object[])"/>
        [HideInCallstack]
        public static void Log(LogType logType, string message, UnityEngine.Object? context = null, [CallerFilePath] string callerFile = "") =>
            LogInternal(message, logType.ToLogEventLevel(), callerFile, 2, context);

        /// <summary>
        /// Writes a verbose message to the log. 
        /// </summary>
        /// <inheritdoc cref="LogInternal(string, LogEventLevel, string, int, UnityEngine.Object, Exception, object[])"/>
        public static void Verbose(string message, UnityEngine.Object? context = null, [CallerFilePath] string callerFile = "") => LogInternal(message, LogEventLevel.Verbose, callerFile, 2, context);

        /// <summary>
        /// Writes a debug message to the log. 
        /// </summary>
        /// <inheritdoc cref="LogInternal(string, LogEventLevel, string, int, UnityEngine.Object, Exception, object[])"/>
        [HideInCallstack]
        public static void Debug(string message, UnityEngine.Object? context = null, [CallerFilePath] string callerFile = "") => LogInternal(message, LogEventLevel.Debug, callerFile, 2, context);

        /// <summary>
        /// Writes an informational message to the log.
        /// </summary>
        /// <inheritdoc cref="LogInternal(string, LogEventLevel, string, int, UnityEngine.Object, Exception, object[])"/>
        [HideInCallstack]
        public static void Info(string message, UnityEngine.Object? context = null, [CallerFilePath] string callerFile = "") => LogInternal(message, LogEventLevel.Information, callerFile, 2, context);

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        /// <inheritdoc cref="LogInternal(string, LogEventLevel, string, int, UnityEngine.Object, Exception, object[])"/>
        [HideInCallstack]
        public static void Warning(string message, UnityEngine.Object? context = null, [CallerFilePath] string callerFile = "") => LogInternal(message, LogEventLevel.Warning, callerFile, 2, context);

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        /// <inheritdoc cref="LogInternal(string, LogEventLevel, string, int, UnityEngine.Object, Exception, object[])"/>
        [HideInCallstack]
        public static void Error(string message, UnityEngine.Object? context = null, [CallerFilePath] string callerFile = "") => LogInternal(message, LogEventLevel.Error, callerFile, 2, context);

        /// <summary>
        /// Writes an error message to the log with an associated exception. 
        /// </summary>
        /// <param name="exception">Exception to write.</param>
        /// <inheritdoc cref="LogInternal(string, LogEventLevel, string, int, UnityEngine.Object, Exception, object[])"/>
        [HideInCallstack]
        public static void Exception(Exception exception, UnityEngine.Object? context = null, [CallerFilePath] string callerFile = "") => 
            LogInternal(string.Empty, LogEventLevel.Error, callerFile, 2, context, exception);

        /// <summary>
        /// Writes a blank line to the log. This can be useful for separating log entries and improving readability.
        /// </summary>
        [HideInCallstack]
        public static void BlankLine() => _internalLogHandler?.LogFormat(LogType.Log, null, string.Empty);

        /// <summary>
        /// Creates a new logging capture scope that captures log events emitted during its lifetime. This is useful for testing and debugging, 
        /// allowing you to capture and inspect log events generated by specific code blocks.
        /// </summary>
        /// <returns><see cref="LoggingCaptureScope"/> representing the capture scope.</returns>
        public static LoggingCaptureScope WithCaptureScope()
        {
            Throws.IfNull(_captureSink, nameof(_captureSink));
            return new LoggingCaptureScope(_captureSink);
        }

        /// <summary>
        /// Internal method to handle application shutdown and perform any necessary cleanup for the logger.
        /// </summary>
        private static void OnShutdown()
        {
            if (!IsInitialized)
                throw new InvalidOperationException("Logger is not initialized.");

            LogInternal($"Logbook shutting down.", LogEventLevel.Information, nameof(Logging));
            _logger?.Dispose();
            _logger = null;

            _config = null;
            UnityEngine.Debug.unityLogger.logHandler = _internalLogHandler;
            Application.quitting -= OnShutdown;
        }

        /// <summary>
        /// Checks if the logger is initialized and throws an exception if it is not. This method is used to ensure that logging operations are only performed when the logger is properly set up.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when the logger is not initialized and we are not using the editor.</exception>
        [HideInCallstack]
        private static void CheckInitialization()
        {
            if (!IsInitialized && !Application.isEditor)
                throw new InvalidOperationException("Logger is not initialized.");
        }

        /// <summary>
        /// Logs a message using Unity's Debug class if the logger is not initialized and the application is running in the Unity editor. 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="level"></param>
        [HideInCallstack]
        [Conditional("UNITY_EDITOR")]
        private static void LogIfUninitialized(string message, LogEventLevel level)
        {
            if (!Application.isEditor || IsInitialized)
                return;

            UnityEngine.Debug.unityLogger.Log(level.ToLogType(), message);
        }

        /// <summary>
        /// Internal method that handles the actual logging of messages to the underlying Serilog logger instance.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="level">Log level to log with.</param>
        /// <param name="callerFile">Path to the file of the class that called the log method.</param>
        /// <param name="stackDepth">Stack depth of the original calling method. Defaults to 1</param>
        /// <param name="context">Optional <see cref="UnityEngine.Object"/> this log message originated from.</param>
        /// <param name="ex">Optional <see cref="System.Exception"/> associated with the log message.</param>
        /// <param name="values">Optional parameter values to log with.</param>
        [HideInCallstack]
        internal static void LogInternal(string message, LogEventLevel level, string callerFile, int stackDepth = 1, UnityEngine.Object? context = null, Exception? ex = null, params object[] values)
        {
            // If we have an exception but no message. We should create
            // one based on our given exception
            if (string.IsNullOrEmpty(message) && ex != null)
                message = $"{ex.GetType().FullName}: {ex.Message}";

            // Check if we are already initialized and attempt to log to Unity's editor
            // directly if we are not. In a proper player build this will throw an exception instead.
            CheckInitialization();
            LogIfUninitialized(message, level);

            // If we are in the editor and the logger is not initialized, we can skip logging
            // as LogIfUninitialized will handle logging to Unity's Debug class if we are in the editor.
            if (Application.isEditor && !IsInitialized)
                return;

            // Attempt to discover our caller's name by our context if provided.
            // Otherwise, if we were not provided a context. We attempt to discover the caller's name
            // by reading the StackFrame at the given stack depth.
            string callerName = context != null ? context.name : string.Empty;
            if (string.IsNullOrEmpty(callerName))
            {
                // Because we know that LogInternal is always called two methods deep from the original
                // calling method. We can skip two frame and read the caller method and type from the stack frame.
                // This allows us to get the correct caller name for logging.
                var callerFrame = new StackFrame(stackDepth, false);
                callerName = GetCallerName(callerFrame, callerFile);
            }

            // Pass our details directly to our underlying Serilog logger instance.
            Throws.IfNull(_logger, nameof(_logger));
            _logger
                .ForContext("Caller", callerName)
                .WithUnityObject(context)
                .Write(level, ex, message, values);
        }

        /// <summary>
        /// Retrieves the name of the caller method or type from the provided stack frame and caller file path. 
        /// </summary>
        /// <param name="callerFrame"><see cref="StackFrame"/> representing the caller.</param>
        /// <param name="callerFile">String to the caller's file.</param>
        /// <param name="defaultName">Optional default name when all other methods fail. Defaults to <see cref="LogbookConstants.DefaultCallerName"/>.</param>
        /// <returns>Name of the calling type.</returns>
        private static string GetCallerName(StackFrame callerFrame, string callerFile, string defaultName = LogbookConstants.DefaultCallerName)
        {
            var callerMethod = callerFrame.GetMethod();
            var callerType = callerMethod?.ReflectedType ?? callerMethod?.DeclaringType;

            if (callerType != null && callerType.IsGenericType)
            {
                var genericArguments = callerType.GetGenericArguments();
                if (genericArguments.Length > 0 && !genericArguments[0].IsGenericParameter)
                    return genericArguments[0].Name;
            }

            var callerName = callerType?.Name ?? Path.GetFileNameWithoutExtension(callerFile);
            return string.IsNullOrEmpty(callerName) ? defaultName : callerName;
        }
    }
}