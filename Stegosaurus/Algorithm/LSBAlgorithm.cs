using System;
using Stegosaurus.Carrier;
using System.Collections;
using Stegosaurus.Exceptions;

namespace Stegosaurus.Algorithm
{
    public class LSBAlgorithm : IStegoAlgorithm
    {
        public ICarrierMedia CarrierMedia { get; set; }

        public string Name => "LSB";

        public byte[] Key { get; set; }

        public int Seed
        {
            get
            {
                return Key == null ? 0 : Key.GetHashCode();
            }
        }

        public void Embed(StegoMessage _message)
        {
            // Convert byteArray to bitArray
            BitArray messageInBits = new BitArray(_message.ToByteArray(Key));

            // Generate random sequence of integers
            RandomNumberList numberList = new RandomNumberList(Seed, CarrierMedia.ByteArray.Length);
            numberList.AddElements(messageInBits.Length);

            // Iterate through all bits
            bool carrierBit;
            for (int index = 0; index < messageInBits.Length; index++)
            {
                int byteArrayIndex = numberList.Next;

                // Get the least significant bit of current position
                carrierBit = (CarrierMedia.ByteArray[byteArrayIndex] % 2) == 1;

                // Change value of byte if LSB does not correspond
                if (carrierBit != messageInBits[index])
                {
                    if (carrierBit)
                        CarrierMedia.ByteArray[byteArrayIndex]--;
                    else
                        CarrierMedia.ByteArray[byteArrayIndex]++;
                }
            }
        }

        public StegoMessage Extract()
        {
            RandomNumberList numberList = new RandomNumberList(Seed, CarrierMedia.ByteArray.Length);

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
            return CarrierMedia.ByteArray.Length / 8;
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
