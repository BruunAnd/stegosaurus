using System;
using Stegosaurus.Carrier;
using System.Collections;
using System.Linq;
using Stegosaurus.Exceptions;
using Stegosaurus.Utility;
using System.Collections.Generic;
using Stegosaurus.Cryptography;

namespace Stegosaurus.Algorithm
{
    public class LSBAlgorithm : IStegoAlgorithm
    {
        private static readonly byte[] LsbSignature = { 0x6C, 0x73, 0x62, 0x51 };

        public ICryptoProvider CryptoProvider { get; set; }
        public ICarrierMedia CarrierMedia { get; set; }

        public string Name => "LSB Algorithm";

        public int Seed => CryptoProvider?.Seed ?? 0;

        private const int SelectedBit = 0x1;

        public void Embed(StegoMessage _message)
        {
            // Combine LsbSignature with byteArray and convert to bitArray
            byte[] messageArray = _message.ToByteArray(CryptoProvider);
            BitArray messageInBits = new BitArray(LsbSignature.Concat(messageArray).ToArray());

            // Generate random sequence of integers
            IEnumerable<int> numberList = new RandomNumberList(Seed, CarrierMedia.ByteArray.Length);

            // Iterate through all bits
            for (int index = 0; index < messageInBits.Length; index++)
            {
                int byteArrayIndex = numberList.First();
                byte sampleValue = CarrierMedia.ByteArray[byteArrayIndex];

                // Get the least significant bit of current position
                bool carrierBit = (sampleValue & SelectedBit) == SelectedBit;

                // Flip LSB if no match
                if (carrierBit != messageInBits[index])
                {
                    CarrierMedia.ByteArray[byteArrayIndex] ^= SelectedBit;
                }
            }
        }

        public StegoMessage Extract()
        {
            IEnumerable<int> numberList = new RandomNumberList(Seed, CarrierMedia.ByteArray.Length);

            // Read bytes and verify LsbSignature
            if (!ReadBytes(numberList, LsbSignature.Length).SequenceEqual(LsbSignature))
            {
                throw new StegoAlgorithmException("LSB Signature is invalid.");
            }

            // Read data size
            int dataSize = BitConverter.ToInt32(ReadBytes(numberList, 4), 0);

            // Return new instance from read data
            byte[] encodedData = ReadBytes(numberList, dataSize);
            return new StegoMessage(encodedData, CryptoProvider);
        }

        public long ComputeBandwidth()
        {
            return (CarrierMedia.ByteArray.Length / 8 ) - LsbSignature.Length;
        }

        private byte[] ReadBytes(IEnumerable<int> _numberList, int _count)
        {
            // Allocate BitArray with count * 8 bits
            BitArray tempBitArray = new BitArray(_count * 8);

            // Iterate through the allocated amount of bits
            for (int i = 0; i < tempBitArray.Length; i++)
            {
                tempBitArray[i] = (CarrierMedia.ByteArray[_numberList.First()] & SelectedBit) == SelectedBit;
            }

            // Copy bitArray to new byteArray
            byte[] tempByteArray = new byte[_count];
            tempBitArray.CopyTo(tempByteArray, 0);

            return tempByteArray;
        }
    }
}
