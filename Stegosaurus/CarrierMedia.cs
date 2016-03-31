using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus
{
    abstract class CarrierMedia
    {
        protected byte[] _innerArray;

        public abstract void Encode();
        public abstract void Decode();

        public virtual void SaveToFile(string destination)
        {
            Encode();
            /* Do stuff.. */
        }
    }
}
