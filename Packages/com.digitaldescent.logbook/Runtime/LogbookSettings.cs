// Copyright Digital Descent, All rights reserved.

using System;
using System.Collections.Generic;
using DigitalDescent.Logbook.Framework;
using Serilog.Events;
using UnityEngine;

#nullable enable
namespace DigitalDescent.Logbook
{
    /// <summary>
    /// Configuration options for the <see cref="Logging"/> class.
    /// </summary>
    [CreateAssetMenu(fileName = "Logbook Settings", menuName = "Logbook/Settings", order = 0)]
    public sealed class LogbookSettings : SingletonAsset<LogbookSettings>
    {
        /// <summary>
        /// Gets or sets the minimum log level for the logging system. Log messages below this level will be ignored.
        /// </summary>
        /// <remarks>
        /// When running inside the Unity editor this defaults to <see cref="LogEventLevel.Debug"/>. Otherwise,
        /// The default value will be <see cref="LogEventLevel.Information"/>
        /// </remarks>
        public LogEventLevel MinimumLevel = Application.isEditor ? LogEventLevel.Debug : LogEventLevel.Information;

        /// <summary>
        /// Configured log targets for the logging system. Each target represents a destination for log messages, 
        /// such as a file, console, or external service.
        /// </summary>
        public List<LogbookTarget> Targets = new();

        /// <summary>
        /// Flag indicating whether to use colors in the logging output. 
        /// If set to true, log messages will be color-coded based on their severity level.
        /// </summary>
        [Header("Logging Colors")]
        public bool UseLoggingColors = !Application.isEditor;

        /// <summary>
        /// <see cref="ConsoleColor"/> to use when logging Verbose messages.
        /// </summary>
        [Tooltip("The color to use for Verbose log messages.")]
        public ConsoleColor VerboseColor = ConsoleColor.Magenta;

        /// <summary>
        /// <see cref="ConsoleColor"/> to use when logging Debug messages.
        /// </summary>
        [Tooltip("The color to use for Debug log messages.")]
        public ConsoleColor DebugColor = ConsoleColor.Gray;

        /// <summary>
        /// <see cref="ConsoleColor"/> to use when logging Information messages.
        /// </summary>
        [Tooltip("The color to use for Information log messages.")]
        public ConsoleColor InformationColor = ConsoleColor.White;

        /// <summary>
        /// <see cref="ConsoleColor"/> to use when logging Warning messages.
        /// </summary>
        [Tooltip("The color to use for Warning log messages.")]
        public ConsoleColor WarningColor = ConsoleColor.Yellow;

        /// <summary>
        /// <see cref="ConsoleColor"/> to use when logging Error messages.
        /// </summary>
        [Tooltip("The color to use for Error log messages.")]
        public ConsoleColor ErrorColor = ConsoleColor.Red;

        /// <summary>
        /// <see cref="ConsoleColor"/> to use when logging Fatal messages.
        /// </summary>
        [Tooltip("The color to use for Fatal log messages.")]
        public ConsoleColor FatalColor = ConsoleColor.Red;
    }
}
