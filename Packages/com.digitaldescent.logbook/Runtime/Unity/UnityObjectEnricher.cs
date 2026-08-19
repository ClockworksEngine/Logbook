// Copyright Digital Descent, All rights reserved.

using Serilog;
using Serilog.Core;
using Serilog.Events;

#nullable enable
namespace DigitalDescent.Logbook.Unity
{
    /// <summary>
    /// 
    /// </summary>
    internal sealed class UnityObjectEnricher : ILogEventEnricher
    {
        public const string UnityContextKey = "%_DO_NOT_USE_UNITY_ID_DO_NOT_USE%";
        private readonly LogEventProperty _property;

        public UnityObjectEnricher(UnityEngine.Object? context) => _property = new LogEventProperty(UnityContextKey, new ScalarValue(context));
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory) => logEvent.AddPropertyIfAbsent(_property);
    }

    /// <summary>
    /// Static extension methods for <see cref="UnityObjectEnricher"/>
    /// </summary>
    public static class UnityObjectEnricherExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static ILogger WithUnityObject(this ILogger logger, UnityEngine.Object? context) => logger.ForContext(new UnityObjectEnricher(context));
    }
}
