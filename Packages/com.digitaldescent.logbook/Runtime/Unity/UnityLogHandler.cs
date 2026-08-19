// Copyright Digital Descent, All rights reserved.

using System;
using UnityEngine;

#nullable enable
namespace DigitalDescent.Logbook.Unity
{
    /// <summary>
    /// Custom <see cref="ILogHandler"/> implementation for passing through
    /// unmodified <see cref="UnityEngine.Debug"/> logging calls to the <see cref="Logging"/> system.
    /// </summary>
    internal sealed class UnityLogHandler : ILogHandler
    {
        /// <inheritdoc cref="ILogHandler.LogException(Exception, UnityEngine.Object)"/>
        [HideInCallstack]
        public void LogException(Exception exception, UnityEngine.Object? context) =>
            Logging.Exception(
                exception: exception,
                context: context,
                callerFile: Logging.DefaultLoggerName);

        /// <inheritdoc cref="ILogHandler.LogFormat(LogType, UnityEngine.Object, string, object[])"/>
        [HideInCallstack]
        public void LogFormat(LogType logType, UnityEngine.Object? context, string format, params object[] args) =>
            Logging.Log(
                logType: logType,
                message: string.Format(format, args),
                context: context,
                callerFile: Logging.DefaultLoggerName);
    }
}