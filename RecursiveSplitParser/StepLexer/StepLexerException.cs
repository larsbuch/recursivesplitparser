using System;
using System.Runtime.Serialization;

namespace StepLexer
{
    [Serializable]
    internal class StepLexerException : Exception
    {
        public StepLexerException()
        {
        }

        public StepLexerException(string message) : base(message)
        {
        }

        public StepLexerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StepLexerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}