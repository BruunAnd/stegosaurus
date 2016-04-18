using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Stegosaurus.Carrier;
using System.Collections;
using Stegosaurus.Exceptions;

namespace Stegosaurus.Algorithm
{
    /// <summary>
    /// Placeholder algorithm. Direct linear LSB implementation.
    /// </summary>
    public class PlaceholderAlgorithm : IStegoAlgorithm
    {

        public ICarrierMedia CarrierMedia { get; set; }

        public string Name => "Placeholder Algorithm";

        public long ComputeBandwidth(ICarrierMedia _CarrierMedia)
        {
            return _CarrierMedia.ByteArray.Length / 8;
        }

        public void Embed(StegoMessage _message)
        {
            // Convert byteArray to bitArray
            BitArray messageInBits = new BitArray(_message.ToByteArray());

            // Iterate through all bits
            bool carrierBit;
            for (int index = 0; index < messageInBits.Length; index++)
            {
                // Get the least significant bit of current position
                carrierBit = (CarrierMedia.ByteArray[index] % 2) == 1;

                // Change value of byte if LSB does not correspond
                if (carrierBit != messageInBits[index])
                {
                    if (carrierBit)
                        CarrierMedia.ByteArray[index]--;
                    else
                        CarrierMedia.ByteArray[index]++;
                }
            }
        }

        public StegoMessage Extract()
        {
            int position = 0;

            // Read header size
            int headerSize = BitConverter.ToInt32(ReadBytes(ref position, 4), 0);

            // Return new instance from read data
            return new StegoMessage(ReadBytes(ref position, headerSize));
        }

        /// <summary>
        /// Reads bytes by going through least significant bits
        /// </summary>
        private byte[] ReadBytes(ref int position, int count)
        {
            // Check if there are enough bytes to read
            if (position + count * 8 > CarrierMedia.ByteArray.Length)
                throw new StegoAlgorithmException("Not enough bytes to read.");

            // Allocate memory for count * 8 bits
            BitArray tempBitArray = new BitArray(count * 8);

            // Iterate through the allocated amount of bits
            for (int i = 0; i < tempBitArray.Length; i++)
                tempBitArray[i] = ( CarrierMedia.ByteArray[position++] % 2 == 1 );

            // Copy bitArray to new byteArray
            byte[] tempByteArray = new byte[count];
            tempBitArray.CopyTo(tempByteArray, 0);

            return tempByteArray;
        }


    }
}
