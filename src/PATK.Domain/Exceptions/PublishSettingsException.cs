using System;

namespace PATK.Domain.Exceptions
{
    public class PublishSettingsException : Exception
    {
        public PublishSettingsException() : base() {}

        public PublishSettingsException(string message) : base(message) { }

        public PublishSettingsException(string format, params object[] args) : base(string.Format(format, args)) { }

        public PublishSettingsException(string message, Exception innerException) : base(message, innerException) { }

        public PublishSettingsException(string format, Exception innerException, params object[] args) :
            base(string.Format(format, args), innerException) { }
    }
}