using System;

namespace SimpleCarCatalogue.Exceptions
{
    public class ObjectNotExistsException : Exception
    {
        public ObjectNotExistsException()
        {

        }

        public ObjectNotExistsException(string message)
            :base(message)
        {

        }
    }
}
