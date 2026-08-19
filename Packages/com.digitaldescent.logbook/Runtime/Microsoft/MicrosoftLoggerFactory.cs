// Copyright Digital Descent, All rights reserved.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace DigitalDescent.Logbook.Microsoft
{
    /// <summary>
    /// Microsoft compatible <see cref="ILoggerFactory"/> implementation for the Logbook logging system.
    /// </summary>
    internal sealed class MicrosoftLoggerFactory : ILoggerFactory
    {
        private readonly IDictionary<string, ILogger> _loggers;

        public MicrosoftLoggerFactory()
        {
            _loggers = new Dictionary<string, ILogger>();
        }

        /// <inheritdoc cref="ILoggerFactory.AddProvider(ILoggerProvider)"/>
        public void AddProvider(ILoggerProvider provider) => throw new NotSupportedException("Adding providers is not supported in this implementation.");

        /// <inheritdoc cref="ILoggerFactory.CreateLogger(string)"/>
        public ILogger CreateLogger(string categoryName)
        {
            if (_loggers.TryGetValue(categoryName, out ILogger logger))
                return logger;

            logger = new MicrosoftLogger(categoryName);
            return logger;
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose() { }
    }
}
