// Copyright Digital Descent, All rights reserved.

using System;
using Microsoft.Extensions.Logging;
using Serilog.Events;
using UnityEngine;

#nullable enable
namespace DigitalDescent.Logbook.Extensions
{
    /// <summary>
    /// Static extension methods for working with <see cref="LogType"/> values.
    /// </summary>
    internal static class LogTypeExtensions
    {
        /// <summary>
        /// Converts a Unity <see cref="LogType"/> to a Microsoft <see cref="LogLevel"/>.
        /// </summary>
        /// <param name="level">Level to convert.</param>
        /// <returns>Converted type.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unknown event level is provided.</exception>
        internal static LogLevel ToLogLevel(this LogType level)
        {
            return level switch
            {
                LogType.Assert => LogLevel.Debug,
                LogType.Log => LogLevel.Information,
                LogType.Warning => LogLevel.Warning,
                LogType.Error => LogLevel.Error,
                LogType.Exception => LogLevel.Critical,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null),
            };
        }

        /// <summary>
        /// Converts a Unity <see cref="LogType"/> to a Serilog <see cref="LogEventLevel"/>.
        /// </summary>
        /// <param name="level">Level to convert.</param>
        /// <returns>Converted type.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unknown event level is provided.</exception>
        internal static LogEventLevel ToLogEventLevel(this LogType level)
        {
            return level switch
            {
                LogType.Assert => LogEventLevel.Debug,
                LogType.Log => LogEventLevel.Information,
                LogType.Warning => LogEventLevel.Warning,
                LogType.Error => LogEventLevel.Error,
                LogType.Exception => LogEventLevel.Fatal,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null),
            };
        }

        /// <summary>
        /// Gets the corresponding <see cref="ConsoleColor"> for a given <see cref="LogType">.
        /// </summary>
        /// <inheritdoc cref="GetLevelColor(LogLevel)"/>
        internal static ConsoleColor GetLevelColor(this LogType level) => level.ToLogLevel().GetLevelColor();

    }
}
