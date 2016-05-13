using System;
using System.Collections.Generic;
using System.Linq;
using Stegosaurus.Utility;
using System.Collections;
using System.ComponentModel;
using Stegosaurus.Exceptions;
using Stegosaurus.Algorithm.CommonSample;
using System.Threading;

namespace Stegosaurus.Algorithm
{
    public class CommonSampleAlgorithm : StegoAlgorithmBase
    {
        public override string Name => "Common Sample";

        protected override byte[] Signature => new byte[] { 0x0C, 0xB3, 0x11, 0x84 };

        /// <summary>
        /// Get or set the maximum distance.
        /// </summary>
        [Category("Algorithm"), Description("The maximum allowed distance between two samples. Higher values may distort the carrier media.")]
        public int MaxDistance { get; set; } = 250;

        /// <summary>
        /// Get or set the maximum sample count.
        /// </summary>
        [Category("Algorithm"), Description("The maximum amount of samples to use. Higher values will take more time to compute.")]
        public int MaxSampleCount { get; set; } = 1000;

        public override void Embed(StegoMessage message, IProgress<int> _progress, CancellationToken _ct)
        {
            var messageBits = new BitArray(Signature.Concat(message.ToByteArray(CryptoProvider)).ToArray());

            // Get all samples.
            List<Sample> samples = GetAllSamples();

            // Find color frequencies.
            // The Samples are clones so their values are not changed by mistake.
            List<Sample> commonFrequencies = samples
                .GroupBy(v => v)
                .OrderByDescending(v => v.Count())
                .Take(MaxSampleCount)
                .Select(s => (Sample)s.Key.Clone())
                .ToList();

            // Find vertices to change.
            RandomNumberList randomNumbers = new RandomNumberList(Seed, samples.Count);
            int numReplaced = 0, numForced = 0;
            for (int i = 0; i < messageBits.Length; i++)
            {
                _ct.ThrowIfCancellationRequested();
                   
                Sample currentSample = samples[randomNumbers.Next];
                int targetValue = messageBits[i] ? 1 : 0;

                // Check if sample already has target value.
                if (currentSample.ModValue == targetValue)
                {
                    continue;
                }

                // Find best match.
                Sample bestMatch = commonFrequencies
                    .Where(s => s.ModValue == targetValue && s.DistanceTo(currentSample) <= MaxDistance)
                    .OrderBy(s => s.LastDistance)
                    .FirstOrDefault(); 

                // If match was found, replace current sample.
                if (bestMatch != null)
                {
                    currentSample.Values = bestMatch.Values;
                    numReplaced++;
                }
                else
                {
                    currentSample.ForceChanges();
                    numForced++;
                }

                // Report progress.
                if (i % 500 != 0)
                    continue;
                float percentage = (( i + 1) / (float) messageBits.Length) * 100;
                _progress?.Report((int) percentage);
            }

            // Report that we are finished.
            _progress?.Report(100);
            Console.WriteLine("{0}% forced, {1}% replaced", 100 * ((float)numForced / (numForced + numReplaced)), 100 * ((float) numReplaced / (numForced + numReplaced)));

            // Write changes.
            int pos = 0;
            foreach (byte sample in samples.SelectMany(current => current.Values))
            {
                CarrierMedia.ByteArray[pos++] = sample;
            }
        }

        public override StegoMessage Extract()
        {
            // Get all samples.
            List<Sample> samples = GetAllSamples();

            // Generate random numbers.
            RandomNumberList randomNumbers = new RandomNumberList(Seed, samples.Count);

            // Read bytes and verify magic header.
            if (!ReadBytes(randomNumbers, samples, Signature.Length).SequenceEqual(Signature))
            {
                throw new StegoAlgorithmException("Magic header is invalid, possibly using a wrong key.");
            }

            // Read length.
            int length = BitConverter.ToInt32(ReadBytes(randomNumbers, samples, 4), 0);

            // Read data and return StegoMessage instance.
            return new StegoMessage(ReadBytes(randomNumbers, samples, length), CryptoProvider);
        }

        public override long ComputeBandwidth()
        {
            return ((CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample) / 8) - Signature.Length;
        }

        /// <summary>
        /// Reads the specified amount of bytes from the CarrierMedia.
        /// </summary>
        private byte[] ReadBytes(RandomNumberList _numberList, List<Sample> samples, int _count)
        {
            // Allocate BitArray with count * 8 bits
            BitArray tempBitArray = new BitArray(_count * 8);

            // Set bits from the values in samples
            for (int i = 0; i < tempBitArray.Length; i++)
            {
                tempBitArray[i] = samples[_numberList.Next].ModValue == 1;
            }

            // Copy bitArray to new byteArray
            byte[] tempByteArray = new byte[_count];
            tempBitArray.CopyTo(tempByteArray, 0);

            return tempByteArray;
        }

        /// <summary>
        /// Returns a list of all samples in the CarrierMedia.
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
