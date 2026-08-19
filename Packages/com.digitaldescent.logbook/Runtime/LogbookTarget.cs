// Copyright Digital Descent, All rights reserved.

using Serilog;
using UnityEngine;

#nullable enable
namespace DigitalDescent.Logbook
{
    /// <summary>
    /// Base object for implementing custom Logbook logging targets.
    /// </summary>
    public abstract class LogbookTarget : ScriptableObject
    {
        /// <summary>
        /// Name of the logbook target.
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Invoked by the primary logging class to initialize the target.
        /// </summary>
        /// <param name="config">Logging config to use.</param>
        public abstract void Initialize(LoggerConfiguration config);
    }
}
