using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stegosaurus.Carrier;
using Stegosaurus.Cryptography;

namespace Stegosaurus.Algorithm
{
    public class GraphTheoreticAlgorithm : IStegoAlgorithm
    {
        public ICryptoProvider CryptoProvider { get; set; }

        public ICarrierMedia CarrierMedia
        {
            get; set;
        }

        public string Name => "Graph Theoretic Algorithm";
        public string CryptoKey { get; set; }

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
