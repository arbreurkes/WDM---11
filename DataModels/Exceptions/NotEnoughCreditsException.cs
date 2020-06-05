using System;
using System.Runtime.Serialization;

namespace DataModels
{
    [Serializable]
    public class NotEnoughCreditsException : Exception
    {
        public NotEnoughCreditsException()
        {
        }

        public NotEnoughCreditsException(string message) : base(message)
        {
        }

        public NotEnoughCreditsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotEnoughCreditsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}