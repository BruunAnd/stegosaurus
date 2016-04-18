﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Exceptions
{
    public class InvalidWaveFileException : InvalidFileException
    {
        public InvalidWaveFileException(string _message, string _fileName)
            : base($"WAVE file was invalid. {_message}", _fileName)
        {
        }
    }
}
