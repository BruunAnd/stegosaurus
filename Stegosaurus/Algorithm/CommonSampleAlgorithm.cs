using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stegosaurus.Carrier;
using Stegosaurus.Cryptography;
using Stegosaurus.Algorithm.GraphTheory;
using Stegosaurus.Utility;
using System.Collections;
using Stegosaurus.Exceptions;

namespace Stegosaurus.Algorithm
{
    public class CommonSampleAlgorithm : IStegoAlgorithm
    {
        public ICarrierMedia CarrierMedia
        {
            get;set;
        }

        public ICryptoProvider CryptoProvider
        {
            get; set;
        }

        public string Name => "Common Sample";

        public long ComputeBandwidth()
        {
            return 1000000;
        }

        public List<Vertex> GetAllVertices()
        {
            var allVertices = new List<Vertex>(CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample);
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
            if (messageBits.Length % 3 != 0)
                throw new StegoAlgorithmException("Wrong length");

            // Get all vertices
            var vertices = GetAllVertices();

            // Find color frequencies
            var colorFrequencies = vertices
                .GroupBy(v => v.Samples, new ArrayComparer<byte>())
                .OrderByDescending(x => x.Count())
                .Where(x => x.Count() > 15)
                .Select(x => x.Key).ToList();
            Console.WriteLine("{0} unique colors", colorFrequencies.Count);

            // Find vertices to change
            List<Vertex> updateVertices = new List<Vertex>();
            var randomNumbers = new RandomNumberList(CryptoProvider.Seed, vertices.Count);
            int replaced = 0, forced = 0;
            for (int i = 0; i < messageBits.Length; i++)
            {
                Vertex nextVertex = vertices[randomNumbers.First()];
                nextVertex.TargetValue = new bool[] {messageBits[i], messageBits[i++], messageBits[i++]};

                // Find matching color
                int bestDistance = 0;
                byte[] bestSample = null;
                foreach (byte[] colorSamples in colorFrequencies)
                {
                    // Do bits match?
                    if (!nextVertex.HasMatchingBits(colorSamples))
                        continue;

                    int distance = nextVertex.DistanceTo(colorSamples);
                    if (bestSample == null || distance < bestDistance)
                    {
                        bestDistance = distance;
                        bestSample = colorSamples;
                    }
                    // Is distance good?
                    if (nextVertex.DistanceTo(colorSamples) > 100)
                        continue;
                }

                // Force overwrite if no match
                if (bestSample == null)
                {
                    forced++;
                    nextVertex.ForceChanges();
                }
                else
                {
                    nextVertex.Samples = bestSample;
                    replaced++;
                }
            }

            Console.WriteLine("{0} forced, {1} replaced", forced, replaced);

            // Write changes
            int pos = 0;
            foreach (Vertex current in vertices)
            {
                foreach (byte sample in current.Samples)
                {
                    CarrierMedia.ByteArray[pos++] = sample;
                }
            }
        }

        public StegoMessage Extract()
        {
            throw new NotImplementedException();
        }
    }

    class ArrayComparer<T> : IEqualityComparer<T[]>
    {
        public bool Equals(T[] x, T[] y)
        {
            return x.SequenceEqual(y);
        }

        public int GetHashCode(T[] obj)
        {
            return obj.Aggregate(string.Empty, (s, i) => s + i.GetHashCode(), s => s.GetHashCode());
        }
    }
}
