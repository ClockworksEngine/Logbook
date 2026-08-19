// Copyright Digital Descent, All rights reserved.

using System.IO;
using System.Text;
using DigitalDescent.Logbook;
using DigitalDescent.Logbook.Extensions;
using DigitalDescent.Logbook.Framework;
using DigitalDescent.Logbook.Unity;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Formatting;
using Serilog.Formatting.Display;
using UnityEngine;
using IUnityLogHandler = UnityEngine.ILogHandler;

#nullable enable
namespace DigitalDescent.Clockworks.Runtime.Unity
{
    /// <summary>
    /// <see cref="ILogEventSink"/> implementation for writing Serilog log events to the Unity console.
    /// </summary>
    internal sealed class UnityLoggerEventSink : ILogEventSink
    {
        private readonly StringBuilder _sb = new();

        private readonly IUnityLogHandler _logHandler;
        private readonly ITextFormatter _formatter;

        public UnityLoggerEventSink(IUnityLogHandler logHandler, string format)
        {
            _logHandler = logHandler;
            _formatter = new MessageTemplateTextFormatter(format);
        }

        /// <inheritdoc cref="ILogEventSink.Emit(LogEvent)"/>
        [HideInCallstack]
        public void Emit(LogEvent logEvent)
        {
            var writer = new StringWriter();
            _formatter.Format(logEvent, writer);

            var message = writer.ToString().Trim();
            var messageColor = logEvent.Level.GetLevelColor();

            Throws.IfNull(Logging.Settings, nameof(Logging.Settings));
            var openingColor = !Logging.Settings.UseLoggingColors ? string.Empty : Application.isEditor ? messageColor.ToUnityColorTag() : messageColor.ToAnsiiColor();
            var closingColor = !Logging.Settings.UseLoggingColors ? string.Empty : Application.isEditor ? messageColor.ToUnityColorTag(true) : messageColor.ToAnsiiColor(true);
            var logType = logEvent.Level.ToLogType();
            message = $"{openingColor}{message}{closingColor}";

            // Write our base formatted message to the builder.
            _sb.Clear();
            _sb.AppendLine(message);

            // If we are not running inside the Unity editor, we format and write our own stack trace for error, exception, and assert log types. 
            // Unity's default exception logs are not very clean and can be difficult to read, so we provide our own formatted stack trace for better readability.
            if (!Application.isEditor && logEvent.Exception != null && Application.GetStackTraceLogType(logType) != StackTraceLogType.None)
            {
                _sb.AppendLine($"{openingColor}{logEvent.Exception.GetType().FullName}: {logEvent.Exception.Message}{closingColor}");
                _sb.AppendLine($"{openingColor}{logEvent.Exception?.StackTrace}{closingColor}");
                _sb.AppendLine(string.Empty);
            }

            Object? context = null;
            if (logEvent.Properties.TryGetValue(UnityObjectEnricher.UnityContextKey, out var prop) &&
                prop is ScalarValue scalar && scalar.Value is Object obj)
                context = obj;

            _logHandler.LogFormat(logType, context, _sb.ToString());
        }
    }

    /// <summary>
    /// Static extension methods for <see cref="UnityLoggerEventSink"/>.
    /// </summary>
    internal static class UnityLoggerEventSinkExtensions
    {
        /// <summary>
        /// Adds a sink that writes log events to the Unity console.
        /// </summary>
        /// <param name="config">Sink configuration to configure.</param>
        /// <param name="logger">Unity ILogger to log to.</param>
        /// <param name="format">Format to use when logging messages.</param>
        /// <returns>LoggerConfiguration for chaining calls.</returns>
        public static LoggerConfiguration Unity(
            this LoggerSinkConfiguration config,
            IUnityLogHandler logger,
            string format = LogbookConstants.ConsoleTemplate) =>
            config.Sink(new UnityLoggerEventSink(logger, format));

        /// <inheritdoc cref="Unity(LoggerSinkConfiguration, IUnityLogHandler, string)"/>
        public static LoggerConfiguration Unity(
            this LoggerSinkConfiguration config,
            string format = LogbookConstants.ConsoleTemplate) =>
            config.Sink(new UnityLoggerEventSink(Debug.unityLogger.logHandler, format));
    }
}
