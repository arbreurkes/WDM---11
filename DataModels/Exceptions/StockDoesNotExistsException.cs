using System;
using System.Runtime.Serialization;

namespace OrleansBasics
{
    [Serializable]
    internal class StockDoesNotExistsException : Exception
    {
        public StockDoesNotExistsException()
        {
        }

        public StockDoesNotExistsException(string message) : base(message)
        {
        }

        public StockDoesNotExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StockDoesNotExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}