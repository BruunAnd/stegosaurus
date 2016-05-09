using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stegosaurus.Carrier;
using Stegosaurus.Cryptography;
using Stegosaurus.Utility;
using System.Collections;
using Stegosaurus.Exceptions;
using System.IO;
using Stegosaurus.Utility.Extensions;

namespace Stegosaurus.Algorithm
{
    public class CommonSampleAlgorithm : IStegoAlgorithm
    {
        private static readonly byte[] CommonSampleSignature = new byte[] { 0x0C, 0xB3, 0x11, 0x84 };

        public ICarrierMedia CarrierMedia { get;set; }

        public ICryptoProvider CryptoProvider { get; set; }

        public string Name => "Common Sample";

        public static string ToBitString(BitArray bits)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < bits.Count; i++)
            {
                char c = bits[i] ? '1' : '0';
                sb.Append(c);
            }

            return sb.ToString();
        }

        public void Embed(StegoMessage message)
        {
            var messageBits = new BitArray(CommonSampleSignature.Concat(message.ToByteArray(CryptoProvider)).ToArray());

            // Get all vertices
            List<CommonSample.Sample> samples = GetAllSamples();

            // Find color frequencies
            IEnumerable<CommonSample.Sample> colorFrequencies = samples
                .GroupBy(v => v)
                .OrderByDescending(v => v.Count())
                .Select(s => s.Key);

            // Get amount of unique colors
            int amountOfSamples = colorFrequencies.Count();

            // Find common samples
            List<CommonSample.Sample> commonFrequencies = colorFrequencies
                .Take(Math.Min((int) Math.Floor(amountOfSamples * 0.025), 500))
                .ToList();

            // Find vertices to change
            RandomNumberList randomNumbers = new RandomNumberList(CryptoProvider.Seed, samples.Count);
            int numReplaced = 0, numForced = 0;
            for (int i = 0; i < messageBits.Length; i++)
            {
                CommonSample.Sample currentSample = samples[randomNumbers.First()];
                int targetValue = messageBits[i] ? 1 : 0;

                // Check if it already has target value
                if (currentSample.ModValue == targetValue)
                {
                    continue;
                }

                // Find best match
                IEnumerable<CommonSample.Sample> possibleMatches = commonFrequencies
                    .Where(s =>  s.ModValue == targetValue && s.DistanceTo(currentSample) <= 1000)
                    .OrderBy(s => currentSample.DistanceTo(s));

                // Find best match
                CommonSample.Sample bestMatch = possibleMatches.FirstOrDefault();
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

                // Set progress
                if (i % 1002 != 0)
                    continue;
                var p = ((float) (i + 1) / messageBits.Length) * 100;
                Console.WriteLine($"{p}%");
            }

            Console.WriteLine("{0}% forced, {1}% replaced", 100 * ((float)numForced / (numForced + numReplaced)), 100 * ((float) numReplaced / (numForced + numReplaced)));

            // Write changes
            int pos = 0;
            foreach (CommonSample.Sample current in samples)
            {
                foreach (byte sample in current.Values)
                {
                    CarrierMedia.ByteArray[pos++] = sample;
                }
            }
        }

        public StegoMessage Extract()
        {
            // Get all samples
            List<CommonSample.Sample> samples = GetAllSamples();

            // Generate random numbers
            RandomNumberList randomNumbers = new RandomNumberList(CryptoProvider.Seed, samples.Count);

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

        public long ComputeBandwidth()
        {
            return ((CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample) / 8) - CommonSampleSignature.Length;
        }

        private byte[] ReadBytes(IEnumerable<int> _numberList, List<CommonSample.Sample> samples, int _count)
        {
            // Allocate BitArray with count * 8 bits
            BitArray tempBitArray = new BitArray(_count * 8);

            // Set bits from the values in samples
            for (int i = 0; i < tempBitArray.Length; i++)
            {
                int currentIndex = _numberList.First();
                tempBitArray[i] = samples[currentIndex].ModValue == 1;
            }

            // Copy bitArray to new byteArray
            byte[] tempByteArray = new byte[_count];
            tempBitArray.CopyTo(tempByteArray, 0);

            return tempByteArray;
        }

        /// <summary>
        /// Returns a list of all samples in the CarrierMedia
        /// </summary>
        public List<CommonSample.Sample> GetAllSamples()
        {
            List<CommonSample.Sample> sampleList = new List<CommonSample.Sample>(CarrierMedia.ByteArray.Length / CarrierMedia.BytesPerSample);

            int currentSample = 0;
            while (currentSample < CarrierMedia.ByteArray.Length)
            {
                byte[] sampleValues = new byte[CarrierMedia.BytesPerSample];

                for (int i = 0; i < CarrierMedia.BytesPerSample; i++)
                {
                    sampleValues[i] = CarrierMedia.ByteArray[currentSample++];
                }

                sampleList.Add(new CommonSample.Sample(sampleValues));
            }

            return sampleList;
        }
    }
}
