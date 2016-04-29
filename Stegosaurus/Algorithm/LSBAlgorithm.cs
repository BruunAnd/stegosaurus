using System;
using Stegosaurus.Carrier;
using System.Collections;
using System.Linq;
using Stegosaurus.Exceptions;
using Stegosaurus.Utility;
using System.Collections.Generic;
using Stegosaurus.Cryptography;
using Stegosaurus.Utility.Extensions;

namespace Stegosaurus.Algorithm
{
    public class LSBAlgorithm : IStegoAlgorithm
    {
        private static readonly byte[] LsbSignature = { 0x6C, 0x73, 0x62 };

        public ICryptoProvider CryptoProvider { get; set; }
        public ICarrierMedia CarrierMedia { get; set; }

        public string Name => "LSB Algorithm";

        public int Seed => CryptoProvider.CryptoKey.ComputeHash();

        public void Embed(StegoMessage _message)
        {
            // Combine LsbSignature with byteArray and convert to bitArray
            BitArray messageInBits = new BitArray(LsbSignature.Concat(_message.ToByteArray(CryptoProvider)).ToArray());

            // Generate random sequence of integers
            IEnumerable<int> numberList = new RandomNumberList(Seed, CarrierMedia.ByteArray.Length);

            // Iterate through all bits
            for (int index = 0; index < messageInBits.Length; index++)
            {
                int byteArrayIndex = numberList.First();

                // Get the least significant bit of current position
                bool carrierBit = (CarrierMedia.ByteArray[byteArrayIndex] % 2) == 1;

                // Continue if carrierBit corresponds to required bit
                if (carrierBit == messageInBits[index])
                    continue;

                // Change value
                if (carrierBit)
                    CarrierMedia.ByteArray[byteArrayIndex]--;
                else
                    CarrierMedia.ByteArray[byteArrayIndex]++;
            }
        }

        public StegoMessage Extract()
        {
            IEnumerable<int> numberList = new RandomNumberList(Seed, CarrierMedia.ByteArray.Length);

            // Read bytes and verify LsbSignature
            if (!ReadBytes(numberList, LsbSignature.Length).SequenceEqual(LsbSignature))
                throw new StegoAlgorithmException("LSB Signature is invalid.");

            // Read data size
            int dataSize = BitConverter.ToInt32(ReadBytes(numberList, 4), 0);

            // Return new instance from read data
            return new StegoMessage(ReadBytes(numberList, dataSize), CryptoProvider);
        }

        public long ComputeBandwidth()
        {
            return (CarrierMedia.ByteArray.Length / 8 ) - LsbSignature.Length;
        }

        private byte[] ReadBytes(IEnumerable<int> numberList, int count)
        {
            // Allocate BitArray with count * 8 bits
            BitArray tempBitArray = new BitArray(count * 8);

            // Iterate through the allocated amount of bits
            for (int i = 0; i < tempBitArray.Length; i++)
                tempBitArray[i] = ( CarrierMedia.ByteArray[numberList.First()] % 2 == 1 );

            // Copy bitArray to new byteArray
            byte[] tempByteArray = new byte[count];
            tempBitArray.CopyTo(tempByteArray, 0);

            return tempByteArray;
        }
    }
}
