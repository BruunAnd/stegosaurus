using System;
using System.Collections.Generic;
using Stegosaurus.Carrier;
using Stegosaurus.Cryptography;
using Stegosaurus.Algorithm.GraphTheory;
using System.Collections;
using System.Linq;
using Stegosaurus.Utility;
using Stegosaurus.Exceptions;
using System.Windows.Forms;

namespace Stegosaurus.Algorithm
{
    public class GraphTheoreticAlgorithm : IStegoAlgorithm
    {
        public ICarrierMedia CarrierMedia
        {
            get; set;
        }

        public ICryptoProvider CryptoProvider
        {
            get; set;
        }

        public string Name => "GTA";

        public long ComputeBandwidth()
        {
            return 100000;
        }

        public List<Vertex> GetAllVertices()
        {
            var allVertices = new List<Vertex>(CarrierMedia.ByteArray.Length % CarrierMedia.BytesPerSample);
            int currentSample = 0, sampleCount = 0;
            while (currentSample < CarrierMedia.ByteArray.Length)
            {
                var sampleStartPosition = currentSample;
                var samples = new byte[CarrierMedia.BytesPerSample];

                for (int i = 0; i < CarrierMedia.BytesPerSample; i++)
                    samples[i] = CarrierMedia.ByteArray[currentSample++];

                allVertices.Add(new Vertex(sampleCount++, samples));
            }
            return allVertices;
        }

        public void Embed(StegoMessage message)
        {
            var messageBits = new BitArray(message.ToByteArray(CryptoProvider));
            Console.WriteLine("{0} bits", messageBits.Length);

            // TODO fix
            if (messageBits.Length % 3 != 0)
            {
                throw new StegoAlgorithmException("Invalid size.");
            }

            // Get all vertices in carrierMedia
            var allVertices = GetAllVertices();
            Console.WriteLine("{0} vertices", allVertices.Count);

            // Assign target values to vertices
            List<Vertex> verticesToChange = new List<Vertex>();
            var rnl = new RandomNumberList(1000, allVertices.Count);
            for (int i = 0; i < messageBits.Length; i++)
            {
                Vertex nextVertex = allVertices[rnl.First()];
                nextVertex.TargetValue = new bool[] {messageBits[i], messageBits[i++], messageBits[i++]};

                verticesToChange.Add(nextVertex);
            }

            Console.WriteLine("{0} vertices with target values", verticesToChange.Count);

            List<Edge> edges = new List<Edge>();
            // Find edges
            foreach (Vertex outer in verticesToChange)
            {
                if (outer.IsInEdge)
                    continue;

                foreach (Vertex inner in verticesToChange)
                {
                    if (inner.IsInEdge || outer == inner)
                        continue;

                    if (outer.HasMatchingBits(inner) && inner.HasMatchingBits(outer))
                    {
                        edges.Add(new Edge(inner, outer));
                        Console.WriteLine("{0} edges", edges.Count);
                    }
                }
            }

            // Swap edges
            foreach (Edge edge in edges)
            {
                Swap(allVertices, edge.First.Position, edge.Second.Position);
            }

            verticesToChange.ForEach(v => v.ForceChanges());

            // Write changes
            int pos = 0;
            foreach (Vertex current in allVertices)
            {
                foreach (byte sample in current.Samples)
                {
                    CarrierMedia.ByteArray[pos++] = sample;
                }
            }
        }

        private void Swap<T>(List<T> list, int first, int second)
        {
            T temp = list[first];
            list[first] = list[second];
            list[second] = temp;
        }

        public StegoMessage Extract()
        {
            var allVertices = GetAllVertices();
            return null;
        }
    }

    class Pixel
    {
        public byte[] Samples;
    }
}
