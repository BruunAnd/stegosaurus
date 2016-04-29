using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Exceptions
{
    public class InvalidImageFileException : InvalidFileException
    {
        public InvalidImageFileException(string _message, string _fileName)
            : base($"Image file was invalid. {_message}", _fileName)
        {
        }
    }
}
