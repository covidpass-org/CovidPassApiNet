using System;
using System.Runtime.Serialization;

namespace CovidPass_API.Exceptions
{
    public class CertificateNotLoadedException : Exception
    {
        public CertificateNotLoadedException()
        {
        }

        protected CertificateNotLoadedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public CertificateNotLoadedException(string message) : base(message)
        {
        }

        public CertificateNotLoadedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}