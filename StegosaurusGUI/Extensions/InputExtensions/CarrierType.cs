using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus.Extensions.InputExtensions
{
    public class CarrierType : IInputType
    {
        public string FilePath { get; set; }

        public CarrierType(string _filePath)
        {
            FilePath = _filePath;
        }
        
    }
}
