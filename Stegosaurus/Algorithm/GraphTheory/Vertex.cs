using System;
using System.Collections.Generic;
using System.Linq;

namespace Stegosaurus.Algorithm.GraphTheory
{
    public class Vertex
    {
        public byte[] Samples;
        public bool[] TargetValue;

        public int TargetMod;

        private const int SelectedBit = 0x1;

        public bool IsInEdge;

        public int Position;

        public Vertex(int _positionInCarrier, byte[] _samples)
        {
            Position = _positionInCarrier;
            Samples = _samples;
        }

        public void ForceChanges()
        {
            Samples[0] ^= 0x1;
        }

        public bool HasMatchingBits(byte[] samples)
        {
            for (var i = 0; i < samples.Length; i++)
            {
                if (i + 1 >= TargetValue.Length)
                    return true;
                if (((samples[i] & SelectedBit ) == SelectedBit) != TargetValue[i])
                    return false;
            }

            return true;
        }

        public int DistanceTo(byte[] samples)
        {
            int distance = 0;

            for (int i = 0; i < Samples.Length; i++)
            {
                distance += (int) Math.Pow(Samples[i] - samples[i], 2);
            }

            return distance;
        }

        public bool HasMatchingBits(Vertex other)
        {
            return HasMatchingBits(other.Samples);
        }
    }
}
