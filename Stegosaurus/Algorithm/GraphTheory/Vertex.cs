using System;
using System.Collections.Generic;

namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Vertex
    {
        public byte[] Samples;
        public bool[] TargetValue;

        private const int SelectedBit = 0x1;

        public int Position;

        public Vertex(int _positionInCarrier)
        {
            Position = _positionInCarrier;
        }

        public bool HasMatchingBits(Vertex other)
        {
            if (Samples.Length != TargetValue.Length)
                return false;

            for (var i = 0; i < Samples.Length; i++)
            {
                if (((Samples[i] & SelectedBit) == SelectedBit) != other.TargetValue[i])
                    return false;
            }

            //Console.WriteLine("{0}, {1}, {2} matches with {3}, {4}, {5}", newSamples[0], newVertex.Samples[1], newVertex.Samples[2], newVertex.TargetValue[0], newVertex.TargetValue[1], newVertex.TargetValue[2]);

            return true;
        }
    }
}
