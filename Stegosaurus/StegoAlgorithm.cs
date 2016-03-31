using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stegosaurus
{
    abstract class StegoAlgorithm
    {

        public StegoCarrier Carrier { get; set; }

        public abstract long ComputeBandwidth();

        public abstract void Embed(StegoMessage message);

        public abstract StegoMessage Extract();
    }
}
