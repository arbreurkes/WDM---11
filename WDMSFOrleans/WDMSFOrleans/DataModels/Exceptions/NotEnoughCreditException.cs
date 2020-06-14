using System;
using System.Runtime.Serialization;

namespace DataModels
{
    [Serializable]
    public class NotEnoughCreditException : Exception
    {
        public NotEnoughCreditException()
        {
        }

        public NotEnoughCreditException(string message) : base(message)
        {
        }

        public NotEnoughCreditException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NotEnoughCreditException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}