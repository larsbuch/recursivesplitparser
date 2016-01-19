using System;
using System.Runtime.Serialization;

namespace RecursiveSplitParser
{
    [Serializable]
    public class GuardClausesException : Exception
    {
        public GuardClausesException()
        {
        }

        public GuardClausesException(string message) : base(message)
        {
        }

        public GuardClausesException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected GuardClausesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}