using System;
using System.Collections.Generic;
using System.Linq;
using Stegosaurus.Utility;
using System.Collections;
using System.ComponentModel;
using Stegosaurus.Exceptions;
using Stegosaurus.Algorithm.CommonSample;

namespace Stegosaurus.Algorithm
{
    public class CommonSampleAlgorithm : StegoAlgorithmBase
    {
        private static readonly byte[] CommonSampleSignature = { 0x0C, 0xB3, 0x11, 0x84 };

        public override string Name => "Common Sample";

        [Category("Algorithm"), Description("The maximum allowed distance between two samples. Higher values may distort the carrier media.")]
        public int MaxDistance { get; set; } = 250;

        [Category("Algorithm"), Description("The maximum amount of samples to use. Higher values will take more time to compute.")]
        public int MaxSampleCount { get; set; } = 750;

        public override void Embed(StegoMessage message)
        {
            var messageBits = new BitArray(CommonSampleSignature.Concat(message.ToByteArray(CryptoProvider)).ToArray());

            // Get all vertices
            List<Sample> samples = GetAllSamples();

            // Find color frequencies
            List<Sample> colorFrequencies = samples
                .GroupBy(v => v)
                .OrderByDescending(v => v.Count())
                .Select(s => s.Key)
                .ToList();

            // Find common samples
            List<Sample> commonFrequencies = colorFrequencies
                .Take(MaxSampleCount)
                .ToList();

            // Find vertices to change
            RandomNumberList randomNumbers = new RandomNumberList(Seed, samples.Count);
            int numReplaced = 0, numForced = 0;
            for (int i = 0; i < messageBits.Length; i++)
            {
                Sample currentSample = samples[randomNumbers.First()];
                int targetValue = messageBits[i] ? 1 : 0;

                // Check if sample already has target value
                if (currentSample.ModValue == targetValue)
                {
                    continue;
                }

                // Find best match
                Sample bestMatch = commonFrequencies
                    .Where(s => s.ModValue == targetValue && s.DistanceTo(currentSample) <= MaxDistance)
                    .OrderBy(s => currentSample.DistanceTo(s))
                    .FirstOrDefault() ?? colorFrequencies.FirstOrDefault(s => s.ModValue == targetValue && s.DistanceTo(currentSample) <= MaxDistance);

                // If match was found, replace current sample
                if (bestMatch != null)
                {
                    // Use a clone of the values, as the reference may be changed later
                    currentSample.Values = (byte[]) bestMatch.Values.Clone();
                    numReplaced++;
                }
                else
                {
                    currentSample.ForceChanges();
                    numForced++;
                }

                // Set progress
                if (i % 1002 != 0)
                    continue;
                var p = ((float) (i + 1) / messageBits.Length) * 100;
                Console.WriteLine($"{p}%");
            }

            Console.WriteLine("{0}% forced, {1}% replaced", 100 * ((float)numForced / (numForced + numReplaced)), 100 * ((float) numReplaced / (numForced + numReplaced)));

            // Write changes
            int pos = 0;
            foreach (byte sample in samples.SelectMany(current => current.Values))
            {
                CarrierMedia.ByteArray[pos++] = sample;
            }
        }

        public override StegoMessage Extract()
        {
            // Get all samples
            List<Sample> samples = GetAllSamples();

            // Generate random numbers
            RandomNumberList randomNumbers = new RandomNumberList(Seed, samples.Count);

            // Read bytes and verify CommonSampleSignature
            if (!ReadBytes(randomNumbers, samples, CommonSampleSignature.Length).SequenceEqual(CommonSampleSignature))
            {
                throw new StegoAlgorithmException("Signature is invalid, possibly using a wrong key.");
            }

            // Read length
            int length = BitConverter.ToInt32(ReadBytes(randomNumbers, samples, 4), 0);

            // Read data and return StegoMessage instance
            return new StegoMessage(ReadBytes(randomNumbers, samples, length), CryptoProvider);
        }

        public override long ComputeBandwidth()
        {
            return ((CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample) / 8) - CommonSampleSignature.Length;
        }

        private byte[] ReadBytes(IEnumerable<int> _numberList, List<Sample> samples, int _count)
        {
            // Allocate BitArray with count * 8 bits
            BitArray tempBitArray = new BitArray(_count * 8);

            // Set bits from the values in samples
            for (int i = 0; i < tempBitArray.Length; i++)
            {
                tempBitArray[i] = samples[_numberList.First()].ModValue == 1;
            }

            // Copy bitArray to new byteArray
            byte[] tempByteArray = new byte[_count];
            tempBitArray.CopyTo(tempByteArray, 0);

            return tempByteArray;
        }

        /// <summary>
        /// Returns a list of all samples in the CarrierMedia
        /// </summary>
        public List<Sample> GetAllSamples()
        {
            List<Sample> sampleList = new List<Sample>(CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample);

            int currentSample = 0;
            while (currentSample < CarrierMedia.ByteArray.Length)
            {
                byte[] sampleValues = new byte[CarrierMedia.BytesPerSample];

                for (int i = 0; i < CarrierMedia.BytesPerSample; i++)
                {
                    sampleValues[i] = CarrierMedia.ByteArray[currentSample++];
                }

                sampleList.Add(new Sample(sampleValues));
            }

            return sampleList;
        }
    }
}
