using System;
using System.Runtime.Serialization;

namespace DataModels
{
    [Serializable]
    public class UserDoesNotExistsException : Exception
    {
        public UserDoesNotExistsException()
        {
        }

        public UserDoesNotExistsException(string message) : base(message)
        {
        }

        public UserDoesNotExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserDoesNotExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}