using System;
using Stegosaurus.Carrier;
using System.Collections;
using System.Linq;
using Stegosaurus.Exceptions;

namespace Stegosaurus.Algorithm
{
    public class LSBAlgorithm : IStegoAlgorithm
    {
        private static readonly byte[] LsbSignature = { 0x6C, 0x73, 0x62 };

        public ICarrierMedia CarrierMedia { get; set; }

        public string Name => "LSB";

        public int Seed => Key?.GetHashCode() ?? 0;

        public byte[] Key { get; set; }

        public void Embed(StegoMessage _message)
        {
            // Combine LsbSignature with byteArray and convert to bitArray
            BitArray messageInBits = new BitArray(LsbSignature.Concat(_message.ToByteArray(Key)).ToArray());

            // Generate random sequence of integers
            RandomNumberList numberList = new RandomNumberList(Seed, CarrierMedia.ByteArray.Length);
            numberList.AddElements(messageInBits.Length);

            // Iterate through all bits
            for (int index = 0; index < messageInBits.Length; index++)
            {
                int byteArrayIndex = numberList.Next;

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
            RandomNumberList numberList = new RandomNumberList(Seed, CarrierMedia.ByteArray.Length);

            // Generate random numbers for LsbSignature
            numberList.AddElements(LsbSignature.Length * 8);

            // Read bytes and verify LsbSignature
            if (!ReadBytes(numberList, LsbSignature.Length).SequenceEqual(LsbSignature))
                throw new StegoAlgorithmException("LSB Signature is invalid.");

            // Generate 4 * 8 random numbers (one for every bit in the header)
            numberList.AddElements(4 * 8);

            // Read data size
            int dataSize = BitConverter.ToInt32(ReadBytes(numberList, 4), 0);

            // Generate dataSize * 8 numbers (one for every bit in the data)
            numberList.AddElements(dataSize * 8);

            // Return new instance from read data
            return new StegoMessage(ReadBytes(numberList, dataSize), Key);
        }

        public long ComputeBandwidth()
        {
            return (CarrierMedia.ByteArray.Length - LsbSignature.Length) / 8;
        }

        private byte[] ReadBytes(RandomNumberList numberList, int count)
        {
            // Check if there are enough bytes to read
            if (numberList.RemainingElements < count * 8)
                throw new StegoAlgorithmException("Not enough bytes to read.");

            // Allocate BitArray with count * 8 bits
            BitArray tempBitArray = new BitArray(count * 8);

            // Iterate through the allocated amount of bits
            for (int i = 0; i < tempBitArray.Length; i++)
                tempBitArray[i] = ( CarrierMedia.ByteArray[numberList.Next] % 2 == 1 );

            // Copy bitArray to new byteArray
            byte[] tempByteArray = new byte[count];
            tempBitArray.CopyTo(tempByteArray, 0);

            return tempByteArray;
        }


    }
}
