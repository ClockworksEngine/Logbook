// Copyright Digital Descent, All rights reserved.
using System;
using DigitalDescent.Logbook.Extensions;
using UnityEngine;

#nullable enable
namespace DigitalDescent.Logbook.Unity
{
    /// <summary>
    /// Unity <see cref="ILogger"/> compatible implementation that forwards logs to the Logbook logging system.
    /// </summary>
    internal sealed class UnityLogger : ILogger
    {
        /// <inheritdoc cref="ILogger.logHandler"/>
        public ILogHandler? logHandler { get; set; }

        /// <inheritdoc cref="ILogger.logEnabled"/>
        public bool logEnabled { get; set; }

        /// <inheritdoc cref="ILogger.filterLogType"/>
        public LogType filterLogType { get; set; }

        /// <inheritdoc cref="ILogger.IsLogTypeAllowed(LogType)"/>
        /// <remarks>In this implementation we always return true. We filter types inside the Logger class.</remarks>
        [HideInCallstack]
        public bool IsLogTypeAllowed(LogType logType) => true;

        /// <inheritdoc cref="ILogger.Log(LogType, object)"/>
        [HideInCallstack]
        public void Log(LogType logType, object message) => LogInternal(logType, message);

        /// <inheritdoc cref="ILogger.Log(LogType, string, object)"/>
        [HideInCallstack]
        public void Log(LogType logType, string tag, object message) => LogInternal(logType, message, tag);

        /// <inheritdoc cref="ILogger.Log(LogType, object, UnityEngine.Object)"/>
        [HideInCallstack]
        public void Log(LogType logType, object message, UnityEngine.Object context) => LogInternal(logType, message);

        /// <inheritdoc cref="ILogger.Log(LogType, string, object, UnityEngine.Object)"/>
        [HideInCallstack]
        public void Log(LogType logType, string tag, object message, UnityEngine.Object context) => LogInternal(logType, message, tag, context);

        /// <inheritdoc cref="ILogger.Log(object)"/>
        [HideInCallstack]
        public void Log(object message) => LogInternal(LogType.Log, message);

        /// <inheritdoc cref="ILogger.Log(string, object)"/>
        [HideInCallstack]
        public void Log(string tag, object message) => LogInternal(LogType.Log, message, tag);

        /// <inheritdoc cref="ILogger.Log(object, UnityEngine.Object)"/>
        [HideInCallstack]
        public void Log(string tag, object message, UnityEngine.Object context) => LogInternal(LogType.Log, message, tag, context);

        /// <inheritdoc cref="ILogger.Log(string, object, UnityEngine.Object)"/>
        [HideInCallstack]
        public void LogWarning(string tag, object message) => LogInternal(LogType.Warning, message, tag);

        /// <inheritdoc cref="ILogger.LogWarning(string, object, UnityEngine.Object)"/>
        [HideInCallstack]
        public void LogWarning(string tag, object message, UnityEngine.Object context) => LogInternal(LogType.Warning, message, tag, context);

        /// <inheritdoc cref="ILogger.LogError(string, object)"/>
        [HideInCallstack]
        public void LogError(string tag, object message) => LogInternal(LogType.Error, message, tag);

        /// <inheritdoc cref="ILogger.LogError(string, object, UnityEngine.Object)"/>
        [HideInCallstack]
        public void LogError(string tag, object message, UnityEngine.Object context) => LogInternal(LogType.Error, message, tag);

        /// <inheritdoc cref="ILogger.LogException(Exception)"/>
        [HideInCallstack]
        public void LogException(Exception exception) => LogInternal(LogType.Error, $"{exception.GetType().FullName}: {exception.Message}", ex: exception);

        /// <inheritdoc cref="ILogHandler.LogException(Exception, UnityEngine.Object)"/>
        [HideInCallstack]
        public void LogException(Exception exception, UnityEngine.Object context) => LogInternal(LogType.Error, $"{exception.GetType().FullName}: {exception.Message}", context: context, ex: exception);

        /// <inheritdoc cref="ILogger.LogFormat(LogType, string, object[])"/>
        [HideInCallstack]
        public void LogFormat(LogType logType, string format, params object[] args) => LogInternal(logType, string.Format(format, args));

        /// <inheritdoc cref="ILogger.LogFormat(LogType, string, object[])"/>
        [HideInCallstack]
        public void LogFormat(LogType logType, UnityEngine.Object context, string format, params object[] args) => LogInternal(logType, string.Format(format, args), context: context);

        [HideInCallstack]
        private void LogInternal(LogType logType, object message, string? tag = null, UnityEngine.Object? context = null, Exception? ex = null)
        {
            tag ??= Logging.DefaultLoggerName;
            Logging.LogInternal(
                message: message.ToString(),
                level: logType.ToLogEventLevel(),
                callerFile: tag,
                stackDepth: 5,
                context);
        }
    }
}