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
        private static readonly byte[] CommonSampleSignature = new byte[] { 0x0C, 0xB3, 0x11, 0x84 };

        public ICarrierMedia CarrierMedia { get;set; }

        public ICryptoProvider CryptoProvider { get; set; }

        public string Name => "Common Sample";

        public long ComputeBandwidth()
        {
            return (CarrierMedia.ByteArray.Length / 8) - CommonSampleSignature.Length;
        }

        public void Embed(StegoMessage message)
        {
            var messageBits = new BitArray(message.ToByteArray(CryptoProvider));

            // Get all vertices
            var vertices = GetAllVertices();
            var minOccurrences = (int)Math.Sqrt(Math.Sqrt(vertices.Count));
            Console.WriteLine("{0} minimum", minOccurrences);

            // Find color frequencies
            var colorFrequencies = vertices
                .GroupBy(v => v.Samples, new ArrayComparer<byte>())
                //.OrderByDescending(x => x.Count())
                .Where(x => x.Count() > minOccurrences)
                .Select(x => x.Key).ToList();

            // Find vertices to change
            var randomNumbers = new RandomNumberList(CryptoProvider.Seed, vertices.Count);
            int replaced = 0, forced = 0;
            for (int i = 0; i < messageBits.Length; i++)
            {
                Vertex currentVertex = vertices[randomNumbers.First()];

                // Determine target value
                var targetValue = new bool[CarrierMedia.BytesPerSample];
                for (int j = 0; j < targetValue.Length; j++)
                    targetValue[j] = messageBits[i++];
                currentVertex.TargetValue = targetValue;

                // Find best match
                var possibleMatches = colorFrequencies
                    .Where(c => currentVertex.HasMatchingBits(c))
                    .OrderBy(c => currentVertex.DistanceTo(c));

                // Take first element


                // Find matching vertex
                bool foundMatch = false;
                foreach (byte[] colorSamples in colorFrequencies)
                {
                    // Do bits match?
                    if (!currentVertex.HasMatchingBits(colorSamples))
                        continue;

                    // Is distance ok?
                    int distance = currentVertex.DistanceTo(colorSamples);
                    if (distance < 10) // TODO determine better max distance?
                    {
                        currentVertex.Samples = colorSamples;
                        foundMatch = true;
                        replaced++;
                    }
                }

                // Force changes if no match
                if (!foundMatch)
                {
                    currentVertex.ForceChanges();
                    forced++;
                }

                // Set progress
                if (i % 501 != 0)
                    continue;
                var p = ((float) (i + 1) / messageBits.Length) * 100;
                Console.Title = $"{p}%";
            }

            Console.WriteLine("{0}% forced, {1}% replaced", 100 * ((float)forced / (forced + replaced)), 100 * ((float) replaced / (forced + replaced)));

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

        public List<Vertex> GetAllVertices()
        {
            var allVertices = new List<Vertex>(CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample);

            int currentSample = 0, sampleCount = 0;
            while (currentSample < CarrierMedia.ByteArray.Length)
            {
                var samples = new byte[CarrierMedia.BytesPerSample];

                for (int i = 0; i < CarrierMedia.BytesPerSample; i++)
                    samples[i] = CarrierMedia.ByteArray[currentSample++];

                allVertices.Add(new Vertex(sampleCount++, samples));
            }

            return allVertices;
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
