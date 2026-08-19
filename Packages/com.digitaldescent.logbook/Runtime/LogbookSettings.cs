// Copyright Digital Descent, All rights reserved.

using System.Collections.Generic;
using DigitalDescent.Logbook.Framework;
using DigitalDescent.Logbook;
using NUnit.Framework;
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
        /// Flag indicating whether to use colors in the logging output. 
        /// If set to true, log messages will be color-coded based on their severity level.
        /// </summary>
        public bool UseLoggingColors = !Application.isEditor;

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
    }
}
