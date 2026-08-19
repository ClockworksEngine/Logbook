// Copyright Digital Descent, All rights reserved.

using System;
using Serilog.Events;
using UnityEngine;

#nullable enable
namespace DigitalDescent.Logbook.Extensions
{
    /// <summary>
    /// Static extension methods for <see cref="LogEventLevel"/>.
    /// </summary>
    internal static class LogEventLevelExtensions
    {
        /// <summary>
        /// Gets the corresponding <see cref="ConsoleColor"> for a given <see cref="LogEventLevel">.
        /// </summary>
        /// <param name="level">Level to look up.</param>
        /// <returns>Match <see cref="ConsoleColor"/>.</returns>
        internal static ConsoleColor GetLevelColor(this LogEventLevel level) =>
            level switch
            {
                LogEventLevel.Verbose       => LogbookSettings.Instance.VerboseColor,
                LogEventLevel.Debug         => LogbookSettings.Instance.DebugColor,
                LogEventLevel.Information   => LogbookSettings.Instance.InformationColor,
                LogEventLevel.Warning       => LogbookSettings.Instance.WarningColor,
                LogEventLevel.Error         => LogbookSettings.Instance.ErrorColor,
                LogEventLevel.Fatal         => LogbookSettings.Instance.FatalColor,
                _                           => LogbookSettings.Instance.InformationColor,
            };

        /// <summary>
        /// Converts a Serilog <see cref="LogEventLevel"/> to a Unity <see cref="LogType"/>.
        /// </summary>
        /// <param name="level">Level to convert.</param>
        /// <returns>Converted type.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when an unknown event level is provided.</exception>
        internal static LogType ToLogType(this LogEventLevel level) =>
            level switch
            {
                LogEventLevel.Verbose => LogType.Log,
                LogEventLevel.Debug => LogType.Log,
                LogEventLevel.Information => LogType.Log,
                LogEventLevel.Warning => LogType.Warning,
                LogEventLevel.Error or LogEventLevel.Fatal => LogType.Error,
                _ => throw new ArgumentOutOfRangeException(nameof(level), level, null),
            };
    }
}
