using System;
using System.Collections.Generic;
using System.Linq;
using Stegosaurus.Utility;
using System.Collections;
using System.ComponentModel;
using Stegosaurus.Exceptions;
using System.Threading;

namespace Stegosaurus.Algorithm
{
    public class CommonSampleAlgorithm : StegoAlgorithmBase
    {
        public override string Name => "Common Sample";

        protected override byte[] Signature => new byte[] { 0x0C, 0xB3, 0x11, 0x84 };

        private static readonly byte BitwiseModFactor = 0x1;

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

        private OptionPresets preset = OptionPresets.Default;
        [Category("Algorithm"), Description("The preset settings, which affects the overall speed and quality of the embedding process.")]
        public OptionPresets Preset
        {
            get
            {
                return preset;
            }
            set
            {
                switch (value)
                {
                    case OptionPresets.Default:
                        MaxDistance = 250;
                        MaxSampleCount = 1000;
                        break;
                    case OptionPresets.Imperceptibility:
                        MaxDistance = 200;
                        MaxSampleCount = 2000;
                        break;
                    case OptionPresets.Performance:
                        MaxDistance = 350;
                        MaxSampleCount = 450;
                        break;
                }

                preset = value;
            }
        }

        public override void Embed(StegoMessage _message, IProgress<int> _progress, CancellationToken _ct)
        {
            Random rand = new Random();
            var messageBits = new BitArray(Signature.Concat(_message.ToByteArray(CryptoProvider)).ToArray());

            // Get all samples.
            List<Sample> samples = Sample.GetSampleListFrom(CarrierMedia, BitwiseModFactor);

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
                    currentSample.Values[rand.Next(CarrierMedia.BytesPerSample)] ^= BitwiseModFactor;
                    numForced++;
                }

                // Update mod value.
                currentSample.UpdateModValue(BitwiseModFactor);

                // Report progress.
                if (i % 100 == 0)
                {
                    float percentage = ( ( i + 1 ) / (float) messageBits.Length ) * 100;
                    _progress?.Report((int) percentage);
                }
            }

            // Report that we are finished.
            _progress?.Report(100);
            Console.WriteLine(@"{0}% forced, {1}% replaced", 100 * ((float)numForced / (numForced + numReplaced)), 100 * ((float) numReplaced / (numForced + numReplaced)));

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
            List<Sample> samples = Sample.GetSampleListFrom(CarrierMedia, BitwiseModFactor);

            // Generate random numbers.
            RandomNumberList randomNumbers = new RandomNumberList(Seed, samples.Count);

            // Read bytes and verify magic header.
            if (!ReadBytes(randomNumbers, samples, Signature.Length).SequenceEqual(Signature))
            {
                throw new StegoAlgorithmException($"Signature for {Name} is invalid, possibly using a wrong key.");
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
        private byte[] ReadBytes(RandomNumberList _numberList, List<Sample> _samples, int _count)
        {
            // Allocate BitArray with count * 8 bits
            BitArray tempBitArray = new BitArray(_count * 8);

            // Set bits from the values in samples
            for (int i = 0; i < tempBitArray.Length; i++)
            {
                tempBitArray[i] = _samples[_numberList.Next].ModValue == 1;
            }

            // Copy bitArray to new byteArray
            byte[] tempByteArray = new byte[_count];
            tempBitArray.CopyTo(tempByteArray, 0);

            return tempByteArray;
        }
    }
}
