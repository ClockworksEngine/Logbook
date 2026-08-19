// Copyright Digital Descent, All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Serilog.Context;
using Serilog.Events;

#nullable enable
namespace DigitalDescent.Logbook.Capture
{
    /// <summary>
    /// <see cref="IDisposable"/> for capturing log events within a specific scope. 
    /// When an instance of this class is created, it starts capturing log events, and when it is disposed, 
    /// it stops capturing and provides access to the captured events.
    /// </summary>
    public sealed class LoggingCaptureScope : IDisposable
    {
        private const string CaptureIdPropertyName = "CaptureId";
        private readonly string _captureId;
        private readonly IDisposable _contextScope;
        private readonly LoggingCaptureSink _sink;
        private readonly DateTimeOffset _startTime;

        /// <summary>
        /// List of all log events captured during the scope of this instance. 
        /// This list is populated when the scope is disposed.
        /// </summary>
        public List<LogEvent> CapturedEvents { get; } = new();

        internal LoggingCaptureScope(LoggingCaptureSink sink)
        {
            _sink = sink;
            _captureId = Guid.NewGuid().ToString();
            _startTime = DateTimeOffset.UtcNow;

            _contextScope = LogContext.PushProperty(CaptureIdPropertyName, _captureId);
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            _contextScope.Dispose();
            var events = _sink.GetEvents()
                .Where(e => e.Timestamp >= _startTime &&
                            e.Properties.TryGetValue(CaptureIdPropertyName, out var captureId) &&
                            captureId.ToString().Contains(_captureId));

            CapturedEvents.AddRange(events);
        }
    }
}
