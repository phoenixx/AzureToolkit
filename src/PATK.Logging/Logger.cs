using System;
using PATK.Logging.Sink;
using Serilog;

namespace PATK.Logging
{
    /// <summary>
    /// Implementation of <see cref="ILogger"/> using serilog and nlog
    /// </summary>
    public class Logger : ILogger
    {
        private readonly Serilog.ILogger _logger;

        public Logger()
        {
            _logger = new LoggerConfiguration().WriteTo.NLog().MinimumLevel.Verbose().CreateLogger();
        }

        public void Error(string format, params object[] args)
        {
            _logger.Error(format, args);
        }

        public void Error(string message)
        {
            _logger.Error(message);
        }

        public void Error(Exception ex, string format, params object[] args)
        {
            _logger.Error(ex, format, args);
        }

        public void Information(string message)
        {
            _logger.Information(message);
        }

        public void Information(string format, params object[] args)
        {
            _logger.Information(format, args);
        }

        public void Warning(string message)
        {
            _logger.Warning(message);
        }

        public void Warning(string format, params object[] args)
        {
            _logger.Warning(format, args);
        }

        public void Trace(string message)
        {
            _logger.Verbose(message);
        }

        public void Trace(string format, params object[] args)
        {
            _logger.Verbose(format, args);
        }

        public void Debug(string message)
        {
            _logger.Debug(message);
        }

        public void Debug(string format, params object[] args)
        {
            _logger.Debug(format, args);
        }
    }
}