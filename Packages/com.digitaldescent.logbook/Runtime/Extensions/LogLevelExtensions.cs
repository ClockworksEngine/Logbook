// Copyright Digital Descent, All rights reserved.

using System;
using Microsoft.Extensions.Logging;
using Serilog.Events;

#nullable enable
namespace DigitalDescent.Logbook.Extensions
{
    /// <summary>
    /// Static extension methods for <see cref="LogLevel"/>.
    /// </summary>
    internal static class LogLevelExtensions
    {
        /// <summary>
        /// Converts a Microsoft <see cref="LogLevel"/> to a Serilog <see cref="LogEventLevel"/>.
        /// </summary>
        /// <param name="level">Level to convert.</param>
        /// <returns>Converted type.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unknown event level is provided.</exception>
        internal static LogEventLevel ToEventLevel(this LogLevel level) =>
            level switch
            {
                LogLevel.Trace => LogEventLevel.Verbose,
                LogLevel.Debug => LogEventLevel.Debug,
                LogLevel.Information => LogEventLevel.Information,
                LogLevel.Warning => LogEventLevel.Warning,
                LogLevel.Error => LogEventLevel.Error,
                LogLevel.Critical => LogEventLevel.Fatal,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null),
            };

        /// <summary>
        /// Converts a Microsoft <see cref="LogLevel"/> to a Unity <see cref="LogType"/>.
        /// </summary>
        /// <param name="level">Level to convert.</param>
        /// <returns>Converted type.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unknown event level is provided.</exception>
        internal static LogLevel ToLogType(this LogLevel level) =>
            level switch
            {
                LogLevel.Trace => LogLevel.Debug,
                LogLevel.Debug => LogLevel.Debug,
                LogLevel.Information => LogLevel.Information,
                LogLevel.Warning => LogLevel.Warning,
                LogLevel.Error => LogLevel.Error,
                LogLevel.Critical => LogLevel.Critical,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null),
            };

        /// <summary>
        /// Gets the corresponding <see cref="ConsoleColor"> for a given <see cref="LogLevel">.
        /// </summary>
        /// <param name="level">Level to look up.</param>
        /// <returns>Matching <see cref="ConsoleColor"/></returns>
        internal static ConsoleColor GetLevelColor(this LogLevel level) => level.ToEventLevel().GetLevelColor();
    }
}
