// Copyright Digital Descent, All rights reserved.

#nullable enable
namespace DigitalDescent.Logbook
{
    /// <summary>
    /// Constants for working with the Logbook logging system.
    /// </summary>
    internal static class LogbookConstants
    {
        /// <summary>
        /// Default name used for logging messages that don't have a specified caller.
        /// </summary>
        public const string DefaultCallerName = "UnityEngine";

        /// <summary>
        /// Template used when logging to the console.
        /// </summary>
        public const string ConsoleTemplate = "{Timestamp:dd-MM hh:mm:ss tt} [{Caller}] [{Level:u3}] {Message:lj}{NewLine}{Exception}";
    }
}
