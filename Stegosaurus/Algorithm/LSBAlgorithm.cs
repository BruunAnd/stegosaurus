using System;
using Stegosaurus.Carrier;
using System.Collections;
using System.Linq;
using Stegosaurus.Exceptions;
using Stegosaurus.Utility;
using System.Collections.Generic;
using System.ComponentModel;
using Stegosaurus.Cryptography;
using System.Threading;

namespace Stegosaurus.Algorithm
{
    public class LSBAlgorithm : StegoAlgorithmBase
    {
        /// <summary>
        /// Enum containing the different bit values.
        /// </summary>
        public enum BitValues : byte
        {
            First = 0x1,
            Second = 0x2,
            Third = 0x4,
            Fourth = 0x8,
            Fifth = 0x10,
            Sixth = 0x20,
            Seventh = 0x40,
            Eighth = 0x80,
        }

        public override string Name => "Least Significant Bit";

        protected override byte[] MagicHeader => new byte[] { 0x6C, 0x73, 0x62, 0x51 };

        /// <summary>
        /// Get or set the bit that will be modified or read from.
        /// </summary>
        [Category("Algorithm"), Description("The bit to modify and read from.")]
        public BitValues WorkingBit { get; set; } = BitValues.First;

        public override void Embed(StegoMessage _message, IProgress<int> _progress, CancellationToken _ct)
        {
            // Combine LsbSignature with byteArray and convert to bitArray.
            byte[] messageArray = _message.ToByteArray(CryptoProvider);
            BitArray messageInBits = new BitArray(MagicHeader.Concat(messageArray).ToArray());

            // Generate random sequence of integers.
            RandomNumberList numberList = new RandomNumberList(Seed, CarrierMedia.ByteArray.Length);

            // Iterate through all bits.
            for (int index = 0; index < messageInBits.Length; index++)
            {
                _ct.ThrowIfCancellationRequested();

                int byteArrayIndex = numberList.Next;
                byte sampleValue = CarrierMedia.ByteArray[byteArrayIndex];

                // Get the least significant bit of current position.
                bool carrierBit = (sampleValue & (byte) WorkingBit) == (byte) WorkingBit;

                // Flip LSB if no match.
                if (carrierBit != messageInBits[index])
                {
                    CarrierMedia.ByteArray[byteArrayIndex] ^= (byte) WorkingBit;
                }

                // Report progress.
                if (index % 500 != 0)
                    continue;
                float percentage = ( ( index + 1 ) / (float) messageInBits.Length ) * 100;
                _progress?.Report((int) percentage);
            }

            // Report that we are finished.
            _progress?.Report(100);
        }

        public override StegoMessage Extract()
        {
            RandomNumberList numberList = new RandomNumberList(Seed, CarrierMedia.ByteArray.Length);

            // Read bytes and verify magic header.
            if (!ReadBytes(numberList, MagicHeader.Length).SequenceEqual(MagicHeader))
            {
                throw new StegoAlgorithmException("Magic header is invalid, possibly using a wrong key.");
            }

            // Read data size.
            int dataSize = BitConverter.ToInt32(ReadBytes(numberList, 4), 0);

            // Return new instance from read data.
            byte[] encodedData = ReadBytes(numberList, dataSize);
            return new StegoMessage(encodedData, CryptoProvider);
        }

        public override long ComputeBandwidth()
        {
            return (CarrierMedia.ByteArray.Length / 8 ) - MagicHeader.Length;
        }

        /// <summary>
        /// Reads a specified amount of bytes from the CarrierMedia.
        /// </summary>
        private byte[] ReadBytes(RandomNumberList _numberList, int _count)
        {
            // Allocate BitArray with count * 8 bits
            BitArray tempBitArray = new BitArray(_count * 8);
             
            // Iterate through the allocated amount of bits
            for (int i = 0; i < tempBitArray.Length; i++)
            {
                tempBitArray[i] = (CarrierMedia.ByteArray[_numberList.Next] & (byte) WorkingBit) == (byte) WorkingBit;
            }

            // Copy bitArray to new byteArray
            byte[] tempByteArray = new byte[_count];
            tempBitArray.CopyTo(tempByteArray, 0);

            return tempByteArray;
        }
    }
}
