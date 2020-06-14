using System;
using System.Runtime.Serialization;

namespace DataModels
{
    [Serializable]
    public class OrderDoesNotExistsException : Exception
    {
        public OrderDoesNotExistsException()
        {
        }

        public OrderDoesNotExistsException(string message) : base(message)
        {
        }

        public OrderDoesNotExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected OrderDoesNotExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}