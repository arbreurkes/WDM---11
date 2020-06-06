using System;
using System.Runtime.Serialization;

namespace Grains
{
    [Serializable]
    public class ItemNotInOrderException : Exception
    {
        public ItemNotInOrderException()
        {
        }

        public ItemNotInOrderException(string message) : base(message)
        {
        }

        public ItemNotInOrderException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ItemNotInOrderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}