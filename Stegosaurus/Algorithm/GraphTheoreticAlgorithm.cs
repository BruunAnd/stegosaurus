using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stegosaurus.Carrier;

namespace Stegosaurus.Algorithm
{
    public class GraphTheoreticAlgorithm : IStegoAlgorithm
    {
        public ICarrierMedia CarrierMedia
        {
            get; set;
        }

        public byte[] Key
        {
            get; set;
        }

        public string Name => "Graph Theoretic Algorithm";

        public long ComputeBandwidth()
        {
            throw new NotImplementedException();
        }

        public void Embed(StegoMessage message)
        {
            throw new NotImplementedException();
        }

        public StegoMessage Extract()
        {
            throw new NotImplementedException();
        }
    }
}
