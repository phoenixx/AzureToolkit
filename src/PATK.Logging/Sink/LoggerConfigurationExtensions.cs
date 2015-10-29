using System;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace PATK.Logging.Sink
{
    public static class LoggerConfigurationExtensions
    {
        public static LoggerConfiguration NLog(
            this LoggerSinkConfiguration loggerConfiguration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            IFormatProvider formatProvider = null)
        {
            if (loggerConfiguration == null)
            {
                throw new ArgumentException("loggerConfiguration");
            }

            return loggerConfiguration.Sink(new NLogSink(formatProvider), restrictedToMinimumLevel);
        }
    }
}