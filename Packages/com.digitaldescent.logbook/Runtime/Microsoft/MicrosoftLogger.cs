// Copyright Digital Descent, All rights reserved.

using System;
using Microsoft.Extensions.Logging;
using UnityEngine;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace DigitalDescent.Logbook.Microsoft
{
    /// <summary>
    /// Microsoft compatible <see cref="ILogger"/> implementation for the Logbook logging system.
    /// </summary>
    internal class MicrosoftLogger : ILogger
    {
        private readonly string _categoryName;

        public MicrosoftLogger(string categoryName)
        {
            _categoryName = categoryName;
        }

        /// <inheritdoc cref="ILogger.BeginScope{TState}(TState)"/>
        [HideInCallstack]
        public IDisposable BeginScope<TState>(TState state) where TState : notnull => null;

        /// <inheritdoc cref="ILogger.IsEnabled(LogLevel)"/>
        /// <remarks>This implementation always returns true. We perform log level filtering in the core logger.</remarks>
        [HideInCallstack]
        public bool IsEnabled(LogLevel logLevel) => true;

        /// <inheritdoc cref="ILogger.Log{TState}(LogLevel, EventId, TState, Exception, Func{TState, Exception, string})"/>
        [HideInCallstack]
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);
            message = $"{_categoryName}: {message}";

            switch (logLevel)
            {
                case LogLevel.Trace:
                case LogLevel.Debug:
                    Logging.Debug(message);
                    break;
                case LogLevel.Information:
                    Logging.Info(message);
                    break;
                case LogLevel.Warning:
                    Logging.Warning(message);
                    break;
                case LogLevel.Error:
                case LogLevel.Critical:
                    Logging.Error(message);
                    break;
                case LogLevel.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel), logLevel, null);
            }
        }
    }

    /// <inheritdoc cref="MicrosoftLogger"/>
    /// <typeparam name="TCategory">Type to use as the category name.</typeparam>
    internal sealed class MicrosoftLogger<TCategory> : MicrosoftLogger, ILogger<TCategory>
    {
        public MicrosoftLogger() : base(typeof(TCategory).Name) { }
    }
}
