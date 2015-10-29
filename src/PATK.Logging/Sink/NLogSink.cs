using System;
using NLog;
using Serilog.Core;
using Serilog.Debugging;
using Serilog.Events;

namespace PATK.Logging.Sink
{
    internal class NLogSink : ILogEventSink
    {
        private readonly IFormatProvider _formatProvider;

        public NLogSink(IFormatProvider formatProvider = null)
        {
            _formatProvider = formatProvider;
        }

        public void Emit(LogEvent logEvent)
        {
            var loggerName = "Default";

            LogEventPropertyValue sourceContext;
            if (logEvent.Properties.TryGetValue(Constants.SourceContextPropertyName, out sourceContext))
            {
                var sv = sourceContext as ScalarValue;
                if (sv?.Value is string)
                {
                    loggerName = (string) sv.Value;
                }
            }

            var level = MapLogLevel(logEvent);
            var message = logEvent.RenderMessage(_formatProvider);
            var exception = logEvent.Exception;

            var nlogEvent = new LogEventInfo(level, loggerName, message)
            {
                Exception = exception
            };

            foreach (var property in logEvent.Properties)
            {
                var sv = property.Value as ScalarValue;
                var format = (sv?.Value is string) ? "l" : null;

                nlogEvent.Properties[property.Key] = property.Value.ToString(format, null);
            }

            var logger = LogManager.GetLogger(loggerName);
            logger.Log(nlogEvent);
        }

        private static LogLevel MapLogLevel(LogEvent logEvent)
        {
            switch (logEvent.Level)
            {
                case LogEventLevel.Debug:
                    return LogLevel.Debug;
                case LogEventLevel.Error:
                    return LogLevel.Error;
                case LogEventLevel.Fatal:
                    return LogLevel.Fatal;
                case LogEventLevel.Information:
                    return LogLevel.Info;
                case LogEventLevel.Verbose:
                    return LogLevel.Trace;
                case LogEventLevel.Warning:
                    return LogLevel.Warn;
                default:
                    SelfLog.WriteLine("Unexpected logging level {0}. Using Info level.", logEvent.Level);
                    return LogLevel.Info;
            }
        }
    }
}
