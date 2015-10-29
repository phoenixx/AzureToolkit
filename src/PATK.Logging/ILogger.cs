using System;

namespace PATK.Logging
{
    /// <summary>
    /// Interface for logging events, errors, warnings etc.
    /// </summary>
    public interface ILogger
    {
        void Error(string format, params object[] args);
        void Error(string message);
        void Error(Exception ex, string format, params object[] args);
        void Information(string message);
        void Information(string format, params object[] args);
        void Warning(string message);
        void Warning(string format, params object[] args);
        void Trace(string message);
        void Trace(string format, params object[] args);
        void Debug(string message);
        void Debug(string format, params object[] args);
    }
}