using System;

namespace Androtomist.Models.Forms
{
    public class FormDataException : Exception
    {
        public FormDataException()
        {
        }

        public FormDataException(string message)
            : base(message)
        {
        }

        public FormDataException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}