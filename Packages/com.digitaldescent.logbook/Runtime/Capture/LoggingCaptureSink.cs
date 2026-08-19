// Copyright Digital Descent, All rights reserved.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Serilog.Core;
using Serilog.Events;

#nullable enable
namespace DigitalDescent.Logbook.Capture
{
    /// <summary>
    /// <see cref="ILogEventSink"/> for capturing log events in a thread-safe manner. 
    /// Used by the <see cref="LoggingCaptureScope"/> object.
    /// </summary>
    internal sealed class LoggingCaptureSink : ILogEventSink
    {
        private readonly ConcurrentQueue<(DateTimeOffset Expiry, LogEvent Event)> _queue = new();
        private readonly TimeSpan _maxAge = TimeSpan.FromMinutes(5);

        /// <inheritdoc cref="ILogEventSink.Emit(LogEvent)"/>
        public void Emit(LogEvent logEvent)
        {
            // Add the log event to the queue with an expiry time based on the configured maximum age.
            var now = DateTimeOffset.UtcNow;
            _queue.Enqueue((now.Add(_maxAge), logEvent));

            // Perform a cleanup of expired log events to prevent memory bloat. 
            while (_queue.TryPeek(out var oldest) && now > oldest.Expiry)
                _queue.TryDequeue(out _);
        }

        /// <summary>
        /// Gets the captured log events that are still within the specified maximum age. 
        /// This method returns an enumerable of log events that have not expired based on the configured 
        /// maximum age.
        /// </summary>
        /// <returns><see cref="IEnumerable{T}"/> of captured <see cref="LogEvent"/> objects.</returns>
        public IEnumerable<LogEvent> GetEvents() => _queue.Select(q => q.Event);
    }
}
